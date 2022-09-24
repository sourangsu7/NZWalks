//using AutoMapper;
//using Microsoft.AspNetCore.Mvc;
//using NZWalks.API.Models.DTO.Region;
//using NZWalks.API.Repository;

//namespace NZWalks.API.Controllers
//{
//    [ApiController]
//    //[Route("NZ-Geo")]
//    [Route("[controller]")]
//    public class NZGeographyController : Controller
//    {
//        private readonly IRegionRepository regionRepository;
//        private readonly IMapper mapper;

//        public NZGeographyController(IRegionRepository regionRepository,IMapper mapper)
//        {
//            this.regionRepository = regionRepository;
//            this.mapper = mapper;
//        }
//        //[HttpGet("region/{id:guid}")]
//        [HttpGet]
//        [Route("region/{id:guid}")]
//        public async Task<IActionResult> GetRegionAsync(Guid id)
//        {
//            var regionDetail = await regionRepository.GetAsync(id);

//            if(regionDetail == null)
//                return NotFound();

//            return Ok(mapper.Map<RegionDTO>(regionDetail));
//        }
//        //[HttpGet("regions/")]
//        [HttpGet]
//        [Route("regions/")]
//        public async Task<IActionResult> GetAllAsync()
//        {
//            var regions = await regionRepository.GetAllAsync();

//            return Ok(mapper.Map<List<RegionDTO>>(regions));
//        }
//    }
//}
