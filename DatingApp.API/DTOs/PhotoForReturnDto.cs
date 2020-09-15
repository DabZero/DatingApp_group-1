using System;

namespace DatingApp.API.DTOs
{
    public class PhotoForReturnDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        // what we get back from Cloudinary to ID this photo
        public string PublicId { get; set; }
    }
}