using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]    //Any access to this controller 
    [Authorize]                                 //requires ActionFilter + authorization    
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IDatingRepository _repo;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }



        // Get api/users?query string i.e. pageSize=xx&pageNumber=xx
        // 
        // ----
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            var currLoggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currLoggedInUserId);

            userParams.UserId = currLoggedInUserId;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = (userFromRepo.Gender == "male") ? "female" : "male";
            }
            

            PagedList<Models.User> users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            // Add the pagination to the response headers 
            // Dto is being sent as a passed object of ActionResult w/ Response body
            // Paging data is being sent as the Response header
            //
            Response.AddPagination(users.CurrentPage,
            users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }



        //Get api/users/{id}
        //
        // ---
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        //Put api/users/{id}
        //
        // ---
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            // Check if the current User is the one that passed the token to the server
            // Trying to match passed id to what is in their token ... see authController line 79
            // User = check the passed token and get info from it
            //
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // Call the repo method to return a single user from the repo <-> DB based on Id
            //
            var userFromRepo = await _repo.GetUser(id);

            // Map(theSource_to Map_from  ,  Destination)
            // Dto only has some data but, we want to update this limited data to a full User object
            // The mapper takes Dto's 5 fields and updates them into the user object
            //
            _mapper.Map(userForUpdateDto, userFromRepo);

            //When GetUser(id) was called on userFromRepo.  EF Core registered a change to this object
            //SaveAll saves all tracked changes to the DB.  The updated User object is now saved 
            //
            if (await _repo.SaveAll())
                return NoContent();     //produces an empty response

            throw new Exception($"Updating user {id} failed to save");
        }
    }
}