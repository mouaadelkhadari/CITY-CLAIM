namespace useCase.Areas.Identity.Data;

public class Statut
{
    public int Id { get; set; }
    public int ReclamationId  { get; set; }
    public string status  { get; set; } 
    public Statut()
    {
        status = "0";
    }
}