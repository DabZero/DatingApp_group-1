using System;
using System.Linq;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    /// <summary>
    /// This AutoMapper defines what can be mapped as a source & output
    /// The entity class is the source
    /// The DTO is the destination/output
    /// </summary>
    public class AutoMapperProfiles : Profile
    {

        //  Destination mappings are done in the constructor 
        //
        public AutoMapperProfiles()
        {
            // <source, destination) --Take in a Source and return a Destination
            //
            CreateMap<User, UserForListDto>()

                .ForMember(dest => dest.PhotoUrl,     //destination member = dto.prop
                    opt => opt.MapFrom(                     //opt to map src =User obj
                        src => src.Photos.FirstOrDefault(    //All Photos for this User
                            p => p.IsMain == true            //Return 1st where IsMain
                        ).Url                        //Of all those a single will return
                    )                                //For that one, give me url prop field
                )
                .ForMember(dest => dest.Age, opt => opt.MapFrom(    //calc age as int
                    u => u.DateOfBirth.CalculateAge()               //to pass to Dto.Age
                ));                                         //Uses extension method



            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(
                    u => u.Photos.Where(p => p.IsMain != false).FirstOrDefault().Url)
                ).ForMember(dest => dest.Age, opt => opt.MapFrom(
                    u => u.DateOfBirth.CalculateAge()
                ));


            CreateMap<Photo, PhotosForDetailedDto>();


            // <source, destination) --Take in a Source and return a Destination
            //
            CreateMap<UserForUpdateDto, User>();
        }
    }
}