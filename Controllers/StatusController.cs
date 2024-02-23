using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using System.Linq;

namespace Plan_current_repairs.Controllers
{
    public class StatusController : Controller
    {
        public readonly IStatus status;
        public readonly Settings settings;
        public readonly ILogger<StatusController> logger;
        public StatusController(IStatus _status, IOptionsMonitor<Settings> _settings, ILogger<StatusController> _logger)
        {
            status = _status;
            settings = _settings.CurrentValue;
            logger = _logger;
        }

        //согласование
        public IActionResult Confirm(int ID, string block)
        {
            status.ChangeBlocking(ID, HttpContext.User.Identity.Name, block );
            logger.LogInformation("Пользователь {user} согласовал {block}-{department}-{currentYear}", HttpContext.User.Identity.Name, block, status.OnlyStatusRecordByID(ID).Department.NameDepartment, status.OnlyStatusRecordByID(ID).Year.Years.ToString());
            return RedirectToAction("Index", "Home");
        }
    }
}
