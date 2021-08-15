using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixScreenControllerApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace SixScreenControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        public ScreenTemplate ScreenTemplate { get; set; }
        IMemoryCache cache;
        public ScreensController(IMemoryCache memoryCache)
        {
            cache = memoryCache;
            if (cache.TryGetValue("CurrentScreenTemplate", out ScreenTemplate st))
            {
                ScreenTemplate = st;
            }
            else
            {
                ScreenTemplate template = new ScreenTemplate
                {
                    Id = 0,
                    Title = "default",
                    ScreenTemplateElements = new List<ScreenTemplateElement>(6)
                };
                cache.Set("CurrentScreenTemplate", template);
                ScreenTemplate = template;
            }
        }

        [HttpGet]
        public ActionResult<ScreenTemplate> Get()
        {
            return new ObjectResult(ScreenTemplate);
        }

        [HttpGet("{screeenNumber}")]
        public ActionResult<ScreenTemplateElement> Get(int screeenNumber)
        {
            if (screeenNumber < 1 || screeenNumber > 6 || ScreenTemplate.ScreenTemplateElements.Count == 0)
            {
                return NotFound();
            }

            return new ObjectResult(ScreenTemplate.ScreenTemplateElements[screeenNumber - 1]);
        }

        [HttpGet("{screeenNumber}/content")]
        public IActionResult GetContent(int screeenNumber)
        {
            string[] imageExp = new string[] { "jpg", "jepg", "bmp", "png", "webp" };
            string[] videoExp = new string[] { "mp4", "avi", "mpeg", "mkv", "3gp", "3g2" };
            if (screeenNumber < 1 || screeenNumber > 6)
            {
                return NotFound();
            }

            if (!ScreenTemplate.ScreenTemplateElements[screeenNumber - 1].IsPlaylist)
            {
                string path = ScreenTemplate.ScreenTemplateElements[screeenNumber - 1].Path;
                string exp = path.Split("\\").LastOrDefault().Split('.').LastOrDefault().ToLower();
                var file = System.IO.File.OpenRead(path);
                if (imageExp.Contains(exp))
                {
                    return File(file, "image/jpeg");
                }
                if (videoExp.Contains(exp))
                {
                    return File(file, "video/mp4");
                }
                else
                {
                    return File(file, "image/gif");
                }
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
            cache.Set("CurrentScreenTemplate", ScreenTemplate);
            return Ok(ScreenTemplate);
        }

        [HttpPut("{screenNumber}")]
        public ActionResult<ScreenTemplate> Put(int screenNumber, ScreenTemplateElement element)
        {
            if (screenNumber < 1 || screenNumber > 6)
            {
                return BadRequest();
            }

            ScreenTemplate.ScreenTemplateElements[screenNumber - 1] = element;
            cache.Set("CurrentScreenTemplate", ScreenTemplate);
            return Ok(ScreenTemplate);
        }
    }
}
