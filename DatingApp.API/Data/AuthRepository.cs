using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{

    /// <summary>
    /// Repository to deal w/ DB operations to Login or Register Users
    /// Impl of IAuthRepository that brings in the DBContext (middleware App<->DB)
    /// To perform CRUD functions on the Users DbSet
    /// </summary>
    /// 
    public class AuthRepository : IAuthRepository
    {
        #region Constructor

        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;
        }
        #endregion

        #region Methods for Authentication of User Login & Register
        /// <summary>
        /// Verifies the users login credentials 
        /// </summary>
        /// <param name="userName">Verified against username in DB to retrieve the User object</param>
        /// <param name="passWord">Validates password.async  Salt-Key re-creates the security object 
        /// and the password is recreated as a byte.  Passed value is compared against DB value byte-by-byte</param>
        /// <returns></returns>
        /// ----
        public async Task<User> Login(string userName, string passWord)
        {
            var user = await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return null; //This method is used by the Controller so when we return a null
                             //the IActionResult response can be 401 .UnathorizedRequest()

            if (!VerifyPasswordHash(passWord, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }


        /// <summary>
        /// Convenience method to verify If a given password matches the encryted password in the DB 
        /// </summary>
        /// <param name="passWord">User supplied Login password</param>     
        /// <param name="passwordHash">Encripted password from the DB</param> 
        /// <param name="passwordSalt"> Previous Key given from Security.Cryptography object that
        ///                             generates same hash, given same password...from same key</param>
        /// <returns>True = Encryted passwords match</returns>
        /// ----
        private bool VerifyPasswordHash(string passWord, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passWord));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }



        /// <summary>
        ///  Saves user into the DB after it ecrypts the password 
        /// </summary>
        /// <param name="user">User Object</param>
        /// <param name="password">User supplied password</param>
        /// <returns>User object with fully encryted password</returns>
        /// ----
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash; byte[] passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            //convert passed string to array of bytes
            user.PasswordHash = passwordHash;

            //Unique key for this specific crypto object
            //Needed later to de-code the password when user is trying to log in
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }



        /// <summary>
        /// Convenience method to Encrypt the given password during registration
        /// Uses Cryptography object to genertae hashing + specific object key
        /// </summary>
        /// <param name="password">Passed by User</param>     
        /// <param name="passwordHash">Generated from Security object's method ComputeHash</param> 
        /// <param name="passwordSalt">Generated from security object's key</param> 
        /// ----
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //hashing object 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                passwordSalt = hmac.Key;
            }
        }


        /// <summary>
        /// Checks against existing Usernames 
        /// </summary>
        /// <param name="userName">Input from the Login/Register screen</param>
        /// <returns>
        ///  True = userName already exists ...Duplicate entry...proceed as BadRequest  
        ///  False= userName does not exist in the DB...OK to proceed
        /// </returns>
        /// ----
        public async Task<bool> UserExists(string userName)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == userName))
                return true;

            else return false;
        }
        #endregion
    }
}