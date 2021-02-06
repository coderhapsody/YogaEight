﻿using System;

namespace GoGym.Providers.ViewModels
{


    [System.Runtime.Serialization.DataContract]
    public class InstructorCheckInViewModel
    {
        [System.Runtime.Serialization.DataMember]
        public int BranchID { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Barcode { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Name { get; set; }

        [System.Runtime.Serialization.DataMember]
        public DateTime CheckInWhen { get; set; }
    }

}