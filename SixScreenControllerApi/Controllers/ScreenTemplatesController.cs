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
    public class ScreenTemplatesController : ControllerBase
    {
        readonly SixScreenControllerContext db;

        public ScreenTemplatesController(SixScreenControllerContext context)
        {
            db = context;

            if (db.ScreenTemplates.Count() < 1)
            {
                var ScreenTemplate = new ScreenTemplate { Title = "notDefaulte" };
                ScreenTemplate.ScreenTemplateElements.Insert(0, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 1, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
                ScreenTemplate.ScreenTemplateElements.Insert(1, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 2, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
                ScreenTemplate.ScreenTemplateElements.Insert(2, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 3, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
                ScreenTemplate.ScreenTemplateElements.Insert(3, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 4, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
                ScreenTemplate.ScreenTemplateElements.Insert(4, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 5, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
                ScreenTemplate.ScreenTemplateElements.Insert(5, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 6, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });

                db.ScreenTemplates.Add(ScreenTemplate);
                db.SaveChanges();
            }
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
