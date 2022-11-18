﻿using Microsoft.AspNetCore.Identity;
using OnlineTutorFinder.Web.Entities.Membership;

namespace OnlineTutorFinder.Web.Services.Membership
{
    public class RoleManager
        : RoleManager<Role>
    {
        public RoleManager(
            IRoleStore<Role> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<Role>> logger
            )
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
