using GenshinNexus.Models.ProjectModels;

namespace GenshinNexus.Models.ViewModels
{
    public class CharacterFiltersVM
    {
        public int? ElementId { get; set; }
        public int? WeaponTypeId { get; set; }
        public int? RegionId { get; set; }
        public int? Rarity { get; set; }

        public IEnumerable<Element> Elements { get; set; } = Enumerable.Empty<Element>();
        public IEnumerable<WeaponType> WeaponTypes { get; set; } = Enumerable.Empty<WeaponType>();
        public IEnumerable<Region> Regions { get; set; } = Enumerable.Empty<Region>();
        public IEnumerable<Character> Characters { get; set; } = Enumerable.Empty<Character>();
    }
}
