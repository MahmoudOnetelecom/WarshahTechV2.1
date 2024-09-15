﻿using BL.Infrastructure;
using DL.DBContext;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Repositories
{
    public interface IBalanceBankRepository
    { }
    public class BalanceBankRepository : Repository<BalanceBank>, IBalanceBankRepository
    {
        public BalanceBankRepository(AppDBContext ctx) : base(ctx)
        { }


    }
}
