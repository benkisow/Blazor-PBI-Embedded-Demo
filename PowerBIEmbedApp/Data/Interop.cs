/* This interop exists to let the app run javascript, since the power bi javascript library has the necessary functions for embedding */
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PowerBIEmbedApp.Data;

public static class Interop
{
    internal static ValueTask<object> EmbedReport(IJSRuntime jsRuntime, string embedUrl, string accessToken, ElementReference powerBiElement, ElementReference spinnerElement)
    {
        return jsRuntime.InvokeAsync<object>("PowerBIFunctions.showReport", embedUrl, accessToken, powerBiElement, spinnerElement);
    }

    internal static void Bootstrap(IJSRuntime jsRuntime)
    {
        jsRuntime.InvokeVoidAsync("PowerBIFunctions.bootstrap");
    }
}
