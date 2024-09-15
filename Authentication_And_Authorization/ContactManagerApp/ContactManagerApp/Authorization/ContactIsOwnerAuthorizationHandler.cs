using ContactManagerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace ContactManagerApp.Authorization
{
    public class ContactIsOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
    {
        UserManager<IdentityUser> _userManager;
        public ContactIsOwnerAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
        {
            if (context.User == null || resource==null) 
            {
                return Task.CompletedTask;
            }

            // If not asking for CRUD permission, return.
            if (requirement.Name != Constants.Constants.CreateOperationName &&
                requirement.Name !=Constants.Constants.ReadOperationName &&
                requirement.Name !=Constants.Constants.UpdateOperationName &&
                requirement.Name != Constants.Constants.DeleteOperationName) 
            {
                return Task.CompletedTask;
            }
            if (resource.OwnerID == _userManager.GetUserId(context.User)) 
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;

        }
    }
}
