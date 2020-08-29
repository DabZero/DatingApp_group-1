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

        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
    }
}