
using AutoMapper;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Region;
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
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();

            var regionListObj = mapper.Map<List<RegionDTO>>(regions);

            return Ok(regionListObj);
        }

        [HttpGet]
        [Route("nz-region/{Id:guid}")]
        [ActionName("GetRegion")]
        public async Task<IActionResult> GetRegion(Guid Id)
        {
            var regionDetails = await regionRepository.GetAsync(Id);
            if(regionDetails == null)
                return NotFound();
            return Ok(mapper.Map<RegionDTO>(regionDetails));
        }

        [HttpPost]
        [Route("nz-region/AddRegion")]
        public async Task<IActionResult> AddRegionAsync(RegionDTO region)
        {
            var regionToPersist = mapper.Map<Region>(region);
            await regionRepository.AddAsync(regionToPersist);
            mapper.Map<RegionDTO>(regionToPersist);
            return CreatedAtAction(nameof(GetRegion), new { Id = regionToPersist.Id }, region);
        }

        [HttpDelete]
        [Route("nz-region/DeleteRegion/{id:guid}")]
        public async Task<IActionResult> DeleteRegionById(Guid id)
        {
            var deletedRegion = await regionRepository.Delete(id);
            if (deletedRegion == null)
                return NotFound();

            var region = mapper.Map<RegionDTO>(deletedRegion);
            return Ok(region);
        }

        [HttpPut]
        [Route("nz-region/UpdateRegion/{id:guid}")]
        public async Task<IActionResult> UpdateRegionByIdAsync(Guid id, RegionDTO region)
        {
            var regionToUpdate = mapper.Map<Region>(region);
            var updatedRegion = await regionRepository.UpdateAsync(id, regionToUpdate);
            if (updatedRegion == null)
                return NotFound();
            return Ok(region);
        }

       
    }
}
