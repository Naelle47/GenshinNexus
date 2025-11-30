using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Dapper;
using GenshinNexus.Models.ProjectModels;

namespace GenshinNexus.Controllers
{
    public class CharacterController : Controller
    {
        private readonly string _connectionString;

        public CharacterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GenshinNexus")
                ?? throw new Exception("Connection string 'GenshinNexus' not found.");
        }

        public IActionResult Index()
        {
            // Récupération de la liste des personnages avec leurs éléments, types d'armes et régions associés
            // Utilisation de LEFT JOIN pour les éléments et régions car certains personnages peuvent ne pas en avoir (ex: Voyageur, personnages sans région)
            // Utilisation de JOIN pour les types d'armes car tous les personnages ont un type d'arme
            // Tri par nom de personnage, cela aura son importance avec l'ajout de nouveaux personnages.
            // L'incrémentation de l'ID dans la base de données ne garantit pas un ordre alphabétique d'où l'importance du ORDER BY sur le nom.
            const string query = @"
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
            e.elementid_pk AS elementid_pk,
            e.name         AS name,

            -- WEAPON
            wt.weapontypeid_pk AS weapontypeid_pk,
            wt.name            AS name,

            -- REGION
            r.regionid_pk AS regionid_pk,
            r.name        AS name

        FROM Characters c
        LEFT JOIN elements e      ON c.elementid_fk     = e.elementid_pk
        JOIN weapon_types wt       ON c.weapontypeid_fk = wt.weapontypeid_pk
        LEFT JOIN regions r        ON c.regionid_fk      = r.regionid_pk
        ORDER BY c.name;
    ";

            // Création de la connexion à la base de données
            using var connection = new NpgsqlConnection(_connectionString);
            // Exécution de la requête et mappage des résultats aux objets Character, Element, WeaponType et Region
            // Mappage multi-objet avec Dapper
            // On prend un objet Character et on lui associe les objets Element, WeaponType et Region récupérés
            // Pour retourner un seul objet Character avec ses propriétés de navigation correctement assignées.
            var characters = connection.Query<Character, Element, WeaponType, Region, Character>(
                query,
                (c, element, weapon, region) =>
                {
                    // Association des objets récupérés aux propriétés de navigation de Character
                    // Gestion des valeurs nulles pour Element et Region
                    // Expression ternaire pour Element et Region. On vérifie si les clés étrangères ont une valeur avant d'assigner l'objet associé.
                    c.Element = c.elementid_fk.HasValue ? element : null;
                    c.WeaponType = weapon;
                    c.Region = c.regionid_fk.HasValue ? region : null;
                    return c;
                },
                splitOn: "elementid_pk,weapontypeid_pk,regionid_pk"
            ).ToList();

            return View(characters);
        }

        public IActionResult Details(int id)
        {
            // Dans cette méthode, on va récupérer les détails d'un personnage spécifique en fonction de son ID.
            // On va également récupérer les informations nécessaires pour bien "monter" un personnage.
            // C'est à dire les armes dont le personnage peut se servir et les artéfacts qu'il peut équiper.
            // Ces données constituent un Build mais il se peut qu'un personnage ait plusieurs builds possibles.

            return View();
        }

    }
}
