using AutoMapper;
using MongoDB.Bson;
using notebook.Controllers.Resources;
using notebook.Models;

namespace notebook.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain model to API Resource
            CreateMap<ApplicationUser, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(au => au.UserName));
            
            // API Resource To Domain Model
            CreateMap<CreateUserResource, ApplicationUser>()
                .ForMember(au => au.FirstName, opt => opt.MapFrom(ur => ur.FirstName))
                .ForMember(au => au.LastName, opt => opt.MapFrom(ur => ur.LastName))
                .ForMember(au => au.PictureUrl, opt => opt.MapFrom(ur => ur.PictureUrl))
                .ForMember(au => au.Birthdate, opt => opt.MapFrom(ur => ur.Birthdate))
                .ForMember(au => au.UserName, opt => opt.MapFrom(ur => ur.Email))
                .ForMember(au => au.Email, opt => opt.MapFrom(ur => ur.Email));

            CreateMap<CreateRoleResource, ApplicationRole>()
                .ForMember(ar => ar.Name, opt => opt.MapFrom(rr => rr.Name));

            CreateMap<CreateNoteResource, Note>()
                .ForMember(n => n.Category, opt => opt.MapFrom(nr => new Category()))
                .ForMember(n => n.Title, opt => opt.MapFrom(nr => nr.Title))
                .ForMember(n => n.Text, opt => opt.MapFrom(nr => nr.Text));
        }
    }
}