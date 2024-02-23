using Microsoft.AspNetCore.Mvc.Rendering;
using Plan_current_repairs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Interface
{
    public interface IWorks
    {
        IEnumerable<GroupNameOfWorks> AllGroupNameOfWorks();
        IEnumerable<NameOfWorks> AllNameOfWorks { get; }
        void AddAndEditGroupNameofWorks(GroupNameOfWorks groupName);
        void AddAndEditNameofWorks(NameOfWorks nameOfWorks);
        IEnumerable<NameOfWorks> GetNameOfWorksOnlyGroup(int groupID);
        IEnumerable<NameOfWorks> GetNameOfWorksOnlyGroup(string groupName);
        void DeleteNameOfWorks(int workID);
        void DeleteGroupNameOfWorks(int groupID);
        NameOfWorks GetOnlyNameByID(int workID);
        GroupNameOfWorks GetOnlyGroupByID(int groupID);
        public List<SelectListItem> GetListGroupNameOfWorksForSelect();
        public void CheckJornal();
    }
}
