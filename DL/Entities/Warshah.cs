﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public class Warshah:BaseDomain
    {

        [Required]

        public string WarshahNameAr { get; set; }
        public string WarshahNameEn { get; set; }
        public decimal Long { get; set; }
        public decimal Lat { get; set; }
        public int? ParentWarshahId { get; set; }
        public string LandLineNum { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int RegionId { get; set; }
        [Required]
        public int CityId { get; set; }
      

        public string Street { get; set; }
        [Required]
        public string Distrect { get; set; }
        public int UnitNum { get; set; }
        public int PostalCode { get; set; }
        public string WarshahLogo { get; set; }
        public string TaxNumber { get; set; }

        public string Terms { get; set; }
        public string CR { get; set; }
        public string WebSite { get; set; }
        public string SalesCode { get; set; }
        public string WarshahIdentifier { get; set; }

        public bool IsOld { get; set; } 

        public Guid OldWarshahId { get; set; }

        public string PrevInvoiceHash { get; set; }

    }
}