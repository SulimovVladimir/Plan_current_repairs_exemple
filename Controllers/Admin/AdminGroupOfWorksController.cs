using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using System.Linq;


namespace Plan_current_repairs.Controllers
{
    [Authorize("IsAdmin")]
    public class AdminGroupOfWorksController: Controller
    {
        public readonly IWorks GroupOfWorks;
        public AdminGroupOfWorksController(IWorks GroupOfWorks)
        {
            this.GroupOfWorks = GroupOfWorks;
        }

        public IActionResult IndexGroup()
        {
            ViewBag.ModelGroup = GroupOfWorks.AllGroupNameOfWorks();
            return View();
        }

        public IActionResult CardGroup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CardGroup(GroupNameOfWorks groupName)
        {
            GroupOfWorks.AddAndEditGroupNameofWorks(groupName);
            return RedirectToAction("IndexGroup");
        }

        public IActionResult DeleteGroupOfWorks(int GroupID)
        {
            if (GroupID != 0)
            {
                TempData["Info"] = $"{GroupOfWorks.GetOnlyGroupByID(GroupID).NameGroup} удалено";
                GroupOfWorks.DeleteGroupNameOfWorks(GroupID);

            }
            else TempData["Info"] = "Не выбрано группа для удаления";
            return RedirectToAction("IndexGroup", "AdminGroupOfWorks");
        }
        public IActionResult EditGroupOfWorks(int GroupID)
        {
            if (GroupID == 0)
            {
                TempData["Info"] = "Не выбрано группа для редактирования";
                return RedirectToAction("IndexGroup", "AdminGroupOfWorks");
            }
            ViewBag.AllGroup = new SelectList(GroupOfWorks.AllGroupNameOfWorks(), "GroupID", "NameGroup");
            
            return View("CardGroup", GroupOfWorks.GetOnlyGroupByID(GroupID));
        }
    }
}
