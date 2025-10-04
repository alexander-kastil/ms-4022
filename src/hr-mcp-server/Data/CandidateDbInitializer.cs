using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HRMCPServer.Data;

public static class CandidateDbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;

        var logger = scopedServices.GetRequiredService<ILoggerFactory>()
            .CreateLogger(typeof(CandidateDbInitializer));
        var context = scopedServices.GetRequiredService<CandidateDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (await context.Candidates.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Candidate database already contains data; skipping seed.");
            return;
        }

        var seedCandidates = GetSeedCandidates();

        await context.Candidates.AddRangeAsync(seedCandidates, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded {Count} candidates into the database.", seedCandidates.Count);
    }

    private static List<Candidate> GetSeedCandidates() => new()
    {
        new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            SpokenLanguages = new List<string> { "English", "Spanish" },
            Skills = new List<string> { "Recruitment", "Employee Relations", "HRIS" },
            CurrentRole = "HR Manager"
        },
        new Candidate
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            SpokenLanguages = new List<string> { "English", "French" },
            Skills = new List<string> { "Talent Acquisition", "Onboarding", "Training" },
            CurrentRole = "HR Specialist"
        },
        new Candidate
        {
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice.johnson@example.com",
            SpokenLanguages = new List<string> { "English", "German" },
            Skills = new List<string> { "Compensation and Benefits", "Performance Management", "Employee Engagement" },
            CurrentRole = "HR Business Partner"
        },
        new Candidate
        {
            FirstName = "Bob",
            LastName = "Brown",
            Email = "bob.brown@example.com",
            SpokenLanguages = new List<string> { "English", "Italian" },
            Skills = new List<string> { "HR Policy Development", "Conflict Resolution", "Diversity and Inclusion" },
            CurrentRole = "HR Director"
        },
        new Candidate
        {
            FirstName = "Emily",
            LastName = "Davis",
            Email = "emily.davis@example.com",
            SpokenLanguages = new List<string> { "English", "Portuguese" },
            Skills = new List<string> { "Workforce Planning", "HR Analytics", "Change Management" },
            CurrentRole = "HR Consultant"
        }
    };
}
