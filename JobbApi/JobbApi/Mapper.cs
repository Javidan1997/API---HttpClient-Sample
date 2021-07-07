using AutoMapper;
using JobbApi.Api.Client.DTOs;
using JobbApi.Api.Manage.DTOs;
using JobbApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Company, CompanyGetDto>();
            CreateMap<Company, CompanyItemDto>();

            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, Api.Manage.DTOs.CategoryItemDto>();
            CreateMap<Category, Api.Client.DTOs.CategoryItemDto>();
            CreateMap<Category, CategoryGetDto>();

            CreateMap<CityCreateDto, City>();
            CreateMap<City, CityItemDto>();
            CreateMap<City, CityGetDto>();

            CreateMap<CountryCreateDto, Country>();
            CreateMap<Country, CountryItemDto>();
            CreateMap<Country, CountryGetDto>();

            CreateMap<JobCreateDto, Job>();
            CreateMap<Category, CategoryInJobDto>();
            CreateMap<City, CityInJobDto>();
            CreateMap<Country, CountryInJobDto>();
            CreateMap<Company, CompanyInJobDto>();
            CreateMap<Job, JobGetDto>();
            CreateMap<Job, JobItemDto>()
                .ForMember(dest => dest.CategoryName, from => from.MapFrom(x => x.Category.Name))
                .ForMember(dest => dest.CityName, from => from.MapFrom(x => x.City.Name))
                .ForMember(dest => dest.CountryName, from => from.MapFrom(x => x.Country.Name))
                .ForMember(dest => dest.CompanyName, from => from.MapFrom(x => x.Company.Name));

            CreateMap<Job, JobGetItemDto>()
               .ForMember(dest => dest.CityName, from => from.MapFrom(x => x.City.Name))
               .ForMember(dest => dest.CompanyName, from => from.MapFrom(x => x.Company.Name));

            CreateMap<Job, JobFilterItemDto>();
            CreateMap<Job, JobSearchItemDto>()
                .ForMember(dest => dest.CompanyName, from => from.MapFrom(x => x.Company.Name));

            CreateMap<Job, JobApplyDto>();
            CreateMap<Job, JobApplyItemDto>()
             .ForMember(dest => dest.CompanyName, from => from.MapFrom(x => x.Company.Name));

            CreateMap<Job, JobBookmarkDto>();
            CreateMap<Job, JobBookmarkItemDto>()
            .ForMember(dest => dest.CompanyName, from => from.MapFrom(x => x.Company.Name));

            CreateMap<CandidateCreateDto, Candidate>();
            CreateMap<AppUser, AppUserInCandidateDto>();
            CreateMap<Job, JobInCandidateDto>();
            CreateMap<Candidate, CandidateGetDto>();
            CreateMap<Candidate, CandidateItemDto>()
                .ForMember(dest => dest.JobTitle, from => from.MapFrom(x => x.Job.Title))
                .ForMember(dest => dest.JobDeadline, from => from.MapFrom(x => x.Job.Deadline))
                .ForMember(dest => dest.AppUserFullName, from => from.MapFrom(x => x.AppUser.FullName))
                .ForMember(dest => dest.AppUserPhoto, from => from.MapFrom(x => x.AppUser.Photo))
                .ForMember(dest => dest.AppUserAddress, from => from.MapFrom(x => x.AppUser.Address))
                .ForMember(dest => dest.AppUserOccupation, from => from.MapFrom(x => x.AppUser.Occupation));







        }
    }
}
