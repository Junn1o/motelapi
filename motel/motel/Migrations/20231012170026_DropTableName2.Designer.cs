﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using motel.Data;

#nullable disable

namespace motel.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231012170026_DropTableName2")]
    partial class DropTableName2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("motel.Models.Domain.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("motel.Models.Domain.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("actualFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("area")
                        .HasColumnType("int");

                    b.Property<DateTime>("datecreatedroom")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isHire")
                        .HasColumnType("bit");

                    b.Property<decimal>("price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("motel.Models.Domain.Post_Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("categoryId")
                        .HasColumnType("int");

                    b.Property<int>("postId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("categoryId");

                    b.HasIndex("postId");

                    b.ToTable("Post_Category");
                });

            modelBuilder.Entity("motel.Models.Domain.Post_Manage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("dateapproved")
                        .HasColumnType("datetime2");

                    b.Property<int>("postId")
                        .HasColumnType("int");

                    b.Property<string>("reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("userAdminId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("postId");

                    b.HasIndex("userAdminId");

                    b.ToTable("Post_Manage");
                });

            modelBuilder.Entity("motel.Models.Domain.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("rolename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("motel.Models.Domain.Tier_User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("credit")
                        .HasColumnType("int");

                    b.Property<DateTime?>("expireDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("regDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("tierId")
                        .HasColumnType("int");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("tierId");

                    b.HasIndex("userId")
                        .IsUnique();

                    b.ToTable("Tier_User");
                });

            modelBuilder.Entity("motel.Models.Domain.Tiers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("tiername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tiers");
                });

            modelBuilder.Entity("motel.Models.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("actualFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("birthday")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("datecreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("gender")
                        .HasColumnType("bit");

                    b.Property<string>("lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("roleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("roleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("motel.Models.Domain.Post", b =>
                {
                    b.HasOne("motel.Models.Domain.User", "user")
                        .WithMany("post")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("motel.Models.Domain.Post_Category", b =>
                {
                    b.HasOne("motel.Models.Domain.Category", "category")
                        .WithMany("post_category")
                        .HasForeignKey("categoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("motel.Models.Domain.Post", "post")
                        .WithMany("post_category")
                        .HasForeignKey("postId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");

                    b.Navigation("post");
                });

            modelBuilder.Entity("motel.Models.Domain.Post_Manage", b =>
                {
                    b.HasOne("motel.Models.Domain.Post", "post")
                        .WithMany("post_manage")
                        .HasForeignKey("postId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("motel.Models.Domain.User", "user")
                        .WithMany("post_manage")
                        .HasForeignKey("userAdminId");

                    b.Navigation("post");

                    b.Navigation("user");
                });

            modelBuilder.Entity("motel.Models.Domain.Tier_User", b =>
                {
                    b.HasOne("motel.Models.Domain.Tiers", "tiers")
                        .WithMany("tier_user")
                        .HasForeignKey("tierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("motel.Models.Domain.User", "user")
                        .WithOne("users_tier")
                        .HasForeignKey("motel.Models.Domain.Tier_User", "userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("tiers");

                    b.Navigation("user");
                });

            modelBuilder.Entity("motel.Models.Domain.User", b =>
                {
                    b.HasOne("motel.Models.Domain.Role", "role")
                        .WithMany("user")
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("role");
                });

            modelBuilder.Entity("motel.Models.Domain.Category", b =>
                {
                    b.Navigation("post_category");
                });

            modelBuilder.Entity("motel.Models.Domain.Post", b =>
                {
                    b.Navigation("post_category");

                    b.Navigation("post_manage");
                });

            modelBuilder.Entity("motel.Models.Domain.Role", b =>
                {
                    b.Navigation("user");
                });

            modelBuilder.Entity("motel.Models.Domain.Tiers", b =>
                {
                    b.Navigation("tier_user");
                });

            modelBuilder.Entity("motel.Models.Domain.User", b =>
                {
                    b.Navigation("post");

                    b.Navigation("post_manage");

                    b.Navigation("users_tier")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
