using Microsoft.EntityFrameworkCore;
using motel.Models.Domain;
//using motel.Repositories;

namespace motel.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasOne(u => u.role).WithMany(a => a.user).HasForeignKey(ai => ai.roleId);
            modelBuilder.Entity<User>().HasMany(u => u.post).WithOne(u => u.user).HasForeignKey(ai => ai.userId);
            modelBuilder.Entity<Post>().HasOne(u => u.user).WithMany(u => u.post).HasForeignKey(u => u.userId).OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Post>().HasMany(pm => pm.post_manage).WithOne(p => p.post).HasForeignKey(pi => pi.postId);
            modelBuilder.Entity<Post>().HasOne(pm => pm.post_manage).WithOne(p => p.post).HasForeignKey<Post_Manage>(pm=>pm.postId);
            
            //modelBuilder.Entity<Tiers>().HasMany(u => u.user).WithOne(t => t.).HasForeignKey(t => t.tierId);
            modelBuilder.Entity<Tier_User>().HasOne(t => t.tiers).WithMany(tu => tu.tier_user).HasForeignKey(t => t.tierId);
            modelBuilder.Entity<Tier_User>().HasOne(t => t.user).WithOne(ut =>ut.users_tier).HasForeignKey<Tier_User>(t => t.userId);
            //modelBuilder.Entity<Post_Manage>().HasOne(p => p.post).WithMany(p => p.post_manage).HasForeignKey(pi => pi.postId).OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Post_Manage>().HasOne(p => p.post).WithOne(p => p.post_manage).HasForeignKey<Post>(pm => pm.Id);
            modelBuilder.Entity<Post_Manage>().HasOne(u => u.user).WithMany(u => u.post_manage).HasForeignKey(pi => pi.userAdminId);
            modelBuilder.Entity<Post_Category>().HasOne(c => c.category).WithMany(pc => pc.post_category).HasForeignKey(ci => ci.categoryId);
            modelBuilder.Entity<Post_Category>().HasOne(r => r.post).WithMany(rc => rc.post_category).HasForeignKey(ri => ri.postId);
            modelBuilder.Entity<Post>().Property(p => p.price).HasPrecision(18, 2);
            modelBuilder.Entity<Tiers>().Property(p => p.price).HasPrecision(18, 2);
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Post_Category> Post_Category { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Tier_User> Tier_User { get; set; }
        public DbSet<Post_Manage> Post_Manage { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Tiers> Tiers { get; set; }
        public DbSet<Role> Role { get; set; }

    }
}

