﻿using AutoMapper;
using BL.Infrastructure;
using DL.Entities;
using EntityFrameworkCore.Triggered;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Helper.Triggers
{
    public class TCountry : IBeforeSaveTrigger<DL.Entities.Country>
    {

        public Task BeforeSave(ITriggerContext<DL.Entities.Country> context, CancellationToken cancellationToken)
        {



            if (context.ChangeType == ChangeType.Modified)
            {
                context.Entity.UpdatedOn = DateTime.Now;
            }

            return Task.CompletedTask;
        }
    }

}

