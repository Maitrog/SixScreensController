using Microsoft.AspNetCore.Mvc;

namespace SixScreenControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Route("/")]
        [HttpGet]
        public string Get()
        {
            return "[GET] /api/screens – Информация обо всех экранах\n" +
                "[POST] /api/screens – Установить шаблон на все экраны\n" +
                "[GET] /api/screens/{screeenNumber} – Информация об экране с номером {screenNumber}\n" +
                "[POST] /api/screens/{screeenNumber} – Установить элемент шаблона на экран с номером {screenNumber}\n" +
                "[GET] /api/screens/{screeenNumber}/content – Контент расположенный на экране с номером {screenNumber}\n\n" +

                "[GET] /api/ScreenTemplates – Информация обо всех шаблонах в базе данных\n" +
                "[POST] /api/ScreenTemplates – Добавить шаблон в базу данных\n" +
                "[GET] /api/ScreenTemplates/{id} – Информация о шаблоне с id {id}\n" +
                "[PUT] /api/ScreenTemplates/{id} – Изменить шаблон с id {id}\n" +
                "[DELETE] /api/ScreenTemplates/{id} – Удалить шаблон с id {id}\n\n" +

                "[GET] /api/ScreenTemplateElements – Информация обо всех элеметах шаблоннов\n" +
                "[GET] /api/ScreenTemplateElements/{id} – Информация об элементе шаблона с id {id}\n" +
                "[POST] /api/ScreenTemplateElements – Добавить элемент шаблона в базу данных\n" +
                "[DELETE] /api/ScreenTemplateElements/{id} – Удалить элемент шаблона из базы данных\n\n" +

                "[GET] /api/Playlist – Информация обо всех плейлистах в базе данных\n" +
                "[GET] /api/Playlist/{id} – Информация о плейлисте с id {id}\n" +
                "[POST] /api/Playlist – Добавить плейлист в базу данных\n" +
                "[DELETE] /api/Playlist/{id} – Удалить плейлист с id {id} из базы данных\n\n" +

                "[GET] /api/PlaylistElement – Информация обо всех элементах плейлистов в базе данных\n" +
                "[GET] /api/Playlist/{id} – Информация об элементе плейлиста с id {id}\n" +
                "[POST] /api/Playlist – Добавить элемент плейлиста в базу данных\n\n" +

                "[HUB] /refresh – SignalR Hub для обновления контента в реальном времени (принимает цифру (номер экрана), и отправляет ее всем клиентпм назад)\n";
        }
    }
}
