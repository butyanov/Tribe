using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tribe.Core.Models.User;
using Tribe.Data.Abstractions;
namespace Tribe.Data;

public class DataContext(DbContextOptions<DataContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IDataContext;