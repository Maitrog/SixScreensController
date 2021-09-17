using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixScreenControllerApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SixScreenControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        readonly SixScreenControllerContext db;

        public PlaylistController(SixScreenControllerContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<List<Playlist>> Get()
        {
            return await db.Playlists.Include(p => p.PlaylistElements).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            Playlist playlist = await db.Playlists.Include(p => p.PlaylistElements).FirstOrDefaultAsync(p => p.Id == id);
            if (playlist == null)
            {
                return NotFound();
            }

            return new ObjectResult(playlist);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Playlist playlist)
        {
            if (playlist == null)
            {
                return BadRequest();
            }

            db.Playlists.Add(playlist);
            await db.SaveChangesAsync();

            return Ok(playlist);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Playlist playlist)
        {
            if (playlist == null)
            {
                return BadRequest();
            }

            db.Playlists.Remove(playlist);
            await db.SaveChangesAsync();

            return Ok(playlist);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var playlist = await db.Playlists.FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
            {
                return BadRequest();
            }

            db.Playlists.Remove(playlist);
            await db.SaveChangesAsync();

            return Ok(playlist);
        }
    }
}
