using AutoMapper;
using GymManagementBLL.ViewModels.AdminManamentViewModels;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            // Add your mapping configurations here
            ConfigureMemberMappings();
            ConfigureTrainerMappings();
            ConfigurePlanMappings();
            ConfigureSessionMappings();
            ConfigureMembershipMappings();
            ConfigureBookingMappings();
            ConfigureAdminMappings();
        }

        private void ConfigureMemberMappings()
        {

            // Member -> MemberViewModel mapping
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Address, options => options.MapFrom(src => $"{src.Address.BuildingNumber} , {src.Address.Street} , {src.Address.City}"))
                .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ToShortDateString()));

            // CreateMemberViewModel -> Member mapping

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, options => options.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    BuildingNumber = src.BuildingNumber
                }))
                .ForMember(dest => dest.HealthRecord, options => options.MapFrom(src => src.HealthRecordViewModel));

            // HealthRecord -> HealthRecordViewModel  mapping

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            // Member -> MemberToUpdateViewModel mapping

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.City, options => options.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, options => options.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.BuildingNumber, options => options.MapFrom(src => src.Address.BuildingNumber));

            // MemberToUpdateViewModel -> Member mapping
            CreateMap<MemberToUpdateViewModel, Member>()
               .ForMember(dest => dest.Name, options => options.Ignore())
               .ForMember(dest => dest.Photo, options => options.Ignore())
               .AfterMap((src, dest) => // After mapping, update the nested Address object Not replacing it Or Change The reference
               {
                   dest.Address.City = src.City;
                   dest.Address.Street = src.Street;
                   dest.Address.BuildingNumber = src.BuildingNumber;
                   dest.UpdatedAt = DateTime.Now;
               });



        }

        private void ConfigureTrainerMappings()
        {

            // Trainer -> TrainerViewModel mapping

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Address, options => options.MapFrom(src => $"{src.Address.BuildingNumber} , {src.Address.Street} , {src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ToString()));

            // CreateTrainerViewModel -> Trainer mapping

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, options => options.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    BuildingNumber = src.BuildingNumber
                }));

            // Trainer -> TrainerToUpdateViewModel mapping

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.City, options => options.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, options => options.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.BuildingNumber, options => options.MapFrom(src => src.Address.BuildingNumber));

            // TrainerToUpdateViewModel -> Trainer mapping

            CreateMap<TrainerToUpdateViewModel, Trainer>()
               .ForMember(dest => dest.Name, options => options.Ignore())
               .AfterMap((src, dest) => // After mapping, update the nested Address object Not replacing it Or Change The reference
               {
                   dest.Address.City = src.City;
                   dest.Address.Street = src.Street;
                   dest.Address.BuildingNumber = src.BuildingNumber;
                   dest.UpdatedAt = DateTime.Now;
               });

        }

        private void ConfigurePlanMappings()
        {
            // Plan -> PlanViewModel mapping
            CreateMap<Plan, PlanViewModel>();

            // plan -> UpdatePlanViewModel mapping (1)

            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.PlanName, options => options.MapFrom(src => src.Name));

            // UpdatePlanViewModel -> plan

            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, options => options.Ignore())
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(src => DateTime.Now));

        }

        private void ConfigureSessionMappings()
        {

            // Session - SessionViewModel
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, options => options.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.CategoryName, options => options.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.AvailableSlots, options => options.Ignore());

            // CreateSessionViewModel -> Session

            CreateMap<CreateSessionViewModel, Session>();

            // Session -> UpdateSessionViewModel
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.CategoryName));
            CreateMap<Trainer, TrainerSelectViewModel>();

        }

        private void ConfigureMembershipMappings()
        {



            // Membership - MembershipViewModel

            CreateMap<Membership, MembershipViewModel>()
                .ForMember(dest => dest.StartDate, options => options.MapFrom(src => src.Plan.CreatedAt))
                .ForMember(dest => dest.PlanName, options => options.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.MemberName, options => options.MapFrom(src => src.Member.Name));

            // Membership - MembershipForMemberViewModel

            CreateMap<Membership, MembershipForMemberViewModel>()
                 .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                 .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
                 .ForMember(dist => dist.StartDate, Option => Option.MapFrom(X => X.CreatedAt));

            CreateMap<CreateMembershipViewModel, Membership>();

            CreateMap<Plan, PlanSelectListViewModel>();
            CreateMap<Member, MemberSelectListViewModel>();
        }

        private void ConfigureBookingMappings()
        {
            // Booking - MemberForSessionViewModel
            CreateMap<Booking, MemberForSessionViewModel>()
                .ForMember(dest => dest.MemberName, options => options.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.BookingDate, options => options.MapFrom(src => src.CreatedAt));

            // CreateBookingViewModel - Booking

            CreateMap<CreateBookingViewModel, Booking>()
                .ForMember(dest => dest.IsAttended, opt => opt.MapFrom(src => false));

        }

        private void ConfigureAdminMappings()
        {
            // ApplicationUser => AdminListViewModel
            CreateMap<ApplicationUser, AdminListViewModel>();

            // CreateAdminViewModel => ApplicationUser
            CreateMap<CreateAdminViewModel, ApplicationUser>();

            // EditAdminViewModel => ApplicationUser
            CreateMap<EditAdminViewModel, ApplicationUser>();

            // ApplicationUser => EditAdminViewModel
            CreateMap<ApplicationUser, EditAdminViewModel>();

            // ApplicationUser => ManageRolesViewModel (جزئي)
            CreateMap<ApplicationUser, ManageRolesViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }

    }
}
