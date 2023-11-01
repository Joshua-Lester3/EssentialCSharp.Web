using Microsoft.Extensions.DependencyInjection;
using System.Net;
using EssentialCSharp.Web.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EssentialCSharp.Web.Extensions.Tests.Integration;

public class CaptchaTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CaptchaTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CaptchaService_Verify_Success()
    {
        ICaptchaService captchaService = _factory.Services.GetRequiredService<ICaptchaService>();

        // From https://docs.hcaptcha.com/#integration-testing-test-keys
        string hCaptchaSecret = "0x0000000000000000000000000000000000000000";
        string hCaptchaToken = "10000000-aaaa-bbbb-cccc-000000000001";
        string hCaptchaSiteKey = "10000000-ffff-ffff-ffff-000000000001";
        HttpResponseMessage response = await captchaService.Verify(hCaptchaSecret, hCaptchaToken, hCaptchaSiteKey);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}