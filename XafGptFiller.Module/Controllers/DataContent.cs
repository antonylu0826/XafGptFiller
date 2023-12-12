using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;

namespace XafGptFiller.Module.Controllers
{
    [DomainComponent]
    public class DataContent : NonPersistentBaseObject
    {
        [ModelDefault("RowCount", "10")]
        public string Content {  get; set; }

    }
}
