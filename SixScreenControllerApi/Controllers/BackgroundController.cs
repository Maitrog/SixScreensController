using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using SixScreenControllerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixScreenControllerApi.Controllers
{
    [Route("api/{controller}")]
    public class BackgroundController : ControllerBase
    {
        private List<string> Backgrounds { get; set; }

        private readonly IMemoryCache cache;

        public BackgroundController(IMemoryCache memoryCache)
        {
            cache = memoryCache;

            if (cache.TryGetValue("Backgrounds", out List<string> backgrounds))
            {
                Backgrounds = backgrounds;
            }
            else
            {
                Backgrounds = new List<string>
                {
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                };
                cache.Set("Backgrounds", Backgrounds);
            }
        }

        [HttpGet("{screenNumber}")]
        public IActionResult Get(int screenNumber)
        {
            string[] imageExp = new string[] { "jpg", "jepg", "bmp", "png", "webp" };
            if (screenNumber < 1 || screenNumber > 6)
            {
                return NotFound();
            }

            string path = Backgrounds[screenNumber - 1];
            string exp = path.Split("\\").LastOrDefault().Split('.').LastOrDefault().ToLower();
            if (imageExp.Contains(exp))
            {
                return PhysicalFile(path, "image/jpeg", enableRangeProcessing: true);
            }
            return BadRequest();
        }

        [HttpPost("{screenNumber}")]
        public ActionResult<Background> Post(int screenNumber, [FromBody] Background background)
        {
            if (screenNumber < 1 || screenNumber > 6)
            {
                return NotFound();
            }

            Backgrounds[screenNumber - 1] = background.Path;
            cache.Set("Backgrounds", Backgrounds);
            Refresh(screenNumber);
            return Ok(background);
        }

        [HttpPost]
        public ActionResult<Background> Post(List<Background> backgrounds)
        {
            for (int i = 1; i < 7; i++)
            {
                Post(i, backgrounds[i - 1]);
            }
            return Ok();
        }

        private async void Refresh(int screenNumber = 0)
        {
            HubConnection hub;
            if (cache.TryGetValue("RefreshUrl", out string url))
            {
                hub = new HubConnectionBuilder().WithUrl(url).WithAutomaticReconnect().Build();
            }
            else
            {
                string protocol = HttpContext.Request.Scheme;
                string host = HttpContext.Request.Host.ToString();
                url = $"{protocol}://{host}/refresh";
                cache.Set("RefreshUrl", url);
                hub = new HubConnectionBuilder().WithUrl(url).WithAutomaticReconnect().Build();
            }
            hub.On<int>("ChangeBackground", screenNumber => Console.WriteLine(screenNumber));

            await hub.StartAsync();

            await hub.SendAsync("ChangeBackground", screenNumber);
        }
    }
}
