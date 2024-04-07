using OngResgisterApi.Models;
using API.Infra;
using restaurant_api.Utils;
using X.PagedList;
using OngResgisterApi.utils;
using System.Net;
using AutoMapper;
using OngResgisterApi.Entities.ViewModels;

namespace OngApi.Services;

public class OngsService
{
    private readonly IMapper _mapper;
    private readonly IMongoRepository<Ong> _ong;

    public OngsService(
       IMongoRepository<Ong> ong, IMapper mapper)
    {
        _mapper = mapper;
        _ong = ong;
    }

    public async Task<IPagedList<Ong>> GetAsync(int page, int count, string? name,
             string? purpose,
            string? search,
             string? how_to_assist) {
        var result =  await _ong.Get();
        var pagedResult = result
        .Where(ong =>
                   EntintyFilters.HasSearchString(ong.Name, name) &&
                   EntintyFilters.HasSearchString(ong.Purpose, purpose) &&
                   ong.HowToAssist.Any(s => EntintyFilters.HasSearchString(s, how_to_assist)) &&
                       (
                       EntintyFilters.HasSearchString(ong.Name, search) ||
                       EntintyFilters.HasSearchString(ong.Purpose, search) ||
                       ong.HowToAssist.Any(s => EntintyFilters.HasSearchString(s, search))
                       )
                   )
                   .ToPagedList(page,count);
        
        return pagedResult;
    }

    public async Task<OngViewlModel> GetAsync(string id){
        var ong = await _ong.Get(id);
        if (ong == null) throw new WebException("An Ong with this id is not found");
        return _mapper.Map<OngViewlModel>(ong);
}
    public async Task<OngViewlModel> GetByNameAsync(string name){
        var ong = await _ong.GetByName(name);
        return _mapper.Map<OngViewlModel>(ong);
    }
    public async Task<Ong> CreateAsync(OngViewlModel newOng){

        var ong = await _ong.GetByName(newOng.Name);
        if (ong != null) throw new ArgumentException("An Ong with this name already exists");
        
        if (newOng.ImageUrl == null || newOng.ImageUrl == "")  newOng.ImageUrl = Image.GetImageFallback();
        if (!Uri.IsWellFormedUriString(newOng.ImageUrl, UriKind.RelativeOrAbsolute)) throw new ArgumentException("The format for the image link is bad structured");


        var ongEntity = new Ong(newOng.Name, newOng.Description,newOng.ImageUrl, newOng.Purpose, newOng.HowToAssist);
        await _ong.Create(ongEntity);
        return ongEntity;
    }

    public async Task UpdateAsync(string id, OngViewlModel updatedOng){
        var ong = await _ong.Get(id);
        if (ong == null) throw new WebException("An Ong with this id is not found");

        var existingOng = await _ong.GetByName(updatedOng.Name);
        if (existingOng != null && existingOng.Id != id) throw new ArgumentException("An Ong with this name already exists");

        if (updatedOng.ImageUrl == null || updatedOng.ImageUrl == "")
            updatedOng.ImageUrl = ong.ImageUrl;

        if (!Uri.IsWellFormedUriString(updatedOng.ImageUrl, UriKind.RelativeOrAbsolute)) throw new ArgumentException("The format for the image link is bad structured");
        updatedOng.Id = ong.Id;
        await _ong.Update(id, _mapper.Map<Ong>(updatedOng));
}
 
    public async Task RemoveAsync(string id)
    {
        var ong = await _ong.Get(id);
        if (ong == null) throw new WebException("An Ong with this id is not found");
        await _ong.Remove(id);
    }
}