using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MonopolyTest.Models.Base
{
    public class ContextDB : DbContext
    {
        private static ContextDB _context;
        //Коллекция сущности Questionnaire
        public DbSet<Pallet> Pallets { get; set; }
        //Коллекция сущности Country
        public DbSet<Box> Boxes { get; set; }
        //Создание БД
        public ContextDB(DbContextOptions<ContextDB> opt) : base(opt)
        {
            Database.EnsureCreated();
        }
        //Получение контекса
        public static ContextDB GetContext()
        {
            if (_context == null)
            {
                var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

                var db_options = new DbContextOptionsBuilder<ContextDB>()
                   .UseNpgsql(configuration.GetConnectionString("Default"))
                   .Options;

                _context = new ContextDB(db_options);

            }

            return _context; ;
        }
    }
}
