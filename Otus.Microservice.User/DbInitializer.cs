using Microsoft.EntityFrameworkCore;

namespace Otus.Microservice.User;

  public class DbInitializer
    {
        private readonly ModelBuilder _builder;

        public DbInitializer(ModelBuilder builder)
        {
            _builder = builder;
        }

        public void Seed()
        {
            _builder.Entity<Models.User>();
        }
    }