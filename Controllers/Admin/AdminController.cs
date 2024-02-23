using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Extensions;
using System;
using System.Threading.Tasks;

namespace Plan_current_repairs.Controllers
{
    [Authorize("IsAdmin")]
    public class AdminController : Controller
    {
        public readonly IWorks works;
        public readonly IEmailService emailService;
        public AdminController(IWorks works, IEmailService _emailService)
        {
            this.works = works;
            emailService = _emailService;
        }
        public IActionResult IndexAdmin()
        {
                return View();
        }

        // проверка на ошибки
        public IActionResult CheckJornal() 
        {
            works.CheckJornal();
            TempData["Info"] = "Проверка завершена";
            return RedirectToAction("IndexAdmin");
        }

        //тест работы почты
        public IActionResult CheckMail()
        {
            emailService.SendEmailAsync("email@domen.ru", "План-отчет по текущему ремонту", DateTime.Now + " -testing mail... http://stg-usv-web03/Plan_current_repairs.com/");
            TempData["Info"] = "Сообщение отправлено";
            return RedirectToAction("IndexAdmin");
        }
    }
}
