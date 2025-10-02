using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Domain.Tasks;
using TaskManagement.Domain.UserDetails;
using TaskManagementWebApi.Models.Tasks;
using TaskManagementWebApi.Models.User;

namespace TaskManagementWebApi
{
    /// <summary>
    /// TaskManagement Auto Mapper Profile
    /// </summary>
    public class TaskManagementAutoMapperProfile : Profile
    {
        /// <summary>
        /// TaskManagement Auto Mapper Profile.
        /// </summary>
        public TaskManagementAutoMapperProfile()
        {     
            createUserMaps();
            createTasksMaps();
        }

        private void createUserMaps()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<IdentityUser, User>()
                .ForMember(x => x.Username, o => o.MapFrom(x => x.UserName))
                .ForMember(x => x.Email, o => o.MapFrom(x => x.Email));
            CreateMap<User, LoginViewModel>();
            CreateMap<User, UserDetailViewModel>();
            CreateMap<User, UserTaskDetailViewModel>();

        }

        private void createTasksMaps()
        {
            CreateMap<Tasks, TaskListViewModel>()
            .ForMember(dest => dest.TaskAssignments,
                opt => opt.MapFrom(src => src.TaskAssignments.Select(a => a.AssigneeId).ToList()));
            CreateMap<ManageTaskViewModel, Tasks>();
        }
     }
}







