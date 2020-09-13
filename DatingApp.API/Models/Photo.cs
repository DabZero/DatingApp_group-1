using System;

namespace DatingApp.API.Models
{
    /// <summary>
    /// Photos tied to dating app Users
    /// </summary>
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        //Property navigation resolution to solve for cascading deletes
        public User Users { get; set; }
        public int UserId { get; set; }
    }
}