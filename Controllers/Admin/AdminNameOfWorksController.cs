using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using Plan_current_repairs.Data.VIewModels;
using System.Collections.Generic;
using System.Linq;


namespace Plan_current_repairs.Controllers
{
    [Authorize("IsAdmin")]
    public class AdminNameOfWorksController: Controller
    {
        public readonly IWorks Works;
        public AdminNameOfWorksController(IWorks NameOfWorks)
        {
            this.Works = NameOfWorks;
        }

        public IActionResult IndexNameOfWorks(SortStateNameOfWork sort = SortStateNameOfWork.NameOfWorkAsc)
        {
            
            if (Works.AllNameOfWorks != null)
            {
                ForAdminNameOfWorks recordsNameOfWorks = new ForAdminNameOfWorks
                {
                    allNameOfWorks = Works.AllNameOfWorks,
                };

                //сортировка
                ViewData["SortNameOfWork"] = sort == SortStateNameOfWork.NameOfWorkAsc ? SortStateNameOfWork.NameOfWorkDesc : SortStateNameOfWork.NameOfWorkAsc;
                ViewData["SortGroup"] = sort == SortStateNameOfWork.GroupAsc ? SortStateNameOfWork.GroupDesc : SortStateNameOfWork.GroupAsc;
                ViewData["SortActive"] = sort == SortStateNameOfWork.ActiveAsc ? SortStateNameOfWork.ActiveDesc : SortStateNameOfWork.ActiveAsc;

                recordsNameOfWorks.allNameOfWorks = sort switch
                {
                    SortStateNameOfWork.NameOfWorkAsc => recordsNameOfWorks.allNameOfWorks.OrderBy(x => x.NameOfWork),
                    SortStateNameOfWork.NameOfWorkDesc => recordsNameOfWorks.allNameOfWorks.OrderByDescending(x => x.NameOfWork),
                    SortStateNameOfWork.GroupAsc => recordsNameOfWorks.allNameOfWorks.OrderBy(x => x.GroupName.NameGroup),
                    SortStateNameOfWork.GroupDesc => recordsNameOfWorks.allNameOfWorks.OrderByDescending(x => x.GroupName.NameGroup),
                    SortStateNameOfWork.ActiveAsc => recordsNameOfWorks.allNameOfWorks.OrderBy(x => x.Active),
                    SortStateNameOfWork.ActiveDesc => recordsNameOfWorks.allNameOfWorks.OrderByDescending(x => x.Active),
                };
                return View(recordsNameOfWorks);
            }
            return View();
        }

        public IActionResult CardNameOfWorks()
        {
            ViewBag.typeRecords = new SelectList(new List<string> { "без участков", "с участками", "с участками и параметрами" });
            ViewBag.AllGroup = Works.GetListGroupNameOfWorksForSelect().Where(x=>x.Text!= "Дополнительные работы, не предусмотренные планом");           
            return View();
        }

        [HttpPost]
        public IActionResult CardNameOfWorks(NameOfWorks nameOfWorks)
        {
            if(nameOfWorks.GroupNameOfWorksID==0)
            {
                ModelState.AddModelError("", "Выберите группу");
                ViewBag.AllGroup = Works.GetListGroupNameOfWorksForSelect().Where(x => x.Text != "Дополнительные работы, не предусмотренные планом");
                return View("CreateNameOfWorks", nameOfWorks);
            }
            nameOfWorks.GroupName = Works.GetOnlyGroupByID(nameOfWorks.GroupNameOfWorksID);
            Works.AddAndEditNameofWorks(nameOfWorks);
            return RedirectToAction("IndexNameOfWorks");
        }

        public IActionResult DeleteNameOfWorks(int WorkID)
        {
            if (WorkID != 0)
            {
                TempData["Info"]=$"{Works.GetOnlyNameByID(WorkID).NameOfWork} удалено";
                Works.DeleteNameOfWorks(WorkID);

            }
            else TempData["Info"] = "Не выбрано наименование работы для удаления";
            return RedirectToAction("IndexNameOfWorks", "AdminNameOfWorks");
        }
        public IActionResult EditNameOfWorks(int WorkID)
        {
            if(WorkID==0)
            {
                TempData["Info"] = "Не выбрано наименование работы для редактирования";
                return RedirectToAction("IndexNameOfWorks", "AdminNameOfWorks");
            }
            ViewBag.AllGroup = Works.GetListGroupNameOfWorksForSelect();
            ViewBag.typeRecords = new SelectList(new List<string> { "без участков", "с участками", "с участками и параметрами" });
            return View("CardNameOfWorks", Works.GetOnlyNameByID(WorkID));
        }
    }
}
