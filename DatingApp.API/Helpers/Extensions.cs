using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    /// <summary>
    /// Extension methods to support Global Error Handling
    /// Adds Headers to outgoing requests when an error occurs
    /// From app.UseExceptionHandler() in the Startup.cs Request Pipeline
    /// </summary>
    public static class Extensions
    {
        #region Extension Methods

        public static void AddApplicationError(this HttpResponse response, string message)
        {
            //Adds additional Headers to a response (k,v)
            //
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            // if(theDateTime.AddYears(age) > DateTime.Today) age--;return age;
            if (DateTime.Now.DayOfYear < theDateTime.DayOfYear)
            {
                age--;            //cannot combine calc + return, must be on sep lines
                return age;         //Else will not incrp the --
            }
            else return age;
        }
    }

    #endregion
}