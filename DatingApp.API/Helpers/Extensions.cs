using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    /// <summary>
    /// Extension methods 
    /// </summary>
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            //Adds additional Headers to a response (k,v)
            //
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}