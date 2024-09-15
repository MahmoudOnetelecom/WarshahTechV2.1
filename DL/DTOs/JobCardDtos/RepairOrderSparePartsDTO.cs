﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DL.DTOs.JobCardDtos
{
   public class RepairOrderSparePartsDTO
    {
        
        public int RepairOrderId { get; set; }   
        public int SparePartId { get; set; }
        public int TechId { get; set; }
     
        public int Quantity { get; set; }
        public decimal PeacePrice { get; set; }
        public string Garuntee { get; set; }
        public decimal FixPrice { get; set; }


    }
}
