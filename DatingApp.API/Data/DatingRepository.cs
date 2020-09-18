using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{

    /// <summary>
    ///  CRUD Methods to deal with Photo.cs & User.cs to/from the DB
    /// </summary>
    public class DatingRepository : IDatingRepository
    {

        #region constructors
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        #endregion

        /// <summary>
        /// Starts tracking this object prior to being saved
        /// </summary>
        /// <typeparam name="T">Can be any object type (Photo, User etc...)</typeparam>
        /// ----
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }



        /// <summary>
        /// Starts tracking this object prior to being deleted
        /// </summary>
        /// <typeparam name="T">Can be any object type (Photo, User etc...)</typeparam>
        /// ----
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }



        /// <summary>
        /// Retreives and Returns a single User from the DB
        /// </summary>
        /// <param name="id">Pass this id to match user in the DB</param>
        /// <returns>Return matched user from DB by Id</returns>
        /// ----
        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }



        /// <summary>
        /// Retreives and Returns multiple Users from the DB
        /// </summary>
        /// <returns>Return all User objects as a List from DB</returns>
        /// ----
        public async Task<IEnumerable<User>> GetUsers()
        {
            List<User> users = await _context.Users.Include(u => u.Photos).ToListAsync();
            return users;
        }



        /// <summary>
        /// This method calls EF change tracker, sees all tracked changes
        /// Saves all tracked changes to the DB
        /// </summary>
        /// <returns>Returs bool    True = Tracked changes saved to the DB  
        ///                   -or-  False= Nothing saved</returns>
        /// ---
        public async Task<bool> SaveAll()
        {

            //SaveChanges() returns an int of how many changes saved to the DB
            //
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(p => p.UserId == userId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}