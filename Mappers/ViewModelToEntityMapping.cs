using AutoMapper;
using OngResgisterApi.Entities.ViewModels;
using OngResgisterApi.Models;

namespace API.Mappers
{
    public class ViewModelToEntityMapping : Profile
    {
        public ViewModelToEntityMapping()
        {
            CreateMap<OngViewlModel, Ong>();
        }
    }
}
