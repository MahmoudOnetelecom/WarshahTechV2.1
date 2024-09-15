﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.AddressDTOs
{
   public class EditCountryDTO
    {
        [Required]
        public int Id { get; set; }


        [Required]
        [MinLength(1), MaxLength(100)]
        public string CountryNameAr { get; set; }


        [Required]
        [MinLength(1), MaxLength(100)]
        public string CountryNameEn { get; set; }
    }
}
