/* Model that is used for the response provided by the Power BI API when pulling an array of reports */
using PowerBIEmbedApp.Models.PowerBiModels;

namespace PowerBIEmbedApp.Models.ResponseModels;

public class ReportsResponse
{
    public PowerBIReport[]? value { get; set; }
}
