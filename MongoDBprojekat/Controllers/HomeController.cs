using MongoDB.Bson;
using System.Web.Mvc;

namespace MongoDBprojekat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var dBContext = new MongoDBContext("mongodb://localhost:27017"))
            {
                dBContext.IsSSLEnabled = true;
                dBContext.DatabaseName = "test";
                dBContext.Connect();

                dBContext.Dispose(); //release the resources used here
            }
            
            //dBContext.InsertIntoUsers(new Models.User() { FirstName = "Nikola", LastName = "Jovanovic", Username = "zeroday", Password = "zeroday", Id = ObjectId.Parse("000000000000000000000010")});
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}