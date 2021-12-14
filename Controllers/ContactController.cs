using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebProjectAircraftForum.Data;
using WebProjectAircraftForum.Models.Contact;

namespace WebProjectAircraftForum.Controllers
{
    public class ContactController : Controller
    {
        private readonly IAppUser _userService;

        public ContactController(IAppUser userService)
        {
            _userService = userService;
        }

        //CONTACT GET
        public IActionResult Index(string id)
        {
            if (id != null)
            {
                var user = _userService.GetById(id);

                if (user == null)
                {
                    return View("OopsPage");
                }

                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.UserName = user.UserName;
                ViewBag.Email = user.Email;
            }

            return View();
        }

        //CONTACT POST
        [HttpPost]
        public IActionResult SendMessage(ContactIndexModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailAddress sender = new MailAddress(model.UserEmail, model.UserName);
                    MailAddress receiver = new MailAddress("ognjensredic@gmail.com");
                    MailMessage message = new MailMessage();

                    message.From = sender;
                    message.To.Add(receiver);
                    message.Subject = "Message from: " + model.FirstName + " " + model.LastName;
                    message.Body = "Subject: " + model.Subject + ", Message: " + model.Message;
                    message.IsBodyHtml = true;

                    SmtpClient client = new SmtpClient("smtp.gmail.com");
                    client.EnableSsl = true;
                    client.Port = 587;

                    client.Credentials = new NetworkCredential("{your_gmail}", "{your_password}"); //enter your gmail and password here
                    client.Send(message);

                    ModelState.Clear();
                    ViewBag.ContactMessageSuccess = "The message has been sent!";
                    return View("Index");
                }
                catch (Exception)
                {
                    ViewBag.ContactMessageFail = "Something went wrong! Please try again.";
                    return View("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
