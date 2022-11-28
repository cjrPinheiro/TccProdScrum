using PS.Domain.Entities;
using PS.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Persistence
{
    public class PSContext : IdentityDbContext<User,Role,int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public PSContext(DbContextOptions<PSContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<JiraDomain> JiraDomains { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SprintTeamMember> SprintTeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur=> ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur=> ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<SprintTeamMember>(sprintTm =>
            {
                sprintTm.HasKey(ur => new { ur.SprintId, ur.TeamMemberId });
                sprintTm.HasOne(ur => ur.Sprint)
                    .WithMany(r => r.TeamMembers)
                    .HasForeignKey(ur => ur.SprintId)
                    .IsRequired();

                sprintTm.HasOne(ur => ur.TeamMember)
                    .WithMany(u => u.Sprints)
                    .HasForeignKey(ur => ur.TeamMemberId)
                    .IsRequired();
            });
     
        }
    }
}
