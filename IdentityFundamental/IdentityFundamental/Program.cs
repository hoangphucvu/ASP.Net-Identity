using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityFundamental
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var username = "ngohungphuc95@gmail.com";
            var password = "Tony@95";
            //var userStore = new UserStore<IdentityUser>();
            //var userManager = new UserManager<IdentityUser>(userStore);
            var userStore = new CustomUserStore(new CustomerDbContext());
            var userManager = new UserManager<CustomUser, int>(userStore);
            var creationResult = userManager.Create(new CustomUser { UserName = username }, password);
            //create new user
            //var creationResult = userManager.Create(new IdentityUser("ngohungphuc95@gmail.com"), "Tony@95");
            //Console.WriteLine("Created: {0}", creationResult.Succeeded);

            //add claims to specific user
            var user = userManager.FindByName(username);
            //var claimResult = userManager.AddClaim(user.Id, new Claim("given_name", "Tony"));
            //Console.WriteLine("Claim: {0}", claimResult.Succeeded);

            var passwordCheck = userManager.CheckPassword(user, password);
            Console.WriteLine("Password : {0}", passwordCheck);
        }
    }

    public class CustomUser : IUser<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }

    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<CustomUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<CustomUser>();
            user.ToTable("Users");
            user.HasKey(x => x.Id);

            user.Property(x => x.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            user.Property(x => x.UserName).IsRequired().HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));

            base.OnModelCreating(modelBuilder);
        }
    }

    public class CustomUserStore : IUserPasswordStore<CustomUser, int>
    {
        private readonly CustomerDbContext context;

        public CustomUserStore(CustomerDbContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task CreateAsync(CustomUser user)
        {
            context.Users.Add(user);
            return context.SaveChangesAsync();
        }

        public Task UpdateAsync(CustomUser user)
        {
            context.Users.Attach(user);
            return context.SaveChangesAsync();
        }

        public Task DeleteAsync(CustomUser user)
        {
            context.Users.Remove(user);
            return context.SaveChangesAsync();
        }

        public Task<CustomUser> FindByIdAsync(int userId)
        {
            return context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<CustomUser> FindByNameAsync(string userName)
        {
            return context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public Task SetPasswordHashAsync(CustomUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(CustomUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(CustomUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}