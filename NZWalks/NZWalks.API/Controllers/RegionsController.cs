using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    //[Route("nz-region")]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();
               
            var regionListObj = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionListObj);
        }

        [HttpGet]
        [Route("nz-region/{Id:guid}")]
        [ActionName("GetRegion")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetRegion(Guid Id)
        {
            var regionDetails = await regionRepository.GetAsync(Id);
            if(regionDetails == null)
                return NotFound();
            return Ok(mapper.Map<Region>(regionDetails));
        }

        [HttpPost]
        [Route("nz-region/AddRegion")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.Region region)
        {
            var regionToPersist = mapper.Map<Models.Domain.Region>(region);
            await regionRepository.AddAsync(regionToPersist);
            mapper.Map<Region>(regionToPersist);
            return CreatedAtAction(nameof(GetRegion), new { Id = regionToPersist.Id }, region);
        }

        [HttpDelete]
        [Route("nz-region/DeleteRegion/{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteRegionById(Guid id)
        {
            var deletedRegion = await regionRepository.Delete(id);
            if (deletedRegion == null)
                return NotFound();

            var region = mapper.Map<Region>(deletedRegion);
            return Ok(region);
        }

        [HttpPut]
        [Route("nz-region/UpdateRegion/{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateRegionByIdAsync(Guid id, Models.DTO.Region region)
        {
            var regionToUpdate = mapper.Map<Models.Domain.Region>(region);
            var updatedRegion = await regionRepository.UpdateAsync(id, regionToUpdate);
            if (updatedRegion == null)
                return NotFound();
            return Ok(region);
        }

       
    }
}
