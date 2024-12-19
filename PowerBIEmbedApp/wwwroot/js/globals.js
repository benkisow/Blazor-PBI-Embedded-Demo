// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
// ----------------------------------------------------------------------------

// Cache logged in user's info
const loggedInUser = {
    accessToken: undefined
};

$(function() {
    // Set default state of isPreviousReportRDL flag
    UserOwnsData.isPreviousReportRDL = false;

    // Cache base endpoint for Power BI REST API
    UserOwnsData.powerBiApi = "https://api.powerbi.com/v1.0/myorg/";

    UserOwnsData.powerBiHostname = "https://app.powerbi.com";
});
