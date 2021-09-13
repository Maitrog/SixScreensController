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
            return "[GET] /screens – Информация обо всех экранах\n" +
                "[POST] /screens – Установить шаблон на все экраны\n" +
                "[GET] /screens/{screeenNumber} – Информация об экране с номером {screenNumber}\n" +
                "[POST] /screens/{screeenNumber} – Установить элемент шаблона на экран с номером {screenNumber}\n" +
                "[GET] /screens/{screeenNumber}/content – Контент расположенный на экране с номером {screenNumber}\n\n" +

                "[GET] /ScreenTemplates – Информация обо всех шаблонах в базе данных\n" +
                "[POST] /ScreenTemplates – Добавить шаблон в базу данных\n" +
                "[GET] /ScreenTemplates/{id} – Информация о шаблоне с id {id}\n" +
                "[PUT] /ScreenTemplates/{id} – Изменить шаблон с id {id}\n" +
                "[DELETE] /ScreenTemplates/{id} – Удалить шаблон с id {id}\n\n" +

                "[GET] /ScreenTemplateElements – Информация обо всех элеметах шаблоннов\n" +
                "[GET] /ScreenTemplateElements/{id} – Информация об элементе шаблона с id {id}" +
                "[POST] /ScreenTemplateElements – Добавить элемент шаблона в базу данных\n" +
                "[DELETE] /ScreenTemplateElements/{id} – Удалить элемент шаблона из базы данных\n\n" +

                "[GET] /Playlist – Информация обо всех плейлистах в базе данных\n" +
                "[GET] /Playlist/{id} – Информация о плейлисте с id {id}\n" +
                "[POST] /Playlist – Добавить плейлист в базу данных\n" +
                "[DELETE] /Playlist/{id} – Удалить плейлист с id {id} из базы данных\n\n" +

                "[GET] /PlaylistElement – Информация обо всех элементах плейлистов в базе данных\n" +
                "[GET] /Playlist/{id} – Информация об элементе плейлиста с id {id}\n" +
                "[POST] /Playlist – Добавить элемент плейлиста в базу данных\n";
        }
    }
}
