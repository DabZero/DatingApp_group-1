using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    #region Interface Methods

    /// <summary>
    /// Interface to seperate the concerns of DB Operations (CRUD)
    /// Methods to be defined by Implementation -"Register", "Login", "UserExists"  
    /// </summary>
    /// 
    public interface IAuthRepository
    {

        /// <summary>
        ///  Registers a new user by
        ///  Saving User into the DB after it ecrypts the password 
        /// </summary>
        /// <param name="user">User object to hold the User credentials</param>
        /// <param name="password">User provides their own password</param>
        /// <returns>
        ///  User object with fully encryted password
        /// </returns>
        /// ----
        Task<User> Register(User user, string password);



        /// <summary>
        /// Checks existing Username + Password against values in the DB
        /// </summary>
        /// <param name="userName">Input from the Login screen</param> 
        /// <param name="passWord">Ecrytped password using  
        ///                        System.Security.Cryptography.HMACSHA512</param>
        /// <returns>Validated User object or Null</returns>
        /// ----
        Task<User> Login(string userName, string passWord);



        /// <summary>
        ///  Checks against existing Usernames in the DB
        /// </summary>
        /// <param name="userName">User provides UserName</param> 
        /// <returns>
        ///  True = userName already exists ...Duplicate entry...proceed as BadRequest  
        ///  False= userName does not exist in the DB...OK to proceed
        /// </returns>
        /// ----
        Task<bool> UserExists(string userName);
    }
    #endregion
}