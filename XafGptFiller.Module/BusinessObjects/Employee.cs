using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafGptFiller.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Employee : Person
    {
        public Employee(Session session) : base(session) { }

        private decimal _AnnualIncome;
        public decimal AnnualIncome
        {
            get { return _AnnualIncome; }
            set { SetPropertyValue<decimal>(nameof(AnnualIncome), ref _AnnualIncome, value); }
        }

        private int _FamilyCount;
        public int FamilyCount
        {
            get { return _FamilyCount; }
            set { SetPropertyValue<int>(nameof(FamilyCount), ref _FamilyCount, value); }
        }

    }
}
