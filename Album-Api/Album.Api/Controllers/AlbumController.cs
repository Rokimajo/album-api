using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Album.Api.PostgreSQL;
using Album.Api.Services;

namespace Album.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly AWSContext _context;
        private readonly IAlbumService _albumService;

        public AlbumController(AWSContext context, IAlbumService service)
        {
            _context = context;
            _albumService = service;
        }

        // GET: api/Album
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumModel>>> GetAlbums()
        {
            try
            {
                return await _albumService.GetAlbums();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message} {ex.StackTrace}");
            }
        }

        // GET: api/Album/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumModel>> GetAlbumModel(int id)
        {
            var albumModel = await _albumService.GetAlbumModel(id);

            if (albumModel == null)
            {
                return NotFound();
            }

            return albumModel;
        }

        // PUT: api/Album/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbumModel(int id, AlbumModel albumModel)
        {
            if (id != albumModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(albumModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Album
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AlbumModel>> PostAlbumModel(AlbumModel albumModel)
        {
            await _albumService.PostAlbumModel(albumModel);
            return CreatedAtAction("GetAlbumModel", new { id = albumModel.ID }, albumModel);
        }

        // DELETE: api/Album/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbumModel(int id)
        {
            var result = await _albumService.DeleteAlbumModel(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        private bool AlbumModelExists(int id)
        {
            return _albumService.AlbumModelExists(id);
        }
    }
}
