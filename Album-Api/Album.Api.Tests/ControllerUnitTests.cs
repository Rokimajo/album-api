using Album.Api.Services;

namespace Album.Api.Tests;
using System.Net;
using System.Net.Http;
using Album.Api;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

public class ControllerUnitTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ControllerUnitTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async void GivenAPI_WhenCheckHealth_ThenReturnOK()
    {
        var response = await _client.GetAsync("/api/health");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Theory]
    [InlineData("/api/hello")]
    [InlineData("/api/hello?name=")]
    [InlineData("/api/hello?name= ")]
    public async Task GetGreeting_WhenGetHelloWithNullOrEmptyName_ThenReturnHelloWorld(string url)
    {
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.NotNull(content);
        Assert.Contains("Hello World", JsonConvert.DeserializeObject<Model>(content).Response);
    }

    [Fact]
    public async void GetGreeting_WhenGetHelloWithName_ThenReturnHelloName()
    {
        var name = "Test";
        var responseNamed = await _client.GetAsync($"/api/hello?name={name}");
        responseNamed.EnsureSuccessStatusCode();
        var contentNamed = await responseNamed.Content.ReadAsStringAsync();
        Assert.NotNull(contentNamed);
        Assert.Equal($"Hello {name}", JsonConvert.DeserializeObject<Model>(contentNamed).Response);
    }

    [Fact]
    public async Task GetGreeting_WhenGetHelloWithSpecialCharacters_ThenReturnHelloName()
    {
        var name = "Test@123";
        var response = await _client.GetAsync($"/api/hello?name={name}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
        Assert.Equal($"Hello {name}", JsonConvert.DeserializeObject<Model>(content).Response);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetGreeting_WhenGetHelloWithName_ThenReturnContentTypeJson()
    {
        var response = await _client.GetAsync("/api/hello?name=Test");
        response.EnsureSuccessStatusCode();
        Assert.NotNull(response);
        Assert.Equal("application/json; charset=utf-8", 
                    response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetGreeting_WhenGetHelloSadFlow_ThenReturnInternalServerError()
    {
        var response = await _client.GetAsync("/api/sadflow?name=Test");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}