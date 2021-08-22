using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixScreenControllerApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Threading;
using SixScreenControllerApi.Hubs;
using Microsoft.AspNetCore.SignalR.Client;

namespace SixScreenControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        public ScreenTemplate CurrentScreenTemplate { get; set; }
        public ScreenTemplate CurrentState { get; set; }
        private List<bool> IsPlaylisits { get; set; }
        private readonly List<Thread> playlistThreads;


        private readonly IMemoryCache cache;
        public ScreensController(IMemoryCache memoryCache)
        {
            cache = memoryCache;
            if (cache.TryGetValue("CurrentScreenTemplate", out ScreenTemplate st))
            {
                CurrentScreenTemplate = st;
            }
            else
            {
                CurrentScreenTemplate = new ScreenTemplate
                {
                    Id = 0,
                    Title = "default",
                    ScreenTemplateElements = new List<ScreenTemplateElement>(6)
                    {
                        new ScreenTemplateElement(){ ScreenNumber = 1},
                        new ScreenTemplateElement(){ ScreenNumber = 2},
                        new ScreenTemplateElement(){ ScreenNumber = 3},
                        new ScreenTemplateElement(){ ScreenNumber = 4},
                        new ScreenTemplateElement(){ ScreenNumber = 5},
                        new ScreenTemplateElement(){ ScreenNumber = 6}
                    }
                };
                cache.Set("CurrentScreenTemplate", CurrentScreenTemplate);
            }
            if (cache.TryGetValue("CurrentState", out ScreenTemplate curState))
            {
                CurrentState = curState;
            }
            else
            {
                CurrentState = new ScreenTemplate
                {
                    Id = 0,
                    Title = "default",
                    ScreenTemplateElements = new List<ScreenTemplateElement>(6)
                    {
                        new ScreenTemplateElement(){ ScreenNumber = 1},
                        new ScreenTemplateElement(){ ScreenNumber = 2},
                        new ScreenTemplateElement(){ ScreenNumber = 3},
                        new ScreenTemplateElement(){ ScreenNumber = 4},
                        new ScreenTemplateElement(){ ScreenNumber = 5},
                        new ScreenTemplateElement(){ ScreenNumber = 6}
                    }
                };
                cache.Set("CurrentState", CurrentState);
            }
            if (cache.TryGetValue("IsPlaylists", out List<bool> isPlaylists))
            {
                IsPlaylisits = isPlaylists;
            }
            else
            {
                IsPlaylisits = new List<bool> { false, false, false, false, false, false };
                cache.Set("IsPlaylists", IsPlaylisits);
            }

            if (cache.TryGetValue("PlaylistThreads", out List<Thread> _playlilstTreads))
            {
                playlistThreads = _playlilstTreads;
            }
            else
            {
                playlistThreads = new List<Thread>
                {
                    new Thread(SetPlaylist),
                    new Thread(SetPlaylist),
                    new Thread(SetPlaylist),
                    new Thread(SetPlaylist),
                    new Thread(SetPlaylist),
                    new Thread(SetPlaylist)
                };
                cache.Set("PlaylistThreads", playlistThreads);
            }
        }

        [HttpGet]
        public ActionResult<ScreenTemplate> Get()
        {
            return new ObjectResult(CurrentScreenTemplate);
        }

        [HttpGet("{screeenNumber}")]
        public ActionResult<ScreenTemplateElement> Get(int screeenNumber)
        {
            if (screeenNumber < 1 || screeenNumber > 6 || CurrentState.ScreenTemplateElements.Count == 0)
            {
                return NotFound();
            }

            return new ObjectResult(CurrentState.ScreenTemplateElements[screeenNumber - 1]);
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

            if (!CurrentState.ScreenTemplateElements[screeenNumber - 1].IsPlaylist)
            {
                string path = CurrentState.ScreenTemplateElements[screeenNumber - 1].Path;
                string exp = path.Split("\\").LastOrDefault().Split('.').LastOrDefault().ToLower();
                var file = System.IO.File.OpenRead(path);
                if (imageExp.Contains(exp))
                {
                    return File(file, "image/jpeg");
                }
                else if (videoExp.Contains(exp))
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
            CurrentScreenTemplate.Id = screenTemplate.Id;
            CurrentScreenTemplate.Title = screenTemplate.Title;
            cache.Set("CurrentScreenTemplate", CurrentScreenTemplate);

            CurrentState.Id = screenTemplate.Id;
            CurrentState.Title = screenTemplate.Title;
            cache.Set("CurrentState", CurrentState);

            foreach (ScreenTemplateElement i in screenTemplate.ScreenTemplateElements)
            {
                Put(i.ScreenNumber, i);
            }
            return Ok(CurrentState);
        }

        [HttpPut("{screenNumber}")]
        public ActionResult<ScreenTemplate> Put(int screenNumber, ScreenTemplateElement element)
        {
            if (screenNumber < 1 || screenNumber > 6)
            {
                return BadRequest();
            }

            if (IsPlaylisits[screenNumber - 1])
            {
                playlistThreads[screenNumber - 1].Interrupt();
                cache.Set("PlaylistThreads", playlistThreads);
            }

            IsPlaylisits[screenNumber - 1] = false;
            cache.Set("IsPlaylists", IsPlaylisits);

            if (!element.IsPlaylist)
            {

                CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1] = element;
                cache.Set("CurrentScreenTemplate", CurrentScreenTemplate);

                CurrentState.ScreenTemplateElements[screenNumber - 1] = element;
                cache.Set("CurrentState", CurrentState);

                Refresh(screenNumber);
                return Ok(CurrentState);
            }
            else
            {
                CurrentScreenTemplate.ScreenTemplateElements[screenNumber - 1] = element;
                cache.Set("CurrentScreenTemplate", CurrentScreenTemplate);

                Playlist pl = JsonConvert.DeserializeObject<Playlist>(element.Path);
                IsPlaylisits[screenNumber - 1] = true;
                cache.Set("IsPlaylists", IsPlaylisits);

                playlistThreads[screenNumber - 1] = new Thread(SetPlaylist);
                playlistThreads[screenNumber - 1].Start(new Tuple<int, Playlist>(screenNumber, pl));
                cache.Set("PlaylistThreads", playlistThreads);
                return Ok(CurrentState);
            }
        }

        private async void Refresh(int screenNumber = 0)
        {
            HubConnection hub;
            if (cache.TryGetValue("RefreshUrl", out string url))
            {
                hub = new HubConnectionBuilder().WithUrl(url).Build();
            }
            else
            {
                string protocol = HttpContext.Request.Scheme;
                string host = HttpContext.Request.Host.ToString();
                url = $"{protocol}://{host}/refresh";
                cache.Set("RefreshUrl", url);
                hub = new HubConnectionBuilder().WithUrl(url).Build();
            }
            hub.On<int>("Refresh", screenNumber => Console.WriteLine(screenNumber));

            await hub.StartAsync();

            await hub.SendAsync("SendRefresh", screenNumber);
        }

        private ActionResult<ScreenTemplate> PutForPlaylist(int screenNumber, ScreenTemplateElement element)
        {
            if (screenNumber < 1 || screenNumber > 6)
            {
                return BadRequest();
            }

            if (!element.IsPlaylist)
            {
                CurrentState.ScreenTemplateElements[screenNumber - 1] = element;
                cache.Set("CurrentState", CurrentState);
                Refresh(screenNumber);
                return Ok(CurrentState);
            }
            return BadRequest();
        }

        public void SetPlaylist(object data)
        {
            try
            {
                int screenNumber = ((Tuple<int, Playlist>)data).Item1;
                Playlist playlist = ((Tuple<int, Playlist>)data).Item2;
                foreach (PlaylistElement i in playlist.PlaylistElements)
                {
                    ScreenTemplateElement screenTemplateElement = new ScreenTemplateElement { Path = i.Path, IsPlaylist = false, ScreenNumber = screenNumber };
                    PutForPlaylist(screenNumber, screenTemplateElement);

                    Thread.Sleep(i.Duration * 1000);
                    if (!IsPlaylisits[screenNumber - 1])
                    {
                        break;
                    }
                }
            }
            catch (ThreadInterruptedException)
            {

            }
        }
    }
}
