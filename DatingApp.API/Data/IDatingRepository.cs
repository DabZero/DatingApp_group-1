using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    /// <summary>
    /// Interface to seperate the concerns of DB Operations (CRUD)
    /// Methods to be defined by Implementation -"Add", "Delete", "SaveAll", "Get"  
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
        ///  Gets a specific Photo.cs based on the Photo Id
        /// </summary>
        /// <param name="id">The Id of a specific Photo.cs</param>
        /// <returns>A specific Photo.cs</returns>
        Task<Photo> GetPhoto(int id);


        /// <summary>
        /// Calls DB to retrieve the isMain photo,cs of a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Photo object where isMain Property = true</returns>
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}