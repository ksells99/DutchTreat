using DutchTreat.Models;
using DutchTreat.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;

        public AppController(IMailService mailService)
        {
            _mailService = mailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
         
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Send email
                _mailService.SendMessage("kai.sells@hotmail.co.uk", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");

                // Success message displayed to user
                ViewBag.UserMessage = "Email sent";

                // Clear form
                ModelState.Clear();
            };
     
            ViewBag.Title = "Contact Us";
            return View();
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }
    }
}
