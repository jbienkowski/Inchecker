using System.Data.Entity;

namespace Inchecker.Entities
{
    class IncheckerDbCtx : DbContext
    {
        public IncheckerDbCtx()
            : base("incheckerDb")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Incheck> Inchecks { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
