using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using GoGym.Providers;
using GoGym.Providers.ViewModels;
using Ninject;

namespace GoGym.FrontEnd.Services
{
    [ServiceContract(Namespace = "AjaxService")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CheckInService
    {
        [Inject]
        public CustomerProvider CustomerService { get; set; }

        [Inject]
        public InvoiceProvider InvoiceService { get; set; }

        [Inject]
        public ItemTypeProvider ItemTypeService { get; set; }


        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

        [OperationContract]
        public bool ItemTypeIsService(int itemTypeID)
        {
            var itemType = ItemTypeService.Get(itemTypeID);
            if (itemType != null)
                return itemType.Type == 'S';
            return false;
        }

        [OperationContract]
        public InvoiceHeaderViewModel GetInvoiceInfo(string invoiceNo)
        {
            var result = new InvoiceHeaderViewModel();
            var invoice = InvoiceService.GetInvoice(invoiceNo);
            if (invoice != null)
            {
                var cust = invoice.Customer;
                var contract = invoice.Contract;
                result.InvoiceNo = invoiceNo;
                result.InvoiceDate = invoice.Date;
                result.InvoiceType = invoice.InvoiceType == 'F' ? "Fresh Member Invoice" : "Existing Member Invoice";
                result.CustomerName = String.Format("{0} {1}", cust.FirstName, cust.LastName);
                result.ContractNo = contract == null ? "Not Available" : contract.ContractNo;
            }
            else
            {
                throw new Exception("Invoice cannot be found");
            }

            return result;
        }

        [OperationContract]
        public CustomerCheckInViewModel DoCheckIn(int branchID, string customerBarcode, string userName)
        {
            string photoPath = HttpContext.Current.Server.MapPath("~/Photo/Customers/");
            CustomerCheckInViewModel cust = CustomerService.DoCheckIn(branchID, customerBarcode, userName, photoPath);
            return cust;
        }

        [OperationContract]
        public CustomerCheckInViewModel[] GetCheckInHistory(int branchID)
        {
            List<CustomerCheckInViewModel> list = CustomerService.GetCheckInHistory(branchID).ToList();
            return list.ToArray();
        }

        [OperationContract]
        public CustomerCheckInViewModel DoCheckInByContract(int branchID, string contractNo, string userName)
        {
            return CustomerService.DoCheckInByContract(branchID, contractNo, userName);
        }
    }
}
