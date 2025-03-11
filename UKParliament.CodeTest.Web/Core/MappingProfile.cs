using AutoMapper;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonViewModel>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

        CreateMap<PersonViewModel, Person>()
            .ForMember(dest => dest.Department, opt => opt.Ignore()); // Ignoring Department mapping
    }
}
