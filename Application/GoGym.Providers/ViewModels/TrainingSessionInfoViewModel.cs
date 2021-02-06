using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoGym.Providers.ViewModels
{
    [DataContract]
    public class TrainingSessionInfoViewModel
    {
        [DataMember]
        public string CustomerBarcode { get; set; }

        [DataMember]
        public string CustomerName { get; set; }        

        [DataMember]
        public IList<TrainingSessionInvoiceViewModel> Invoices { get; set; }

        public int CustomerHomeBranchID { get; set; }
    }

    [DataContract]
    public class TrainingSessionInvoiceViewModel
    {
        public int QuotaID { get; set; }

        [DataMember]
        public int ItemID { get; set; }

        [DataMember]
        public string ItemBarcode { get; set; }

        [DataMember]
        public short Balance { get; set; }

        [DataMember]
        public string ItemDescription { get; set; }
    }
}
