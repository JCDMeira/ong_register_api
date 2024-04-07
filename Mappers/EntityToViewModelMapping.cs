using AutoMapper;
using OngResgisterApi.Entities.ViewModels;
using OngResgisterApi.Models;

namespace API.Mappers
{
    public class EntityToViewModelMapping : Profile
    {
        public EntityToViewModelMapping()
        {
            #region [Entidades]
            CreateMap<Ong, OngViewlModel>();
            #endregion


        }
    }
}
