
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    // The IAsyncActionFilter impl= Called before the action and surropunds it, 
    // after model binding is complete, perform the actions assigned to next()
    //
    public class LogUserActivity : IAsyncActionFilter
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">The Action being executed</param>
        /// <param name="next">Run this code after the Action has been executed</param>
        /// <returns></returns>
        /// 
        /// ---
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            // After a request HttpContext (request getter) get some claims info
            //
            var userId = int.Parse(resultContext.HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier).Value);


            // We need a repo to return us a User by ID so, we can change a prop value
            // resultContext = after the request HttpContext= Get the request
            // requestService = Get or set a service instance from StartUp to use a service
            // GetService<> = get a particular service and return the instance so we can use it
            //
            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();

            var user = await repo.GetUser(userId);

            user.LastActive = DateTime.Now;

            await repo.SaveAll();
        }
    }
}