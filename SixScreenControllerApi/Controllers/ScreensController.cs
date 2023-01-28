using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SixScreenControllerApi.Models;

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
            string defaultPath = Directory.GetCurrentDirectory() + "\\assets\\Default.jpg";

            cache = memoryCache;
            if (cache.TryGetValue("CurrentScreenTemplate", out ScreenTemplate st))
            {
                CurrentScreenTemplate = st;
            }
            else
            {
                CurrentScreenTemplate = new ScreenTemplate(defaultPath);
                cache.Set("CurrentScreenTemplate", CurrentScreenTemplate);
            }
            if (cache.TryGetValue("CurrentState", out ScreenTemplate curState))
            {
                CurrentState = curState;
            }
            else
            {
                CurrentState = new ScreenTemplate(defaultPath);
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
                    return PhysicalFile(path, "image/jpeg", enableRangeProcessing: true);
                }
                else if (videoExp.Contains(exp))
                {
                    return PhysicalFile(path, "video/mp4", enableRangeProcessing: true);
                }
                else
                {
                    return PhysicalFile(path, "image/gif", enableRangeProcessing: true);
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

        [HttpGet("{screeenNumber}/type")]
        public ActionResult<string> GetType(int screeenNumber)
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
                if (imageExp.Contains(exp))
                {
                    return new ObjectResult("{\"media_type\":\"img\"}");
                }
                else if (videoExp.Contains(exp))
                {
                    return new ObjectResult("{\"media_type\":\"vid\"}");
                }
                else
                {
                    return new ObjectResult("{\"media_type\":\"gif\"}");
                }
            }

            return BadRequest();

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
                for (int i = 0; i < playlist.PlaylistElements.Count; i++)
                {
                    ScreenTemplateElement screenTemplateElement = new ScreenTemplateElement { Path = playlist.PlaylistElements[i].Path, IsPlaylist = false, ScreenNumber = screenNumber };
                    PutForPlaylist(screenNumber, screenTemplateElement);

                    Thread.Sleep(playlist.PlaylistElements[i].Duration * 1000);
                    if (!IsPlaylisits[screenNumber - 1])
                    {
                        break;
                    }
                    if(i + 1 == playlist.PlaylistElements.Count)
                    {
                        i = 0;
                    }
                }
            }
            catch (ThreadInterruptedException)
            {

            }
        }
    }
}
