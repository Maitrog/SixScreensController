using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixScreenControllerApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixScreenControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenTemplatesController : ControllerBase
    {
        readonly SixScreenControllerContext db;

        public ScreenTemplatesController(SixScreenControllerContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ScreenTemplate>>> Get()
        {
            return await db.ScreenTemplates.Include(x => x.ScreenTemplateElements).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScreenTemplate>> Get(int id)
        {
            var template = await db.ScreenTemplates.Include(x => x.ScreenTemplateElements).FirstOrDefaultAsync(x => x.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return template;
        }

        [HttpPost]
        public async Task<ActionResult<ScreenTemplate>> Post(ScreenTemplate screenTemplate)
        {
            if (screenTemplate == null)
            {
                return BadRequest();
            }

            db.ScreenTemplates.Add(screenTemplate);
            await db.SaveChangesAsync();
            return Ok(screenTemplate);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ScreenTemplate>> Put(int id, ScreenTemplate _screenTemplate)
        {
            ScreenTemplate screenTemplate = await db.ScreenTemplates.Include(x => x.ScreenTemplateElements).FirstOrDefaultAsync(x => x.Id == id);
            if (_screenTemplate == null)
            {
                return BadRequest();
            }
            if (screenTemplate == null)
            {
                return NotFound();
            }

            screenTemplate.Title = _screenTemplate.Title;
            for (int i = 0; i < 6; i++)
            {
                if (screenTemplate.ScreenTemplateElements[i] != _screenTemplate.ScreenTemplateElements[i])
                {
                    screenTemplate.ScreenTemplateElements[i] = _screenTemplate.ScreenTemplateElements[i];
                }
            }
            await db.SaveChangesAsync();

            return Ok(_screenTemplate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ScreenTemplate>> Delete(int id)
        {
            ScreenTemplate screenTemplate = db.ScreenTemplates.FirstOrDefault(x => x.Id == id);
            if (screenTemplate == null)
            {
                return NotFound();
            }

            db.ScreenTemplates.Remove(screenTemplate);
            await db.SaveChangesAsync();
            return Ok(screenTemplate);
        }
    }
}
