using System;
using System.ComponentModel.DataAnnotations;

namespace useCase.Areas.Identity.Data;

public class Reclamation
{
    public int Id { get; set; }   

    public string NomUtilisateur { get; set; }

    public string PrenomUtilisateur { get; set; }

    public DateTime DateCreation { get; set; }

    public string InteretName { get; set; }

    public string Description { get; set; }

    public string Address { get; set; }

    public Interet Interet { get; set; }
    
    public string status { get; set; }
    public Reclamation()
    {
        status = "Pending"; 
    }

}
