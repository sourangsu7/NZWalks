
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    //[Route("Region")]
    [Route("nz-region")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;

        public RegionsController(IRegionRepository regionRepository)
        {
            this.regionRepository = regionRepository;
        }
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    #region Hardcoded entity to Test API end point
        //    //var regions= new List<Region>();
        //    //regions.Add(new Region()
        //    //{
        //    //    Id = Guid.NewGuid(),
        //    //    Latitude = -36.840556,
        //    //    Longitude = 174.74,
        //    //    Code = "AUK",
        //    //    Area = 607.10,
        //    //    Population = 1463000
        //    //});
        //    //regions.Add(new Region()
        //    //{
        //    //    Id = Guid.NewGuid(),
        //    //    Latitude = -43.53,
        //    //    Longitude = 172.620278,
        //    //    Code = "ChCh",
        //    //    Area = 295.15,
        //    //    Population = 380600
        //    //});
        //    #endregion

        //    var regions = regionRepository.GetAll();
        //    return Ok(regions);
        //}
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllRegionsAsync();
            return Ok(regions);
        }
    }
}
