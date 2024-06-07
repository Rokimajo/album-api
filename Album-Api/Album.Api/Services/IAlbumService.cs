using Album.Api.PostgreSQL;
using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Services;

public interface IAlbumService
{
    public Task<ActionResult<IEnumerable<AlbumModel>>> GetAlbums();
    public Task<ActionResult<AlbumModel>> GetAlbumModel(int id);
    public Task PostAlbumModel(AlbumModel albumModel);
    public Task<bool> DeleteAlbumModel(int id);
    public bool AlbumModelExists(int id);

}