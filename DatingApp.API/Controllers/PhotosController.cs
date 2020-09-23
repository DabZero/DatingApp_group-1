using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{

    [Authorize]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, IMapper mapper,
        // services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            IOptions<CloudinarySettings> cloudinaryConfig)// type = <class from Helpers>
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            // Bring in the Account class from Cloudinary library
            //
            Account acc = new Account(

                // 3 strings from api>appsetting.json
                //
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            // new instance of Cloudinary with acc details above
            //
            _cloudinary = new Cloudinary(acc);
        }


        // GET  api/users/5/photos/{id}
        // GetPhoto is the name of this route
        // ---
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            // Get Photo from Repo using async-await
            //
            var photoFromRepo = await _repo.GetPhoto(id);

            // Return the photo as a Dto because of Navigational property in Photo object
            // has a full User object with all details we do not want to return
            //
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        // POST  api/users/5/photos
        //
        // ---
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser
        (int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
        {

            // Check if the current User is the one that passed the token to the server
            // Trying to match passed id to what is in their token ... see authController line 79
            // User = check the passed token and get info from it .. we are [Authorize] this request
            //
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // Call the repo method to return a single user from the repo <-> DB based on Id
            //
            User userFromRepo = await _repo.GetUser(userId);

            // call Dto instance that is being passed and get its File property (has photo)
            //
            IFormFile file = photoForCreationDto.File;

            //To hold the results we get back from Cloudinary.  ImageUploadResult= Cloudinary class
            //
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                // read this file into memory then dispose whne done
                //
                using (var stream = file.OpenReadStream())
                {
                    // Populate the "uploadResult" from Cloudinary w/ the Photo from Client
                    // IFormFile as file which we will read as a stream, get name tied to this object
                    // Transform the Photo to meet our shape/size specs for the site
                    // Use all of these as params which we use to initialize uploadresult
                    // This is what we are going to pass to cloud storage
                    //
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    // Calls method to upload to 3rd party storage + store in local variable
                    //
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            //These are to populate Dto w/ results returned back from Cloudinay
            //
            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            // map returned results from our Dto -> Photo object 
            //
            Photo photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }

            // Track changes to the User object which now has updated Photo
            // details that we are adding from Dto (has upload response + isMain)
            //
            userFromRepo.Photos.Add(photo);



            if (await _repo.SaveAll())
            {

                // Convert updated Photo object to Dto because, we want the return
                // status code 201 also, include header info about this photo 
                // Once saved the photo will have a DB generated Id 
                //
                var phototoReturn = _mapper.Map<PhotoForReturnDto>(photo);

                //Show location header of created resource
                // string routeName = Name [httpGet{"{id}"}, Nme = "GetPhoto"] 
                // object routeValues, new object with values from passed userId + 
                //                     photo(mapped Dto from above) 
                // object value = The object being created
                return CreatedAtRoute("GetPhoto",
                    new { userId = userId, id = photo.Id }, phototoReturn);
            }

            else return BadRequest("Could not add the Photo");

        }
        //POST  api/users/5/photos/{photoId}/setMain
        //
        // ---
        [HttpPost("{photoId}/setMain")]
        public async Task<IActionResult> setMainPhoto(int userId, int photoId)
        {
            // Check if the current User is the one that passed the token to the server
            // Trying to match passed id to what is in their token ... see authController line 79
            // User = check the passed token and get info from it .. we are [Authorize] this request
            //
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // Call the repo method to return a single user from the repo <-> DB based on Id
            //
            User user = await _repo.GetUser(userId);

            //Does this photo exist in this User's photo collection (yes/No)
            //
            if (!user.Photos.Any(p => p.Id == photoId))
                return Unauthorized("No matched photos");

            // Since exists, Get the photo from the repo
            //
            var photoFromRepo = await _repo.GetPhoto(photoId);

            //Is this passed photo the main photo for this user already?
            //
            if (photoFromRepo.IsMain) return BadRequest("This is already the main photo");

            // Get the existing main photo for a partucular user
            //
            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);

            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            // If we are able to save these updates 
            //
            if (await _repo.SaveAll()) return NoContent();

            else return BadRequest("Could not set this photo to main");


        }

        // DELETE  api/users/{userId}/photos/{photoId}
        //
        // ---
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int userId, int photoId)
        {
            // Check if the current User is the one that passed the token to the server
            // Trying to match passed id to what is in their token ... see authController line 79
            // User = check the passed token and get info from token .. we are [Authorize] this request
            //
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // Call the repo method to return a single user from the repo <-> DB based on Id
            //
            User user = await _repo.GetUser(userId);

            //Does this photo exist in this User's photo collection (yes/No)
            //
            if (!user.Photos.Any(p => p.Id == photoId))
                return Unauthorized("No matched photos");

            // Since exists, Get the photo from the repo
            //
            var photoFromRepo = await _repo.GetPhoto(photoId);

            //Is this passed photo the main photo for this user already?
            //
            if (photoFromRepo.IsMain) return BadRequest("Cannot delete your main photo");

            // Only Cloudinary photos have a publicId
            //
            if (photoFromRepo.PublicId != null)
            {
                // How to delete a photo in the Cloudinary 3rd party storage
                // https://cloudinary.com/documentation/image_upload_api_reference#destroy_method
                // Deleting a Cloudinary photo requires     1. Make separate "DeleteParams"
                // 2. Pass delete params into its destroy method
                //
                var deletionParams = new DeletionParams(photoFromRepo.PublicId);

                // Destroy is the method being invoked to delete on Cloudinary.com location
                //
                var deletionResult = _cloudinary.Destroy(deletionParams);

                // results from successful deletion = string with value "ok"
                //
                if (deletionResult.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }
            }


            if (photoFromRepo.PublicId == null)
            {
                _repo.Delete(photoFromRepo);
            }

            if (await _repo.SaveAll())
            {
                return Ok();
            }
            else return BadRequest("Failed to delete the Photo");

        }
    }
}