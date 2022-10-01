
namespace NZWalks.API.Models.DTO
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        //navigational properties
        public DTO.Region Region { get; set; }
        public DTO.WalkDifficulty.WalkDifficulty WalkDifficulty { get; set; }
    }
}
