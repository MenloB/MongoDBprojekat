using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MongoDBprojekat.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotAuthorizedAccess()
        {
            string gif = "";
            using (var dbContext = new MongoDBContext())
            {
                dbContext.ConnectionString = "mongodb://localhost:27017";
                dbContext.DatabaseName = "test";
                dbContext.IsSSLEnabled = true;

                dbContext.Connect();

                Random randomNumber = new Random();
                int id = randomNumber.Next(1, 9); // generise novi int izmedju 0 i 3

                gif = dbContext.GetGIFUrl(id);

                dbContext.Dispose();
            }

            ViewBag.Message = gif;

            return View();
        }
    }
}