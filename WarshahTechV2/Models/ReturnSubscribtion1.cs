﻿using System;
using System.Collections.Generic;

#nullable disable

namespace WarshahTechV2.Models
{
    public partial class ReturnSubscribtion1
    {
        public int Idkey { get; set; }
        public Guid InvoiceId { get; set; }
        public int InvoiceSerial { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Vat { get; set; }
        public decimal? BeforeDiscount { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Total { get; set; }
        public Guid? WarshahId { get; set; }
        public string Tag { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
