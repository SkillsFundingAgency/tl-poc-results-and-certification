using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Data
{
    public class ResultsAndCertificationDbContext : DbContext
    {
        public ResultsAndCertificationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<TqRoute> TqRoutes { get; set; }
        public virtual DbSet<TqPathway> TqPathways { get; set; }
        public virtual DbSet<TqSpecialism> TqSpecialisms { get; set; }
        public virtual DbSet<TqAwardingOrganisation> TqAwardingOrganisations { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<TqProvider> TqProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This will singularize all table names
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.Relational().TableName = entityType.DisplayName();
            }
        }
    }
}
