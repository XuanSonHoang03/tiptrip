using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using TipTrip.IdentityFramework.Constants;

namespace TipTrip.IdentityFramework.DBContext
{
    [Table(TableConstants.IdentityTable, Schema = SchemaConstants.IdentitySchema)]
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public ApplicationDBContext() { }
        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<IdentityRole>().HasData(
				new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
				new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" },
				new IdentityRole { Id = "3", Name = "Manager", NormalizedName = "MANAGER" }
			);
		}
	}

}
