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
    }
}
