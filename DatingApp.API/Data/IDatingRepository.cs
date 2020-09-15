using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    /// <summary>
    /// Interface to seperate the concerns of DB Operations (CRUD)
    /// Methods to be defined by Implementation -"Add", "Delete", "SaveAll", "Get User/s"  
    /// </summary>
    /// 
    public interface IDatingRepository
    {
        /// <summary>
        /// Start tracking this object prior to being saved to the DB
        /// </summary>
        /// <typeparam name="T">Can be any object type (Photo, User etc...)</typeparam>
        /// ----
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Start tracking this object prior to being deleted to the DB
        /// </summary>
        /// <typeparam name="T">Can be any object type (Photo, User etc...)</typeparam>
        /// ----
        void Delete<T>(T entity) where T : class;


        /// <summary>
        /// This method calls EF change tracker, sees all tracked changes
        /// Saves all tracked changes to the DB
        /// </summary>
        /// <returns>Returs bool    True = Tracked changes saved to the DB  
        ///                   -or-  False= Nothing saved</returns>
        Task<bool> SaveAll();


        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);

        /// <summary>
        /// Get a Photo object from DB based on Photo Id property
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a given Photo object based on what id you pass</returns>
        Task<Photo> GetPhoto(int id);
    }
}