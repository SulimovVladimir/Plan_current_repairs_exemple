using Plan_current_repairs.Data.Models;

namespace Plan_current_repairs.Data.VIewModels
{
    public class CardAct
    {
        public ActOfWork actOfWork { get; set; }
        public int RecordID { get; set; }
        public int RecordOtherID { get; set; }
        public string Block { get; set; }
    }
}
