/* Model that is used for the response provided by the Power BI API when pulling an array of workspaces */
using PowerBIEmbedApp.Models.PowerBiModels;

namespace PowerBIEmbedApp.Models.ResponseModels;

public class WorkspacesResponse
{
    public PowerBIWorkspace[]? value { get; set; }
}
