using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.WalkDifficulty;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository,IMapper mapper)
        {
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/GetAllWalkDifficulties")]
        public async Task<IActionResult> GetAllWalkDifficultyAsync()
        {
            var walkDifficulties = await _walkDifficultyRepository.GetAllAsync();
            return Ok(_mapper.Map<List<Models.DTO.WalkDifficulty.WalkDifficulty>>(walkDifficulties));
        }

        [HttpGet]
        [Route("/GetWalkDifficultyById/{id:guid}")]
        [ActionName("GetWalkDifficultyByIdAsync")]
        public async Task<IActionResult> GetWalkDifficultyByIdAsync(Guid id)
        {
            var walkDifficulty = await _walkDifficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
                return NotFound();
            return Ok(_mapper.Map<Models.DTO.WalkDifficulty.WalkDifficulty>(walkDifficulty));
        }
        [HttpPost]
        [Route("/AddWalkDifficultyLevel")]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.WalkDifficulty.CreateWalkDifficulty walkDifficulty)
        {
            var _walkDifficulty = _mapper.Map<Models.Domain.WalkDifficulty>(walkDifficulty);
            var addedWalkDifficultyLevel = await _walkDifficultyRepository.AddAsync(_walkDifficulty);
            var newlyAddedWalkDifficulty = _mapper.Map<Models.DTO.WalkDifficulty.WalkDifficulty>(addedWalkDifficultyLevel);
 
            return CreatedAtAction(nameof(GetWalkDifficultyByIdAsync), new { id = addedWalkDifficultyLevel.Id }, newlyAddedWalkDifficulty);

        }
        [HttpDelete]
        [Route("/RemoveWalkDifficultyById/{id:guid}")]
        public async Task<IActionResult> DeleteWalkDiffiuclty(Guid id)
        {
            var deletedWalkDifficulty = await _walkDifficultyRepository.Delete(id);
            if (deletedWalkDifficulty == null)
                return NotFound();
            return Ok(_mapper.Map<Models.DTO.WalkDifficulty.WalkDifficulty>(deletedWalkDifficulty));

        }

        [HttpPut]
        [Route("/UpdateWalkDifficulty")]
        public async Task<IActionResult> UpdateWalkDifficulty(Guid id, CreateWalkDifficulty walkDifficulty)
        {
            var domainWalkDifficulty = _mapper.Map<Models.Domain.WalkDifficulty>(walkDifficulty);
            var updatedWalkDifficulty = await _walkDifficultyRepository.UpdateAsync(id,domainWalkDifficulty);

            if (updatedWalkDifficulty == null)
                return NotFound();
            return Ok(_mapper.Map<Models.DTO.WalkDifficulty.WalkDifficulty>(updatedWalkDifficulty));
        }
    }
}
