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
    public class ScreenTemplateElementsController : ControllerBase
    {
        readonly SixScreenControllerContext db;

        public ScreenTemplateElementsController(SixScreenControllerContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ScreenTemplateElement>>> Get()
        {
            return await db.ScreenTemplateElements.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<ScreenTemplateElement>>> Get(int id)
        {
            var element = await db.ScreenTemplateElements.FirstOrDefaultAsync(x => x.Id == id);
            if (element == null)
            {
                return NotFound();
            }

            return new ObjectResult(element);
        }

        [HttpPost]
        public async Task<ActionResult<ScreenTemplateElement>> Post(ScreenTemplateElement screenTemplateElement)
        {
            if (screenTemplateElement == null)
            {
                return BadRequest();
            }

            db.ScreenTemplateElements.Add(screenTemplateElement);
            await db.SaveChangesAsync();
            return Ok(screenTemplateElement);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ScreenTemplateElement>> Delete(int id)
        {
            ScreenTemplateElement screenTemplateElement = db.ScreenTemplateElements.FirstOrDefault(x => x.Id == id);
            if (screenTemplateElement == null)
            {
                return NotFound();
            }

            db.ScreenTemplateElements.Remove(screenTemplateElement);
            await db.SaveChangesAsync();
            return Ok(screenTemplateElement);
        }
    }
}
