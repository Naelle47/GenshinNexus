namespace GenshinNexus.Models.ProjectModels
{
    public class Character
    {
        // Section : propriétés issues de la table Characters
        public int characterid_pk { get; set; }
        public string name { get; set; }
        public int rarity { get; set; }
        public DateOnly release_date { get; set; }
        public string icon_url { get; set; }
        public int? elementid_fk { get; set; }
        public int weapontypeid_fk { get; set; }
        public int? regionid_fk { get; set; }


        // Section : propriétés de navigation (associations avec d'autres tables)
        // Ici les relations sont du type Many-to-One (car un personnage appartient à une seule région, un seul élément, et un seul type d'arme)
        public Element? Element { get; set; }
        public WeaponType WeaponType { get; set; }
        public Region? Region { get; set; }
    }
}

// Remarques : 
// - Region et regionid_fk sont nullable pour gérer les personnages sans région associée.
// - Element et elementid_fk sont nullable car seul le Traveler n'a pas d'élément associé (il peut changer d'élément à volonté).