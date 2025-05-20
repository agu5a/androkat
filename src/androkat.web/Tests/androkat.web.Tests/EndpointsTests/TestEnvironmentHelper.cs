using System;

namespace androkat.web.Tests.EndpointsTests;

internal static class TestEnvironmentHelper
{
    public static string GetUpdateRadioMusorModelTestsUrl() =>
        Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_UPDATE_RADIO_MUSOR_API_URL");

    public static string GetHealthCheckTestsUrl() =>
        Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_HEALTH_CHECK_API_URL");

    public static string GetCreateTempContentTestsUrl() =>
        Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_SAVE_TEMP_CONTENT_API_URL");

    public static string GetCreateContentDetailsModelTestsUrl() =>
        Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_SAVE_CONTENT_DETAILS_API_URL");

    public static string GetContentsApiTestsUrl() =>
        Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_GET_CONTENTS_API_URL");

    public static string GetXAPIKey() =>
        Environment.GetEnvironmentVariable("ANDROKAT_CREDENTIAL_CRON_API_KEY");
}
