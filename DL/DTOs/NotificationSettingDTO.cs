﻿using DL.DTOs.SharedDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DL.DTOs
{
    public class NotificationSettingDTO : BaseDomainDTO
    {

        
        public int WarshahId { get; set; }
       


        public int NameNotificationId { get; set; }


        public int StatusNotificationId { get; set; }

    }
}