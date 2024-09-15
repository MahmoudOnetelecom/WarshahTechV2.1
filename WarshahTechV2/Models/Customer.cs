﻿using System;
using System.Collections.Generic;

#nullable disable

namespace WarshahTechV2.Models
{
    public partial class Customer
    {
        public Guid CustomerId { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string LicenseNo { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Passport { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Nationality { get; set; }
        public int? AccountStatusId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? ActivatedByMail { get; set; }
        public string Cn { get; set; }
    }
}
