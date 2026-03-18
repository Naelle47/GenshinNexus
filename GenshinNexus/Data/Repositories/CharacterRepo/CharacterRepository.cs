using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GenshinNexus.Models.ProjectModels;

namespace GenshinNexus.Data.Repositories.CharacterRepo;

public class CharacterRepository : ICharacterRepository
{
    private readonly IDbConnection _db;

    public CharacterRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Character>> GetFilteredAsync(int? elementId, int? weaponTypeId, int? regionId, int? Rarity)
    {
        const string sql = @"
        SELECT 
            c.characterid_pk,
            c.name,
            c.rarity,
            c.release_date,
            c.icon_url,
            c.elementid_fk,
            c.weapontypeid_fk,
            c.regionid_fk,

            -- ELEMENT
            e.elementid_pk,
            e.name,
            e.icon_url,

            -- WEAPON
            wt.weapontypeid_pk,
            wt.name,
            wt.icon_url,

            -- REGION
            r.regionid_pk,
            r.name,
            r.icon_url

        FROM characters c
        LEFT JOIN elements      e  ON c.elementid_fk     = e.elementid_pk
        JOIN weapon_types       wt ON c.weapontypeid_fk  = wt.weapontypeid_pk
        LEFT JOIN regions       r  ON c.regionid_fk      = r.regionid_pk
        WHERE (@ElementId   IS NULL OR c.elementid_fk     = @ElementId)
          AND (@WeaponTypeId IS NULL OR c.weapontypeid_fk = @WeaponTypeId)
          AND (@RegionId    IS NULL OR c.regionid_fk      = @RegionId)
          AND (@Rarity      IS NULL OR c.rarity           = @Rarity)
        ORDER BY c.name;";

        var result = await _db.QueryAsync<Character, Element, WeaponType, Region, Character>(
            sql,
            (c, element, weapon, region) =>
            {
                c.Element = c.elementid_fk.HasValue ? element : null;
                c.WeaponType = weapon;
                c.Region = c.regionid_fk.HasValue ? region : null;
                return c;
            },
            new { ElementId = elementId, WeaponTypeId = weaponTypeId, RegionId = regionId, Rarity = Rarity },
            splitOn: "elementid_pk,weapontypeid_pk,regionid_pk"
        );

        return result;
    }


    public async Task<IEnumerable<Element>> GetAllElementsAsync()
    {
        const string sql = @"SELECT elementid_pk,
                           name,
                           icon_url
                           FROM elements
                           ORDER BY name;";
        return await _db.QueryAsync<Element>(sql);
    }

    public async Task<IEnumerable<Region>> GetAllRegionsAsync()
    {
        const string sql = @"SELECT regionid_pk,
                           name,
                           icon_url
                           FROM regions
                           ORDER BY name;";
        return await _db.QueryAsync<Region>(sql);
    }

    public async Task<IEnumerable<WeaponType>> GetAllWeaponTypesAsync()
    {
        const string sql = @"SELECT weapontypeid_pk,
                           name,
                           icon_url
                           FROM weapon_types
                           ORDER BY name;";
        return await _db.QueryAsync<WeaponType>(sql);
    }
}
