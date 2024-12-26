namespace GESTIONCOMMANDES.Models.Entities
{
    public class ProduitDto
    {
        public string Libelle { get; set; }
        public decimal Prix { get; set; }
        public int QteStock { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
