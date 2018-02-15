using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MongoDBprojekat.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            var fileName = "";
            var fileSavePath = "";
            var uploadedFile = Request.Files[0];
            fileName = Path.GetFileName(uploadedFile.FileName);
            if (Request.Cookies.Count > 0)
            {
                /* VEROVATNO SERVER SPRECAVA DA SE KREIRA FOLDER  */
                //Directory.CreateDirectory("~/UploadedFiles/" + Request.Cookies[0]["username"]);
                fileSavePath = Server.MapPath("~/UploadedFiles/" + Request.Cookies[0]["username"] + "/" + fileName);
                uploadedFile.SaveAs(fileSavePath);
            }

            using (var DBContext = new MongoDBContext())
            {
                DBContext.ConnectionString = "mongodb://localhost:27017";
                DBContext.DatabaseName = "test";
                DBContext.IsSSLEnabled = true;

                DBContext.Connect();

                Models.UploadedFile fileToUpload = new Models.UploadedFile()
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = Request.Files[0].FileName,
                    Url = Request.Files[0].FileName
                };

                DBContext.UpdateUserUploads(fileToUpload, Request.Cookies[0]["username"]);

                DBContext.Dispose(); //release resources used here
            }

            return Content("Successfully saved the file.");
        }


        [AsyncTimeout(150)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimeoutError")]
        public void Remove(string _id, string userId)
        {
            using (var dbContext = new MongoDBContext())
            {
                dbContext.ConnectionString = "mongodb://localhost:27017";
                dbContext.IsSSLEnabled = true;
                dbContext.DatabaseName = "test";

                dbContext.Connect();

                dbContext.RemoveFromFiles(_id, userId);

                dbContext.Dispose(); //release the resources used here
            }

            Redirect("/Account/Profile?_id=" + userId);
        }

        public ActionResult SearchUploaded(string searchID)
        {
            var searchedResult = new Models.UploadedFile();

            using (var dbContext = new MongoDBContext("mongodb://localhost:27017"))
            {
                dbContext.DatabaseName = "test";
                dbContext.IsSSLEnabled = true;
                dbContext.Connect();

                searchedResult = dbContext.SearchUploadedFile(searchID);

                dbContext.Dispose(); //release the resources used here
            }

            return View(searchedResult);
        }
    }
}