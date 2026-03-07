using System.Collections.Generic;
using System.Threading.Tasks;
using GenshinNexus.Models.ProjectModels;

namespace GenshinNexus.Data.Repositories.CharacterRepo;

public interface ICharacterRepository
{
    Task<IEnumerable<Character>> GetFilteredAsync(int? elementId, int? weaponTypeId, int? regionId, int? rarity);
    Task<IEnumerable<Element>> GetAllElementsAsync();
    Task<IEnumerable<Region>> GetAllRegionsAsync();
    Task<IEnumerable<WeaponType>> GetAllWeaponTypesAsync();
}
