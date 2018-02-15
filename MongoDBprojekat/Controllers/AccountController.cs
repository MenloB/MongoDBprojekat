using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MongoDBprojekat.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string username, string password)
        {
            using (var dbContext = new MongoDBContext("mongodb://localhost:27017/"))
            {
                dbContext.DatabaseName = "test";
                dbContext.IsSSLEnabled = true;


                dbContext.Connect();

                Models.User u = dbContext.GetUserByUsername(username, password);

                if (u != null)
                {
                    HttpCookie cookie = new HttpCookie("upload_services");
                    cookie["username"] = u.Username;
                    cookie["firstname"] = u.FirstName;
                    cookie["lastname"] = u.LastName;
                    cookie.Expires = DateTime.Now.AddDays(1d);
                    Response.Cookies.Add(cookie);

                    dbContext.Dispose();

                    return Content("Uspešno ste se prijavili, " + u.FirstName + " " + u.LastName);
                }

                dbContext.Dispose();
            }


            return Content("Pogresno korisničko ime ili lozinka");
        }

        public void Register(string firstname, string lastname, string email, string password, string username)
        {
            Models.User user = new Models.User(firstname, lastname, email, username, password);

            using (var dbContext = new MongoDBContext())
            {
                dbContext.ConnectionString = "mongodb://localhost:27017";
                dbContext.DatabaseName = "test";
                dbContext.IsSSLEnabled = true;

                dbContext.Connect();

                dbContext.InsertIntoUsers(user);

                dbContext.Dispose();
            }

            Response.Redirect("/Account?login=true");
        }

        public new ActionResult Profile(string _id)
        {
            Models.User user = new Models.User();
            using (var context = new MongoDBContext())
            {
                context.ConnectionString = "mongodb://localhost:27017";
                context.IsSSLEnabled = true;
                context.DatabaseName = "test";

                context.Connect();


                var id = ObjectId.Parse(_id);
                user = context.GetUserById(id);

                context.Dispose();
            }

            if (user.Username != null)
                return View(user);
            else
                return Content("Profile not found.");
        }

        public void Logout()
        {
            Session.Abandon();
            Response.Cookies["upload_services"].Expires = DateTime.Now.AddDays(-1d);

            Response.Redirect("/");
        }
    }
}