using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixScreenControllerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixScreenControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        public ScreenTemplate ScreenTemplate { get; set; }
        public ScreensController()
        {
            ScreenTemplate = new ScreenTemplate { Id = 0, Title = "Defaulte" };
            ScreenTemplate.ScreenTemplateElements.Insert(0, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 1, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
            ScreenTemplate.ScreenTemplateElements.Insert(1, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 2, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
            ScreenTemplate.ScreenTemplateElements.Insert(2, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 3, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
            ScreenTemplate.ScreenTemplateElements.Insert(3, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 4, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
            ScreenTemplate.ScreenTemplateElements.Insert(4, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 5, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
            ScreenTemplate.ScreenTemplateElements.Insert(5, new ScreenTemplateElement { Id = 0, IsPlaylist = false, ScreenNumber = 6, ScreenTemplateId = 0, Path = "S:\\Documents\\Миша\\Фото\\Райз.png" });
        }

        [HttpGet]
        public ActionResult<ScreenTemplate> Get()
        {
            return new ObjectResult(ScreenTemplate);
        }

        [HttpGet("{screeenNumber}")]
        public ActionResult<ScreenTemplateElement> Get(int screeenNumber)
        {
            if (screeenNumber < 1 || screeenNumber > 6)
            {
                return NotFound();
            }

            return new ObjectResult(ScreenTemplate.ScreenTemplateElements[screeenNumber - 1]);
        }

        [HttpGet("{screeenNumber}/content")]
        public IActionResult GetContent(int screeenNumber)
        {
            if (screeenNumber < 1 || screeenNumber > 6)
            {
                return NotFound();
            }

            if (!ScreenTemplate.ScreenTemplateElements[screeenNumber - 1].IsPlaylist)
            {
                var image = System.IO.File.OpenRead(ScreenTemplate.ScreenTemplateElements[screeenNumber - 1].Path);
                return File(image, "image/jpeg");
            }

            return BadRequest();
        }

        [HttpPost]
        public ActionResult<ScreenTemplate> Post(ScreenTemplate screenTemplate)
        {
            if (screenTemplate == null)
            {
                return BadRequest();
            }

            ScreenTemplate = screenTemplate;
            return Ok(ScreenTemplate);
        }

        [HttpPut("{screeenNumber}")]
        public ActionResult<ScreenTemplate> Put(int screenNumber, ScreenTemplateElement element)
        {
            if (screenNumber < 1 || screenNumber > 6)
            {
                return BadRequest();
            }

            ScreenTemplate.ScreenTemplateElements.Insert(screenNumber - 1, element);
            return Ok(ScreenTemplate);
        }
    }
}
