using Dropzone.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Dropzone.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult FileList()
        {
            string[] ext = new string[]
            {
                ".jpg", ".png", ".gif", ".jpeg", ".bmp", // graficzne
                ".mp3", ".wma", "wav", // muzyczne
                ".mp4", ".avi", ".wmv", // wideo
                ".zip", ".rar", // skompresowane
                ".txt", ".docx", ".pdf", // tekstowe
                ".accdb", ".xlsx" // officowe (baza danych, arkusz)
            };

            return View(ext);
        }
        [HttpPost]
        public ActionResult ShowUploadedFilesByExtensions(string[] extensions)
        {

            List<UploadedFileModel> files = new List<UploadedFileModel>();

            string path = Server.MapPath("~/MyFiles");
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (FileInfo file in dir.GetFiles())
            {
                string ext = Path.GetExtension(file.FullName);

                if (extensions == null || extensions.Contains(ext))
                { 
                    files.Add(new UploadedFileModel { Path = file.FullName, FileName = file.Name, Extension = ext });
                }

            }

            return View(files);
        }

        public ActionResult Index()
        {
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

        public ActionResult Upload()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = Path.Combine(Server.MapPath("~/MyFiles"));
                        string pathString = System.IO.Path.Combine(path.ToString());
                        var fileName1 = Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var uploadpath = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(uploadpath);
                    }
                }

            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new
                {
                    Message = fName
                });
            }
            else
            {
                return Json(new
                {
                    Message = "Error in saving file"
                });
            }
        }

        [HttpPost]
        public void sendEmail(string receiverEmail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderemail = new MailAddress("turniej.sportowy2017@gmail.com", "Turniej");
                    var receiveremail = new MailAddress(receiverEmail, "Receiver");

                    var password = "abcd12345678";
                    var sub = "Potwierdzenie";
                    var body = "Upload plików został wykonany pomyślnie.";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(senderemail.Address, password)
                    };

                    using (var mess = new MailMessage(senderemail, receiveremail)
                    {
                        Subject = sub,
                        Body = body
                    }
                    )
                    {
                        smtp.Send(mess);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Nie można wysłać.";
            }
        }
    }
}