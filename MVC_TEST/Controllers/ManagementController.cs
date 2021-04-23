using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_TEST.Models;
using static MVC_TEST.Models.NewsModels;

namespace MVC_TEST.Controllers
{
    public class ManagementController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public ManagementController(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult showdata()
        {
            NewsModels nm = new NewsModels();
            News nw = new News();
            var query = "SELECT * FROM tb_nilecon";
            SqlConnection con = new SqlConnection(Startup.ConnectionString);
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read()){
                nw = new News();
                if(reader["ID"] != DBNull.Value)
                {
                    nw.id = int.Parse(reader["ID"].ToString());
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
            return PartialView("AllNews",nm);
        }

        public async Task<IActionResult> Adddata(List<IFormFile> files,string title,string supcontent,string maincontent)
        {
            try
            {
                var basePath = Path.Combine(_hostingEnvironment.WebRootPath,"uploads");
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                foreach (IFormFile postedFile in files)
                {
                    //string partimagee = "/UploadFiles/";
                    string extFile = Path.GetExtension(postedFile.FileName);
                    string fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + extFile;
                    var filepath = Path.Combine(basePath, fileName);
                    //var SSS = partimagee + fileName;
                    using (FileStream stream = new FileStream(filepath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    var query = "INSERT INTO tb_nilecon (title,sup_content,main_content,image)VALUES(@title,@supcontent,@maincontent,@image)";
                    SqlParameter pt = new SqlParameter();
                    List<SqlParameter> lpt = new List<SqlParameter>();

                    pt = new SqlParameter();
                    pt.ParameterName = "@title";
                    pt.Value = title;
                    lpt.Add(pt);

                    pt = new SqlParameter();
                    pt.ParameterName = "@supcontent";
                    pt.Value = supcontent;
                    lpt.Add(pt);

                    pt = new SqlParameter();
                    pt.ParameterName = "@maincontent";
                    pt.Value = maincontent;
                    lpt.Add(pt);

                    pt = new SqlParameter();
                    pt.ParameterName = "@image";
                    pt.Value = fileName;
                    lpt.Add(pt);

                    SqlConnection con = new SqlConnection(Startup.ConnectionString);
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    foreach (var item in lpt)
                    {
                        cmd.Parameters.Add(item);
                    }
                    
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
                return Ok("SUCCESS");
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        public async Task<IActionResult> Editdata(IEnumerable<IFormFile> files, int id, string title, string supcontent, string maincontent, string imageName)
        {
            try
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var size = files.Sum(f => f.Length);
                if (size != 0)
                {
                    var ll = path + "\\" + imageName;
                    if (System.IO.File.Exists(ll))
                    {
                        System.IO.File.Delete(ll);
                    }

                    foreach (IFormFile postedFile in files)
                    {
                        string extFile = Path.GetExtension(postedFile.FileName);
                        string fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + extFile;
                        string filepath = Path.Combine(path, fileName);
                        using (FileStream stream = new FileStream(filepath, FileMode.Create))
                        {
                            postedFile.CopyTo(stream);
                        }
                        var query = "UPDATE tb_nilecon SET title = @title,sup_content = @supcontent,main_content = @maincontent,image = @filename WHERE id = @id";
                        SqlParameter pt = new SqlParameter();
                        List<SqlParameter> lpt = new List<SqlParameter>();

                        pt = new SqlParameter();
                        pt.ParameterName = "@id";
                        pt.Value = id;
                        lpt.Add(pt);

                        pt = new SqlParameter();
                        pt.ParameterName = "@title";
                        pt.Value = title;
                        lpt.Add(pt);

                        pt = new SqlParameter();
                        pt.ParameterName = "@supcontent";
                        pt.Value = supcontent;
                        lpt.Add(pt);

                        pt = new SqlParameter();
                        pt.ParameterName = "@maincontent";
                        pt.Value = maincontent;
                        lpt.Add(pt);

                        pt = new SqlParameter();
                        pt.ParameterName = "@filename";
                        pt.Value = fileName;
                        lpt.Add(pt);

                        SqlConnection con = new SqlConnection(Startup.ConnectionString);
                        SqlCommand cmd = new SqlCommand(query, con);
                        con.Open();
                        foreach (var item in lpt)
                        {
                            cmd.Parameters.Add(item);
                        }
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else
                {
                    var query = "UPDATE tb_nilecon SET title = @title,sup_content = @supcontent,main_content = @maincontent,image = @filename WHERE id = @id";
                    SqlParameter pt = new SqlParameter();
                    List<SqlParameter> lpt = new List<SqlParameter>();

                    pt = new SqlParameter();
                    pt.ParameterName = "@id";
                    pt.Value = id;
                    lpt.Add(pt);

                    pt = new SqlParameter();
                    pt.ParameterName = "@title";
                    pt.Value = title;
                    lpt.Add(pt);

                    pt = new SqlParameter();
                    pt.ParameterName = "@supcontent";
                    pt.Value = supcontent;
                    lpt.Add(pt);

                    pt = new SqlParameter();
                    pt.ParameterName = "@maincontent";
                    pt.Value = maincontent;
                    lpt.Add(pt);

                    pt = new SqlParameter();
                    pt.ParameterName = "@filename";
                    pt.Value = imageName;
                    lpt.Add(pt);

                    SqlConnection con = new SqlConnection(Startup.ConnectionString);
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    foreach (var item in lpt)
                    {
                        cmd.Parameters.Add(item);
                    }
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                

                return Ok("true");
            }catch(Exception ex)
            {
                return Ok("false");
            }
        }

        public IActionResult Deletedata(int id,string image)
        {
            try
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (image != null)
                {
                    var ll = path + "\\" + image;
                    if (System.IO.File.Exists(ll))
                    {
                        System.IO.File.Delete(ll);
                    }
                }
                SqlParameter pt = new SqlParameter();
                List<SqlParameter> lpt = new List<SqlParameter>();
                var query = "DELETE FROM tb_nilecon WHERE id = @id";
                SqlConnection con = new SqlConnection(Startup.ConnectionString);
                SqlCommand cmd = new SqlCommand(query, con);

                pt = new SqlParameter();
                pt.ParameterName = "@id";
                pt.Value = id;
                lpt.Add(pt);

                con.Open();
                foreach (var item in lpt)
                {
                    cmd.Parameters.Add(item);
                }
                cmd.ExecuteNonQuery();
                con.Close();

                return Ok("success");
            }catch(Exception ex)
            {
                return Ok("false");
            }
        }
    }
}
