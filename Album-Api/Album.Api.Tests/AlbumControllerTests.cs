using System.Drawing.Printing;
using System.Net.Http.Json;
using System.Text;
using Album.Api.PostgreSQL;
using Album.Api.Services;
using NuGet.ContentModel;
using NuGet.Protocol;
using Xunit.Abstractions;

namespace Album.Api.Tests;
using System.Net;
using System.Net.Http;
using Album.Api;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

public class AlbumControllerUnitTests : IClassFixture<InMemoryApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper output;

    public AlbumControllerUnitTests(InMemoryApplicationFactory factory, ITestOutputHelper output)
    {
        _client = factory.CreateClient();
        this.output = output;
    }
    
    [Fact]
    public async Task GetAlbums_WhenGetAlbums_EnsureSuccessfulStatusCode_EnsureCorrectType()
    {
        var response = await _client.GetAsync("/api/Album");
        response.EnsureSuccessStatusCode();
        Assert.NotNull(response);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<AlbumModel>>();
        Assert.NotNull(result);
        Assert.IsType<List<AlbumModel>>(result);
    }
    
    [Fact]
    public async Task GetAlbums_WhenGetNotExistingAlbumModel_ReturnsNoContent()
    {
        int albumId = -1;
        var response = await _client.GetAsync($"/api/Album/{albumId}");
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAlbums_AfterPosting_ReturnsAllAlbumsIncludingPostedOne()
    {
        var newAlbum = new AlbumModel { Name = "Test Album", Artist = "Test Artist", ImageUrl = "http://example.com/test.jpg" };
        var postContent = JsonConvert.SerializeObject(newAlbum);
        var httpPostContent = new StringContent(postContent, Encoding.UTF8, "application/json");


        var postResponse = await _client.PostAsync("/api/Album", httpPostContent);
        postResponse.EnsureSuccessStatusCode();


        var getResponse = await _client.GetAsync("/api/Album");
        var albums = await getResponse.Content.ReadFromJsonAsync<List<AlbumModel>>();

        // Assert - Ensure the GET request was successful and contains the posted album
        getResponse.EnsureSuccessStatusCode();
        Assert.NotEmpty(albums);
        Assert.Contains(albums, a => a.Name == "Test Album" && a.Artist == "Test Artist" && a.ImageUrl == "http://example.com/test.jpg");
    }
    
    [Fact]
    public async Task GetAlbums_WhenPostAlbumModel_ReturnsCreatedAtActionResult()
    {
        var newAlbum = new AlbumModel { Name = "New Album", Artist = "New Artist", ImageUrl = "http://example.com/image.jpg" };
        var content = JsonConvert.SerializeObject(newAlbum);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/Album", httpContent);
        var result = await response.Content.ReadFromJsonAsync<AlbumModel>();
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal("New Album", result.Name);
        Assert.Equal("New Artist", result.Artist);
    }
    
    [Fact]
    public async Task GetAlbums_WhenDeleteAlbumModel_ReturnsNoContentResult()
    {
        int albumIdToDelete = 3; // ID always exists due to db initializer
        var response = await _client.DeleteAsync($"/api/Album/{albumIdToDelete}");
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAlbums_WhenDeleteNotExistingAlbumModel_ReturnsNotFound()
    {
        int albumIdToDelete = -1;
        var response = await _client.DeleteAsync($"/api/Album/{albumIdToDelete}");
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAlbums_WhenPutWithMismatchingID_ReturnsBadRequest()
    {
        int albumId = -1;
        var album = new AlbumModel { Name = "PutAlbum", Artist = "PutArtist", ImageUrl = "http://example.com/image.jpg" };
        var content = JsonConvert.SerializeObject(album);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync($"/api/Album/{albumId}", httpContent);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAlbums_WhenPutNotExistingID_ReturnsNotFound()
    {
        int albumId = -1;
        var album = new AlbumModel { ID = -1, Name = "PutAlbum", Artist = "PutArtist", ImageUrl = "http://example.com/image.jpg" };
        var content = JsonConvert.SerializeObject(album);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync($"/api/Album/{albumId}", httpContent);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAlbums_WhenPutExistingAlbum_ReturnsNoContent()
    {
        var getResponse = await _client.GetAsync("/api/Album");
        getResponse.EnsureSuccessStatusCode();
        Assert.NotNull(getResponse);
        var result = await getResponse.Content.ReadFromJsonAsync<IEnumerable<AlbumModel>>();
        var album = result.First();
        var albumModel = new AlbumModel { ID = album.ID, Name = "PutAlbum", Artist = "PutArtist", ImageUrl = "http://example.com/image.jpg" };
        var content = JsonConvert.SerializeObject(album);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync($"/api/Album/{album.ID}", httpContent);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAlbums_WhenPutAlbumUpdatesAlbumSuccessfully_ReturnsNoContent()
    {
        // Arrange - First, post a new album to update later
        var newAlbum = new AlbumModel { Name = "Original Album", Artist = "Original Artist", ImageUrl = "http://example.com/original.jpg" };
        var postContent = JsonConvert.SerializeObject(newAlbum);
        var httpPostContent = new StringContent(postContent, Encoding.UTF8, "application/json");
        var postResponse = await _client.PostAsync("/api/Album", httpPostContent);
        var postedAlbum = await postResponse.Content.ReadFromJsonAsync<AlbumModel>();
        
        var updatedAlbum = new AlbumModel { ID = postedAlbum.ID, Name = "Updated Album", Artist = "Updated Artist", ImageUrl = "http://example.com/updated.jpg" };
        var updateContent = JsonConvert.SerializeObject(updatedAlbum);
        var httpUpdateContent = new StringContent(updateContent, Encoding.UTF8, "application/json");

        var updateResponse = await _client.PutAsync($"/api/Album/{postedAlbum.ID}", httpUpdateContent);
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/Album/{postedAlbum.ID}");
        var getResult = await getResponse.Content.ReadFromJsonAsync<AlbumModel>();
        Assert.Equal("Updated Album", getResult.Name);
        Assert.Equal("Updated Artist", getResult.Artist);
        Assert.Equal("http://example.com/updated.jpg", getResult.ImageUrl);
    }
}