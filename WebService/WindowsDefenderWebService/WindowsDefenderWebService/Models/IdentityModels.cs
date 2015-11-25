using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;

namespace WindowsDefenderWebService.Models
{
    
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

    /// <summary>
    /// The context class for the DB entities. Contains a DbSet for each entity set
    /// in the database.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
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

        /// <summary>
        /// Entity set for the user roles table.
        /// </summary>
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        /// <summary>
        /// Entity set for the user claims table.
        /// </summary>
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        /// <summary>
        /// Entity set for the user logins table.
        /// </summary>
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        /// <summary>
        /// Entity set for the users table.
        /// </summary>
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        /// <summary>
        /// Entity set for the friends table.
        /// </summary>
        public virtual DbSet<Friend> Friends { get; set; }
        /// <summary>
        /// Entity set for the maps table.
        /// </summary>
        public virtual DbSet<Map> Maps { get; set; }
        /// <summary>
        /// Entity set for the match histories table.
        /// </summary>
        public virtual DbSet<MatchHistory> MatchHistories { get; set; }
        /// <summary>
        /// Entity set for the match history details table.
        /// </summary>
        public virtual DbSet<MatchHistoryDetail> MatchHistoryDetails { get; set; }
        /// <summary>
        /// Entity set for the special abilities table.
        /// </summary>
        public virtual DbSet<SpecialAbility> SpecialAbilities { get; set; }
        /// <summary>
        /// Entity set for the thread posts table.
        /// </summary>
        public virtual DbSet<ThreadPost> ThreadPosts { get; set; }
        /// <summary>
        /// Entity set for the threads table.
        /// </summary>
        public virtual DbSet<Thread> Threads { get; set; }
        /// <summary>
        /// Entity set for the towers table.
        /// </summary>
        public virtual DbSet<Tower> Towers { get; set; }
        /// <summary>
        /// Entity set for the viruses table.
        /// </summary>
        public virtual DbSet<Virus> Viruses { get; set; }
    }
}