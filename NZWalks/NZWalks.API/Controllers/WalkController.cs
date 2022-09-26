using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalkController(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("region-circuit")]
        public async Task<IActionResult> GetAllAsync()
        {
            var allWalks = await walkRepository.GetAllAsync();
            var allWalksAsOutput = mapper.Map<List<Models.DTO.Walk>>(allWalks);
            return Ok(allWalksAsOutput);
        }

        [HttpGet]
        [Route("region-circuit/{id:guid}")]
        [ActionName("GetWalkByIdAsync")]
        public async Task<IActionResult> GetWalkByIdAsync(Guid id)
        {
            var walk= await walkRepository.GetAsync(id);
            if(walk == null)
                return NotFound();
            return Ok(mapper.Map<Models.DTO.Walk>(walk));

        }
        [HttpDelete]
        [Route("region-circuit/deletewalk/{id:guid}")]
        public async Task<IActionResult> DeleteWalkByIdAsync(Guid id)
        {
           var deletedWalk = await walkRepository.Delete(id);
            if (deletedWalk == null)
                return NotFound();
            return Ok(deletedWalk);
        }

        [HttpPost]
        [Route("region-circuit/AddRegion")]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.CreateWalk newCircuit)
        {
            var newWalk = mapper.Map<Walk>(newCircuit);
            var newlyAddedWalkInDomain = await walkRepository.AddAsync(newWalk);

            Models.DTO.Walk walk = new Models.DTO.Walk()
            {
                Id = newWalk.Id,
                Length = newWalk.Length,
                Name = newWalk.Name
            };

            return CreatedAtAction(nameof(GetWalkByIdAsync), new { id = newlyAddedWalkInDomain.Id }, walk);

        }

        [HttpPut]
        [Route("UpdateWalkById/{id:guid}")]
        public async Task<IActionResult> UpdateWalkById(Guid id, Models.DTO.UpdateWalk walk)
        {
            var domainWalk =  mapper.Map<Walk>(walk);
            domainWalk = await walkRepository.UpdateAsync(id, domainWalk);

            if (domainWalk == null)
                return NotFound();
            var updatedWalk = mapper.Map<Models.DTO.Walk>(domainWalk);
            return Ok(updatedWalk);
        }

    }
}
