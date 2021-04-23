using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_TEST.Models;
using static MVC_TEST.Models.NewsModels;

namespace MVC_TEST.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult listnew()
        {
            NewsModels nm = new NewsModels();
            News nw = new News();

            var query = "SELECT * FROM tb_nilecon";
            SqlConnection con = new SqlConnection(Startup.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nw = new News();
                if(reader["id"] != DBNull.Value)
                {
                    nw.id = int.Parse(reader["id"].ToString());
                }
                if(reader["title"] != DBNull.Value)
                {
                    nw.titel = reader["title"].ToString();
                }
                if(reader["sup_content"] != DBNull.Value)
                {
                    nw.sup_content = reader["sup_content"].ToString();
                }
                if(reader["main_content"] != DBNull.Value)
                {
                    nw.main_content = reader["main_content"].ToString();
                }
                if(reader["image"] != DBNull.Value)
                {
                    nw.image = reader["image"].ToString();
                }
                nm.LNews.Add(nw);
            }
            return PartialView("Listnews", nm);
        }
    }
}
