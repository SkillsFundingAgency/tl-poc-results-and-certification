using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Data
{
    public class ResultsAndCertificationDbContext : DbContext
    {
        public ResultsAndCertificationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<TqRoute> TqRoute { get; set; }
        public virtual DbSet<TqPathway> TqPathway { get; set; }
        public virtual DbSet<TqSpecialism> TqSpecialism { get; set; }
        public virtual DbSet<TqAwardingOrganisation> TqAwardingOrganisation { get; set; }
        public virtual DbSet<Provider> Provider{ get; set; }
        public virtual DbSet<TqProvider> TqProvider { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This will singularize all table names
            //foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    entityType.Relational().TableName = entityType.DisplayName();
            //}
        }
    }
}
