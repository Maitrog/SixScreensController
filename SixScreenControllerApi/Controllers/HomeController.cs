using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixScreenControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "/screens – Информация обо всех экранах\n" +
                "/screens/{screeenNumber} – Информация об экране с номером {screenNumber}\n" +
                "/screens/{screeenNumber}/{content} – Контент расположенный на экране с номером {screenNumber}\n" +
                "/ScreenTemplates – Информация обо всех шаблонах в базе данных\n" +
                "/ScreenTemplates/{id} – Информация о шаблоне с id {id}\n" +
                "/ScreenTemplateElements – Информация обо всех элеметах шаблоннов\n" +
                "/ScreenTemplateElements/{id} – Информация об элементе шаблона с id {id}";
        }
    }
}
