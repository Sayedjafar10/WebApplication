using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

public class CVContext : IdentityDbContext<User>
{
    public CVContext(DbContextOptions<CVContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }

    public DbSet<Project> Projekts { get; set; }
    public DbSet<CV> CVs { get; set; }
    public DbSet<Erfarenhet> Erfarenheter { get; set; }
    public DbSet<CVErfarenhet> CVErfarenheter { get; set; }
    public DbSet<Kompetens> Kompetenser { get; set; } 
    public DbSet<CVKompetens> CVKompetenser { get; set; } 
    public DbSet<CVUtbildning> CVUtbildningar { get; set; } 
    public DbSet<Utbildning> Utbildningar { get; set; }
    public DbSet<LoginViewModel> LoginModels { get; set; }
    public DbSet<RegisterViewModel> RegisterModels { get; set; }

    public DbSet<UserParticipationProject> UserParticipationProjects { get; set; }

    public DbSet<Meddelande> Meddelanden {  get; set; }    // Måste man konfigurera någon relation som ni har gjort nedan? /Denis



	// Övriga DbSet-egenskaper och konfigurationer för övriga modeller

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        // Konfigurera många-till-många-relationen mellan Erfarenhet och CV
        modelBuilder.Entity<CVErfarenhet>()
            .HasKey(cve => new { cve.ErfarenhetId, cve.CVId });

        modelBuilder.Entity<CVErfarenhet>()
            .HasOne(cve => cve.Erfarenhet)
            .WithMany(e => e.CVErfarenheter)
            .HasForeignKey(cve => cve.ErfarenhetId);

        modelBuilder.Entity<CVErfarenhet>()
            .HasOne(cve => cve.CV)
            .WithMany(cv => cv.CVErfarenheter)
            .HasForeignKey(cve => cve.CVId);

        // Skapar en många-till-många relation mellan komepetens och Cv
        modelBuilder.Entity<CVKompetens>()
           .HasKey(cvk => new { cvk.CVId, cvk.KompetensId });

        modelBuilder.Entity<CVKompetens>()
            .HasOne(cvk => cvk.CV)
            .WithMany(cv => cv.CVKompetenser)
            .HasForeignKey(cvk => cvk.CVId);

        modelBuilder.Entity<CVKompetens>()
            .HasOne(cvk => cvk.Kompetens)
            .WithMany(k => k.CVKompetenser)
            .HasForeignKey(cvk => cvk.KompetensId);

        //Skapar en många-till-många relation mellan utbildning och Cv
        modelBuilder.Entity<CVUtbildning>()
           .HasKey(cvu => new { cvu.CVId, cvu.UtbildningId });

        modelBuilder.Entity<CVUtbildning>()
            .HasOne(cvu => cvu.CV)
            .WithMany(cv => cv.CVUtbildningar)
            .HasForeignKey(cvu => cvu.CVId);

        modelBuilder.Entity<CVUtbildning>()
            .HasOne(cvu => cvu.Utbildning)
            .WithMany(u => u.CVUtbildningar)
            .HasForeignKey(cvu => cvu.UtbildningId);


        // Många-till-många mellan User och Project. Deltagandet av användare till projekt

        modelBuilder.Entity<UserParticipationProject>()
                .HasKey(up => new { up.UserID, up.ProjectID });

        modelBuilder.Entity<UserParticipationProject>()
            .HasOne(up => up.User)
            .WithMany(u => u.UsersParticipationsProjects)
            .HasForeignKey(up => up.UserID);

        modelBuilder.Entity<UserParticipationProject>()
            .HasOne(up => up.Project)
            .WithMany(p => p.UsersParticipationsProjects)
            .HasForeignKey(up => up.ProjectID);


		// Relationerna mellan användare - konversation - meddelande:


        // Configuring one-to-many relationship for ´sender
        modelBuilder.Entity<Meddelande>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SkickadeMeddelanden) 
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict); 

        // Configuring  one-to-many relationship for receiver
        modelBuilder.Entity<Meddelande>()
            .HasOne(m => m.Receiver)
            .WithMany(u => u.MottagnaMeddelanden)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        //Skapar en-till-många samband mellan User och Project
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Creator) 
            .WithMany(u => u.CreatedProjects)
            .HasForeignKey(p => p.CreatorID)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<User>().HasData(UserTestData.GetTestData().ToArray());
    }

}

