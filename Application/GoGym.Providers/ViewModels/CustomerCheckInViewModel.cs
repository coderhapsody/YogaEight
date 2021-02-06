using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoGym.Providers.ViewModels
{

    [DataContract]
    public class CustomerCheckInViewModel
    {
        [DataMember]
        public IList<string> Messages { get; set; }

        [DataMember]
        public IList<string> PickUpPersons { get; set; }

        [DataMember]
        public IList<string> PickUpPhotos { get; set; }

        [DataMember]
        public IList<string> Classes { get; set; }

        [DataMember]
        public string CustomerBarcode { get; set; }

        [DataMember]
        public string Photo { get; set; }

        [DataMember]
        public bool IsPhotoExist { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerStatus { get; set; }

        [DataMember]
        public string CustomerStatusColor { get; set; }

        [DataMember]
        public string CustomerStatusBackgroundColor { get; set; }

        [DataMember]
        public string Age { get; set; }        

        [DataMember]
        public string PackageName { get; set; }

        [DataMember]
        public DateTime? When { get; set; }

        [DataMember]
        public bool AllowCheckIn { get; set; }

        [DataMember]
        public string ExpiredDate { get; set; }

        [DataMember]
        public string MemberSince { get; set; }

        [DataMember]
        public string  ContractNo { get; set; }
    }

    

}