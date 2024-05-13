namespace Album.Api.Tests;
using System.Net;
using System.Net.Http;
using Album.Api;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UnitTest1(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async void TestAPIHealth()
    {
        var response = await _client.GetAsync("/api/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async void GreetingTest_NullName()
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
    public async void GreetingTest_GivenName()
    {
        var responseNamed = await _client.GetAsync("/api/hello?name=Test");
        responseNamed.EnsureSuccessStatusCode();
        var contentNamed = await responseNamed.Content.ReadAsStringAsync();
        Assert.NotNull(contentNamed);
        Assert.Contains("Hello Test", JsonConvert.DeserializeObject<Model>(contentNamed).Response);
    }
}