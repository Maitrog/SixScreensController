using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixScreenControllerApi.Models
{
    public class PlaylistElement
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int Duration { get; set; } = 10;

        public int PlaylistId { get; set; }
    }
}
