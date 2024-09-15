﻿using System;
using System.Collections.Generic;

#nullable disable

namespace WarshahTechV2.Models
{
    public partial class Users1
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Cr { get; set; }
        public DateTime? CrexpiryDate { get; set; }
        public string LicenseNo { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public Guid? WarshahId { get; set; }
        public int RoleId { get; set; }
        public int? AccountStatusId { get; set; }
        public bool PerAddFixCommand { get; set; }
        public bool PerAddMotor { get; set; }
        public bool PerAddMotorOwner { get; set; }
        public bool PerIssueInvoice { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}