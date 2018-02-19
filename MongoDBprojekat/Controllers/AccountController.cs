using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDBprojekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            if(Request.Cookies.Count <= 0)
            {
                Response.Redirect("/Error/NotAuthorizedAccess");
                return null;
            }
            else
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

                    if (user.Username != null)
                        return View(user);
                    else
                        return Content("Profile not found.");
                }
            }
            

            
        }

        public void DeleteAccount(string id)
        {
            using (var dbContext = new MongoDBContext())
            {
                dbContext.ConnectionString = "mongodb://localhost:27017";
                dbContext.IsSSLEnabled = true;
                dbContext.DatabaseName = "test";

                dbContext.Connect();

                dbContext.RemoveAccount(id);

                dbContext.Dispose();
            }
        }

        public void UpdateProfilePhoto(string id, HttpPostedFileBase profile)
        {
            var fileName = "";
            var fileSavePath = "";
            var uploadedFile = Request.Files[0];
            fileName = Path.GetFileName(uploadedFile.FileName);
            if (Request.Cookies.Count > 0)
            {
                /* VEROVATNO SERVER SPRECAVA DA SE KREIRA FOLDER  */
                //Directory.CreateDirectory("~/UploadedFiles/" + Request.Cookies[0]["username"]);
                fileSavePath = Server.MapPath("~/UploadedFiles/" + Request.Cookies[0]["username"] + "/ProfilePictures/" + fileName);
                uploadedFile.SaveAs(fileSavePath);
            }
            using (var dbContext = new MongoDBContext())
            {
                dbContext.ConnectionString = "mongodb://localhost:27017";
                dbContext.IsSSLEnabled = true;
                dbContext.DatabaseName = "test";

                dbContext.Connect();

                dbContext.UpdateProfilePhoto(id, profile.FileName);

                dbContext.Dispose();
            }
        }

        public void Logout()
        {
            Session.Abandon();
            Response.Cookies["upload_services"].Expires = DateTime.Now.AddDays(-1d);

            Response.Redirect("/");
        }

        public ActionResult UpdateAccount(string id)
        {
            User u = new User();

            using (var dbContext = new MongoDBContext())
            {
                dbContext.ConnectionString = "mongodb://localhost:27017";
                dbContext.DatabaseName = "test";
                dbContext.IsSSLEnabled = true;

                dbContext.Connect();

                u = dbContext.GetUserById(ObjectId.Parse(id));

                dbContext.Dispose();
            }

            return View(u);
        }

        public void Update(string id, string firstname, string lastname, string username, string email, string password, string confpassword)
        {
            if (password == confpassword) {
                if (password != null)
                {
                    using (var dbContext = new MongoDBContext())
                    {
                        dbContext.ConnectionString = "mongodb://localhost:27017";
                        dbContext.DatabaseName = "test";
                        dbContext.IsSSLEnabled = true;

                        dbContext.Connect();

                        dbContext.UpdateUserDetails(id, firstname, lastname, username, email, password);

                        dbContext.Dispose();

                        Logout();
                        Login(username, password);

                        Response.Redirect("/Account/Profile?_id=" + id);
                    }
                }
                else
                {
                    using (var dbContext = new MongoDBContext())
                    {
                        dbContext.ConnectionString = "mongodb://localhost:27017";
                        dbContext.DatabaseName = "test";
                        dbContext.IsSSLEnabled = true;

                        dbContext.Connect();

                        dbContext.UpdateUserDetails(id, firstname, lastname, username, email);

                        dbContext.Dispose();

                        Logout();

                        HttpCookie cookie = new HttpCookie("upload_services");
                        cookie["username"] = username;
                        cookie["firstname"] = firstname;
                        cookie["lastname"] = lastname;
                        cookie.Expires = DateTime.Now.AddDays(1d);
                        Response.Cookies.Add(cookie);

                        Response.Redirect("/Account/Profile?_id=" + id);
                    }
                }
            } else
            {
                return;
            }
        }
    }
}