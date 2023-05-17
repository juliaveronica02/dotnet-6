using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication5.Entities;

public partial class DotnetAuthenticationContext : DbContext
{
    public DotnetAuthenticationContext()
    {
    }

    public DotnetAuthenticationContext(DbContextOptions<DotnetAuthenticationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=Hallo123$;database=dotnet-authentication");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .HasColumnName("username");
			entity.Property(e => e.Password)
				.HasColumnType("text")
				.HasColumnName("password");
	});

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
