using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoGym.Providers.ViewModels
{
    [DataContract]
    public class InvoiceHeaderViewModel
    {
        [DataMember]
        public string InvoiceNo { get; set; }
        
        [DataMember]
        public DateTime InvoiceDate { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string ContractNo { get; set; }

        [DataMember]
        public string InvoiceType { get; set; }
    }
}
