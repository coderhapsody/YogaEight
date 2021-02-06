using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using GoGym.Providers;
using Ninject;

namespace GoGym.FrontEnd.Services
{
    [ServiceContract(Namespace = "GoGym")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AjaxCallService
    {
        [Inject]
        public ItemProvider ItemService { get; set; }

        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public decimal GetUnitPrice(int itemID)
        {
            var item = ItemService.Get(itemID);
            return item == null ? 0 : item.UnitPrice;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
