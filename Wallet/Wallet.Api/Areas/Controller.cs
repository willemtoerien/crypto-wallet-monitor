﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.DataAccess;
using Wallet.Api.Extensions;

namespace Wallet.Api.Areas
{
    [ApiController]
    [Authorize]
    public abstract class Controller : ControllerBase
    {
        public WalletDbContext Db { get; }

        public bool IsAuthenticated => User.Identity.IsAuthenticated;

        public int UserId => User.GetUserId();

        public IQueryable<DataAccess.User> UserQuery => Db.Users.Where(x => x.UserId == UserId);

        protected Controller(WalletDbContext db)
        {
            Db = db;
        }
    }
}