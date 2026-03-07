using GenshinNexus.Data.Repositories.CharacterRepo;
using GenshinNexus.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GenshinNexus.Controllers;

public class CharacterController : Controller
{
    private readonly ICharacterRepository _characterRepo;

    public CharacterController(ICharacterRepository characterRepo)
    {
        _characterRepo = characterRepo;
    }

    public async Task<IActionResult> Index(int? elementId, int? weaponTypeId, int? regionId, int? Rarity)
    {
        var vm = new CharacterFiltersVM
        {
            ElementId = elementId,
            WeaponTypeId = weaponTypeId,
            RegionId = regionId,
            Elements = await _characterRepo.GetAllElementsAsync(),
            Regions = await _characterRepo.GetAllRegionsAsync(),
            WeaponTypes = await _characterRepo.GetAllWeaponTypesAsync(),
            Characters = await _characterRepo.GetFilteredAsync(elementId, weaponTypeId, regionId, Rarity)
        };

        return View(vm);
    }

    public IActionResult Details(int id)
    {
        // Tu implémenteras ici plus tard avec le repo (GetByIdAsync, etc.)
        return View();
    }
}
