namespace useCase.Areas.Identity.Data
{
    public class Interet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Localisation { get; set; }
        public string IdResponsable { get; set; }
        public Utilisateur Responsable { get; set; }
    }
}
