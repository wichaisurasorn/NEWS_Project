using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_TEST.Models
{
    public class NewsModels
    {
        private List<News> _LNews = new List<News>();

        public List<News> LNews { get => _LNews; set => _LNews = value; }

        public class News
        {
            public int id { get; set; }
            public string titel { get; set; }
            public string sup_content { get; set; }
            public string main_content { get; set; }
            public string image { get; set; }
        }
    }
}
