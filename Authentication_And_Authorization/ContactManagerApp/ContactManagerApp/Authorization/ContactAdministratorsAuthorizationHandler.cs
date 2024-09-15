﻿using ContactManagerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ContactManagerApp.Authorization
{
    public class ContactAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement,Contact>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
        {
            if(context.User == null)
            {
                return Task.CompletedTask;
            }
            if (context.User.IsInRole(Constants.Constants.ContactAdministratorsRole))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;  
        }
    }
}
