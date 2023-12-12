using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace XafGptFiller.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class MachineRepairLog : BaseObject
    {
        public MachineRepairLog(Session session) : base(session) { }


        private string _MachineID;
        public string MachineID
        {
            get { return _MachineID; }
            set { SetPropertyValue<string>(nameof(MachineID), ref _MachineID, value); }
        }

        private DateTime _RepairOn;
        public DateTime RepairOn
        {
            get { return _RepairOn; }
            set { SetPropertyValue<DateTime>(nameof(RepairOn), ref _RepairOn, value); }
        }

        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "10")]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue<string>(nameof(Description), ref _Description, value); }
        }

        private string _Processing;
        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "10")]
        public string Processing
        {
            get { return _Processing; }
            set { SetPropertyValue<string>(nameof(Processing), ref _Processing, value); }
        }



    }
}
