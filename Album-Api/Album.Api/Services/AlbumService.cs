using Album.Api.PostgreSQL;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Album.Api.Services;

public class AlbumService : IAlbumService
{
    private readonly AWSContext _context;

    public AlbumService(AWSContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<IEnumerable<AlbumModel>>> GetAlbums()
    {
        return await _context.Albums.ToListAsync();
    }
    
    public async Task<ActionResult<AlbumModel>> GetAlbumModel(int id)
    {
        return await _context.Albums.FindAsync(id);
    }
    
    public async Task PostAlbumModel(AlbumModel albumModel)
    {
        _context.Albums.Add(albumModel);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAlbumModel(int id)
    {
        var albumModel = await _context.Albums.FindAsync(id);
        if (albumModel == null)
        {
            return true;
        }

        _context.Albums.Remove(albumModel);
        await _context.SaveChangesAsync();

        return false;
    }
    
    public bool AlbumModelExists(int id)
    {
        return _context.Albums.Any(e => e.ID == id);
    }
}