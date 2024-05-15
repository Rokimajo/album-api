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
    public async void TestAPIHealth_HappyFlow()
    {
        var response = await _client.GetAsync("/api/health");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async void GreetingTest_Null_And_Empty_Name_HappyFlow()
    {
        var responseNull = await _client.GetAsync("/api/hello");
        var responseEmpty = await _client.GetAsync("/api/hello?name=");
        var responseWhitespace = await _client.GetAsync("/api/hello?name= ");
        responseNull.EnsureSuccessStatusCode();
        responseEmpty.EnsureSuccessStatusCode();
        responseWhitespace.EnsureSuccessStatusCode();
        
        var contentNull = await responseNull.Content.ReadAsStringAsync();
        var contentEmpty = await responseEmpty.Content.ReadAsStringAsync();
        var contentWhitespace = await responseWhitespace.Content.ReadAsStringAsync();

        Assert.NotNull(contentNull);
        Assert.NotNull(contentEmpty);
        Assert.NotNull(contentWhitespace);

        Assert.Contains("Hello World", JsonConvert.DeserializeObject<Model>(contentNull).Response);
        Assert.Contains("Hello World", JsonConvert.DeserializeObject<Model>(contentEmpty).Response);
        Assert.Contains("Hello World", JsonConvert.DeserializeObject<Model>(contentWhitespace).Response);
    }

    [Fact]
    public async void GreetingTest_NameGiven_HappyFlow()
    {
        var name = "Test";
        var responseNamed = await _client.GetAsync($"/api/hello?name={name}");
        responseNamed.EnsureSuccessStatusCode();
        var contentNamed = await responseNamed.Content.ReadAsStringAsync();
        Assert.NotNull(contentNamed);
        Assert.Contains($"Hello {name}", JsonConvert.DeserializeObject<Model>(contentNamed).Response);
    }

    [Fact]
    public async Task GreetingTest_SpecialCharacters_HappyFlow()
    {
        var name = "Test@123";
        var response = await _client.GetAsync($"/api/hello?name={name}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
        Assert.Contains($"Hello {name}", JsonConvert.DeserializeObject<Model>(content).Response);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TestContentType_HappyFlow()
    {
        var response = await _client.GetAsync("/api/hello?name=Test");
        response.EnsureSuccessStatusCode();
        Assert.NotNull(response);
        Assert.Equal("application/json; charset=utf-8", 
                    response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GreetingTestError_SadFlow()
    {
        var response = await _client.GetAsync("/api/sadflow?name=Test");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}