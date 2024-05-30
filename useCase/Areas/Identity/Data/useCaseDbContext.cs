using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using useCase.Areas.Identity.Data;

namespace useCase.Data;

public class useCaseDbContext : IdentityDbContext<Utilisateur>
{
    public useCaseDbContext(DbContextOptions<useCaseDbContext> options)
        : base(options)
    {
    }


    public DbSet<Interet> Interets { get; set; }
    public DbSet<Reclamation> Reclamations { get; set; }
    public DbSet<Statut> Statuts { get; set; }

    public void CreerReclamationAvecStatut(Reclamation reclamation, string status)
    {
        // Ajouter la réclamation à la base de données
        this.Reclamations.Add(reclamation);
        this.SaveChanges();

        // Créer un nouveau statut avec l'id de la réclamation créée
        var statut = new Statut
        {
            ReclamationId = reclamation.Id,
            status = status
        };

        // Ajouter le statut à la base de données
        this.Statuts.Add(statut);
        this.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
