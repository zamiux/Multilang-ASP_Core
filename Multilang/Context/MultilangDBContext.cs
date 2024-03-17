using Microsoft.EntityFrameworkCore;
using Multilang.Models;
using System.Globalization;

namespace Multilang.Context
{
    public class MultilangDBContext:DbContext
    {
        public MultilangDBContext(DbContextOptions<MultilangDBContext> options):base(options)
        {
            
        }

        #region dbSet
        public DbSet<New> News{ get; set; }
        public DbSet<Language> Languages { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //default lang
            //string lang = CultureInfo.CurrentCulture.Name;
           // modelBuilder.Entity<New>().HasQueryFilter(n => n.Language.LangTitle == lang);


            base.OnModelCreating(modelBuilder);
        }
    }
}
