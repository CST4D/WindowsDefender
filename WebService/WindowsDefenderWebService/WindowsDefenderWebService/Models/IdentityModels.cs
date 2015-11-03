using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;

namespace WindowsDefenderWebService.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("TowerDefenceModels", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Map> Maps { get; set; }
        public virtual DbSet<MatchHistory> MatchHistories { get; set; }
        public virtual DbSet<MatchHistoryDetail> MatchHistoryDetails { get; set; }
        public virtual DbSet<SpecialAbility> SpecialAbilities { get; set; }
        public virtual DbSet<ThreadPost> ThreadPosts { get; set; }
        public virtual DbSet<Thread> Threads { get; set; }
        public virtual DbSet<Tower> Towers { get; set; }
        public virtual DbSet<Virus> Viruses { get; set; }
    }
}