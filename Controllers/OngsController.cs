using Microsoft.AspNetCore.Mvc;
using OngResgisterApi.Models;
using OngResgisterApi.utils;
using restaurant_api.Utils;
using OngApi.Services;
using X.PagedList;

namespace OngResgisterApi.Controllers
{
    [Route("/api/ongs")]
    [ApiController]
    public class OngsController : Controller
    {
        private OngsService _ongsService;

        public OngsController(OngsService service) => _ongsService = service;

        [HttpGet]
        public async Task<ActionResult<List<Ong>>> Get(
            int? page, 
            int? count, 
            [FromQuery] string? name, 
            [FromQuery] string? purpose,
            [FromQuery] string? search,
            [FromQuery] string? how_to_assist
            ) {
            int pageList = page ?? 1;
            int pageCount = count ?? 20;
            var result = await _ongsService.GetAsync(pageList, pageCount,name,purpose,search,how_to_assist);
            return Ok(result);
         }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Ong>> Get(string id)
        {
            var ong = await _ongsService.GetAsync(id);
            if(ong == null) return NotFound();
            return Ok(ong);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Ong newOng)
        {
            var ong = await _ongsService.GetByNameAsync(newOng.Name);
            if(ong != null) return BadRequest();
            if (newOng.ImageUrl == null || newOng.ImageUrl == "")
                newOng.ImageUrl = Image.GetImageFallback();
            if (!Uri.IsWellFormedUriString(newOng.ImageUrl, UriKind.RelativeOrAbsolute)) return BadRequest();
            await _ongsService.CreateAsync(newOng);
            return CreatedAtAction(nameof(Get), new {id= newOng.Id}, newOng);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Ong updatedOng)
        {
            var ong = await _ongsService.GetAsync(id);
            if (ong == null) return NotFound();

            var existingOng = await _ongsService.GetByNameAsync(updatedOng.Name);
            if (existingOng != null && existingOng.Id != id) return BadRequest();

            if (updatedOng.ImageUrl == null || updatedOng.ImageUrl == "")
                updatedOng.ImageUrl = ong.ImageUrl;

            if (!Uri.IsWellFormedUriString(updatedOng.ImageUrl, UriKind.RelativeOrAbsolute)) return BadRequest();
            updatedOng.Id = ong.Id;
            await _ongsService.UpdateAsync(id, updatedOng);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ong = await _ongsService.GetAsync(id);
            if(ong == null) return NotFound();
            await _ongsService.RemoveAsync(id);
            return NoContent();
        }
    }
}
