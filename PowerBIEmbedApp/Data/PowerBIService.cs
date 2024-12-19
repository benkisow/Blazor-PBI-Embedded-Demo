/* This is a service used for authorizing with Power BI, as well as pulling workspaces and reports */
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using PowerBIEmbedApp.Models;
using PowerBIEmbedApp.Models.PowerBiModels;
using PowerBIEmbedApp.Models.ResponseModels;

namespace PowerBIEmbedApp.Data;

[AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
public class PowerBIService
{
    private const string POWER_BI_API = "https://api.powerbi.com/v1.0/myorg";

    private static HttpClient httpClient = new HttpClient();

    private readonly ILogger<PowerBIService> _logger;

    private readonly GraphServiceClient m_graphServiceClient;

    private readonly ITokenAcquisition m_tokenAcquisition;

    public AuthDetails? AuthDetails;

    public event EventHandler AuthDetailsStateChangedHandler;

    private void AuthDetailsStateHasChanged()
    {
        AuthDetailsStateChangedHandler?.Invoke(this, EventArgs.Empty);
    }

    public PowerBIReport? EmbeddedReport;

    public event EventHandler EmbeddedReportStateChangedHandler;

    private void EmbeddedReportStateHasChanged()
    {
        EmbeddedReportStateChangedHandler?.Invoke(this, EventArgs.Empty);
    }

    // Method to set embedded report
    public async void SetEmbeddedReport(string reportId)
    {
        PowerBIReport report = await GetReportByIdAsync(reportId);
        this.EmbeddedReport = report;
        EmbeddedReportStateHasChanged();
    }

    public IConfiguration Configuration { get; }    
    
    public PowerBIService(  ILogger<PowerBIService> logger,
                            GraphServiceClient graphServiceClient,
                            ITokenAcquisition tokenAcquisition,
                            IConfiguration configuration)
    {
        this._logger = logger;
        this.m_graphServiceClient = graphServiceClient;
        this.m_tokenAcquisition = tokenAcquisition;
        this.Configuration = configuration;
    }

    // Method for getting auth details
    public async Task<AuthDetails> GetAuthDetailsAsync()
    {
        _logger.LogInformation("RUNNING GetAuthDetailsAsync()");

        User userInfo;
        string userName = "";

        try
        {
            // Get username of logged in user
            userInfo = await m_graphServiceClient.Me.Request().GetAsync();
            userName = userInfo.DisplayName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        _logger.LogInformation("USER NAME: {userName}", userName);

        var accessToken = "";

        // Generate token for the signed in user
        try
        {
            var scopes = Configuration["AzureAd:Scopes:0"].Split(" ");

            accessToken = await m_tokenAcquisition.GetAccessTokenForUserAsync(scopes);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting access token\n", ex);
        }

        _logger.LogInformation("ACCESS TOKEN: {accessToken}", accessToken.ToString());

        AuthDetails authDetails = new AuthDetails
        {
            UserName = userName,
            AccessToken = accessToken.ToString()
        };

        // Add access token to http client header
        httpClient.DefaultRequestHeaders.Authorization
            = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authDetails.AccessToken);

        this.AuthDetails = authDetails;
        AuthDetailsStateHasChanged();

        return await Task.FromResult(authDetails);
    }

    // Method for refreshing user permissions
    public async void tryRefreshUserPermissions()
    {
        // API Endpoint to refresh user permissions
        string permissionsRefreshEndpoint = $"{POWER_BI_API}/RefreshUserPermissions";

        HttpResponseMessage response = await httpClient.PostAsync(permissionsRefreshEndpoint, null);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Permissions refreshed successfully");
        }
        else if ((int)response.StatusCode == 429)
        {
            _logger.LogInformation("Permissions refresh will be available in up to an hour");
        }
        else
        {
            _logger.LogError("An error occured while trying to refresh user permissions\n{responseContent}", response.Content);
        }
    }

    // Method for getting all workspaces and returning an array of workspaces
    public async Task<PowerBIWorkspace[]> GetWorkspacesAsync()
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{POWER_BI_API}/groups");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<WorkspacesResponse>(jsonString);

            PowerBIWorkspace[] workspaces = jsonObject.value;

            return workspaces;
        }
        else
        {
            return null;
        }
    }

    // Method for getting all reports from My Workspace and returning an array of reports
    public async Task<PowerBIReport[]> GetReportsAsync()
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{POWER_BI_API}/reports");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<ReportsResponse>(jsonString);

            PowerBIReport[] reports = jsonObject.value;

            return reports;
        }
        else
        {
            return null;
        }

    }

    // Method for getting all reports in a group and returning an array of reports
    public async Task<PowerBIReport[]> GetReportsInGroupAsync(string groupId)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{POWER_BI_API}/groups/{groupId}/reports");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<ReportsResponse>(jsonString);

            PowerBIReport[] reports = jsonObject.value;

            return reports;
        }
        else
        {
            return null;
        }

    }

    // Method for getting a single report, by Id
    public async Task<PowerBIReport> GetReportByIdAsync(string reportId)
    {
        if (reportId == null)
        {
            return null;
        }
        HttpResponseMessage response = await httpClient.GetAsync($"{POWER_BI_API}/reports/{reportId}");

        if (response.IsSuccessStatusCode )
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<PowerBIReport>(jsonString);

            PowerBIReport report = jsonObject;

            return report;
        } else
        {
            return null;
        }
    }
}