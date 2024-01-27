using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using signiel.Models;

namespace signiel.Contexts;

public partial class SignielContext : DbContext
{
    public SignielContext(DbContextOptions<SignielContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LandingBanner> LandingBanners { get; set; }

    public virtual DbSet<LandingSection> LandingSections { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TripDetail> TripDetails { get; set; }

    public virtual DbSet<TripDetailImage> TripDetailImages { get; set; }

    public virtual DbSet<TripImage> TripImages { get; set; }

    public virtual DbSet<TripRecommend> TripRecommends { get; set; }

    public virtual DbSet<TripRecommendItem> TripRecommendItems { get; set; }

    public virtual DbSet<TripSchedule> TripSchedules { get; set; }

    public virtual DbSet<TripTag> TripTags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_bin")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<LandingBanner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("landing_banners");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .HasColumnName("image");
            entity.Property(e => e.Query)
                .HasMaxLength(200)
                .HasColumnName("query");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<LandingSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("landing_sections");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .HasColumnName("image");
            entity.Property(e => e.Label)
                .HasMaxLength(50)
                .HasColumnName("label");
            entity.Property(e => e.Query)
                .HasMaxLength(200)
                .HasColumnName("query");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trips");

            entity.HasIndex(e => e.Author, "AUTHOR_FK");

            entity.HasIndex(e => e.Content, "details").HasAnnotation("MySql:FullTextIndex", true);

            entity.HasIndex(e => e.Title, "title").HasAnnotation("MySql:FullTextIndex", true);

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Author)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("author");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Days)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("days");
            entity.Property(e => e.Nights)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("nights");
            entity.Property(e => e.Price)
                .HasColumnType("bigint(20)")
                .HasColumnName("price");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Trips)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("AUTHOR_FK");
        });

        modelBuilder.Entity<TripDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trip_details");

            entity.HasIndex(e => e.Trip, "TRIP_DETAILS_TRIP_FK");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(10000)
                .HasColumnName("description");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .HasColumnName("location");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.Trip)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("trip");

            entity.HasOne(d => d.TripNavigation).WithMany(p => p.TripDetails)
                .HasForeignKey(d => d.Trip)
                .HasConstraintName("TRIP_DETAILS_TRIP_FK");
        });

        modelBuilder.Entity<TripDetailImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trip_detail_images");

            entity.HasIndex(e => e.Detail, "TRIP_DETAIL_IMAGES_DETAIL_FK");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Detail)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("detail");
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .HasColumnName("image");

            entity.HasOne(d => d.DetailNavigation).WithMany(p => p.TripDetailImages)
                .HasForeignKey(d => d.Detail)
                .HasConstraintName("TRIP_DETAIL_IMAGES_DETAIL_FK");
        });

        modelBuilder.Entity<TripImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trip_images");

            entity.HasIndex(e => e.Trip, "TRIP_IMAGES_TRIP_FK");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .HasColumnName("image");
            entity.Property(e => e.Trip)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("trip");

            entity.HasOne(d => d.TripNavigation).WithMany(p => p.TripImages)
                .HasForeignKey(d => d.Trip)
                .HasConstraintName("TRIP_IMAGES_TRIP_FK");
        });

        modelBuilder.Entity<TripRecommend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trip_recommend");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<TripRecommendItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trip_recommend_item");

            entity.HasIndex(e => e.Next, "FK_trip_recommend_item_trip_recommend");

            entity.HasIndex(e => e.Recommend, "FK_trip_recommend_item_trip_recommend_2");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .HasColumnName("image");
            entity.Property(e => e.Next)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("next");
            entity.Property(e => e.Query)
                .HasMaxLength(500)
                .HasColumnName("query");
            entity.Property(e => e.Recommend)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("recommend");
            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .HasColumnName("text");

            entity.HasOne(d => d.NextNavigation).WithMany(p => p.TripRecommendItemNextNavigations)
                .HasForeignKey(d => d.Next)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_trip_recommend_item_trip_recommend");

            entity.HasOne(d => d.RecommendNavigation).WithMany(p => p.TripRecommendItemRecommendNavigations)
                .HasForeignKey(d => d.Recommend)
                .HasConstraintName("FK_trip_recommend_item_trip_recommend_2");
        });

        modelBuilder.Entity<TripSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trip_schedules");

            entity.HasIndex(e => e.Trip, "TRIP_SCHEDULES_TRIP_FK");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.Trip)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("trip");

            entity.HasOne(d => d.TripNavigation).WithMany(p => p.TripSchedules)
                .HasForeignKey(d => d.Trip)
                .HasConstraintName("TRIP_SCHEDULES_TRIP_FK");
        });

        modelBuilder.Entity<TripTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trip_tags");

            entity.HasIndex(e => e.Trip, "TRIP_FK");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Key)
                .HasMaxLength(50)
                .HasColumnName("key");
            entity.Property(e => e.Trip)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("trip");
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .HasColumnName("value");

            entity.HasOne(d => d.TripNavigation).WithMany(p => p.TripTags)
                .HasForeignKey(d => d.Trip)
                .HasConstraintName("TRIP_FK");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Name, "nickname").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Nickname)
                .HasMaxLength(50)
                .HasColumnName("nickname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
