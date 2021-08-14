using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixScreenControllerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixScreenControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistElementController : ControllerBase
    {
        readonly SixScreenControllerContext db;

        public PlaylistElementController(SixScreenControllerContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<List<PlaylistElement>> Get()
        {
            return await db.PlaylistElements.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var element = await db.PlaylistElements.FirstOrDefaultAsync(e => e.Id == id);
            if (element == null)
            {
                return BadRequest();
            }

            return new ObjectResult(element);
        }

        [HttpPost]
        public async Task<ActionResult> Post(PlaylistElement element)
        {
            if(element == null)
            {
                return BadRequest();
            }

            db.PlaylistElements.Add(element);
            await db.SaveChangesAsync();

            return Ok(element);
        }
    }
}
