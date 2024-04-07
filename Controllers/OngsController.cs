using Microsoft.AspNetCore.Mvc;
using OngResgisterApi.Models;
using OngApi.Services;
using System.Net;
using OngResgisterApi.Entities.ViewModels;

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
            try
            {
                var ong = await _ongsService.GetAsync(id);
                return Ok(ong);
            }
            catch (WebException ex)
            {
                return NotFound(new { message = $"{ex.Message}", status = "404", type = "NotFound", success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(OngViewlModel newOng)
        {
            try
            {
            var ong = await _ongsService.CreateAsync(newOng);
            return CreatedAtAction(nameof(Get), new {id= ong.Id}, ong);
            }
            catch (ArgumentException ex)
            {
               return BadRequest(new { message = $"{ex.Message}", status= "400", type= "BadRequest", success= false });
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, OngViewlModel updatedOng)
        {
            try
            {
            await _ongsService.UpdateAsync(id, updatedOng);
            return NoContent();
            }
            catch (WebException ex) { 
                return NotFound(new { message = $"{ex.Message}", status = "404", type = "NotFound", success = false });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = $"{ex.Message}", status = "400", type = "BadRequest", success = false });
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
            await _ongsService.RemoveAsync(id);
                return NoContent();
            }catch (WebException ex)
            {
                return NotFound(new { message = $"{ex.Message}", status = "404", type = "NotFound", success = false });
            }
        }
    }
}
