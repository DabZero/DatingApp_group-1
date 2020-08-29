using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public async static void SeedUsers(DataContext context)
        {
            //Check if DB Users table + NOT any elements exist
            //
            if (!context.Users.Any())
            {
                //Read the .json file in Data folder, hold as text
                //
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

                //Use Newtonsoft to convert .NET <-> Json
                //Read json data and convert into .NET User objects 
                //Hold Users as a List ... pass text as a param
                // 
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var u in users)
                {
                    byte[] passwordHash; byte[] passwordSalt;
                    CreatePasswordHash("1234", out passwordHash, out passwordSalt);

                    u.PasswordHash = passwordHash;
                    u.PasswordSalt = passwordSalt;
                    await context.Users.AddAsync(u);
                }
                await context.SaveChangesAsync();
            }
        }
        //---> This method is static to match static SeedUsers.  Only 1 copy needed on startup
        //---> Opting to use method here vs. re-use identical method in AuthRepository 
        //---> The method should stay private vs. making AuthRepo method public and re-sharing 
        /// ----
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //hashing object 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                //convert passed string to array of bytes
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                //Unique key for this specific crypto object
                //Needed to de-code the password when user is trying to log in
                passwordSalt = hmac.Key;
            }
        }
    }
}