using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.MemberViewModels;
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
        }

        private void ConfigureMemberMappings()
        {

            // Member -> MemberViewModel mapping
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Address, options => options.MapFrom(src => $"{src.Address.City} , {src.Address.Street} , {src.Address.BuildingNumber}"))
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
                .ForMember(dest => dest.Address, options => options.MapFrom(src => $"{src.Address.City} , {src.Address.Street} , {src.Address.BuildingNumber}"))
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

    }
}
