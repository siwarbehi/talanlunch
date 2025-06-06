﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TalanLunch.Infrastructure.Data;

#nullable disable

namespace TalanLunch.Infrastructure.Migrations
{
    [DbContext(typeof(TalanLunchDbContext))]
    [Migration("20250405234758_AddMenuDishManyToManyyy")]
    partial class AddMenuDishManyToManyyy
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TalanLunch.Domain.Entities.Dish", b =>
                {
                    b.Property<int>("DishId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DishId"));

                    b.Property<int>("CurrentRating")
                        .HasColumnType("int");

                    b.Property<string>("DishDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DishName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DishPhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DishPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("DishQuantity")
                        .HasColumnType("int");

                    b.Property<bool>("IsSalad")
                        .HasColumnType("bit");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReviewCount")
                        .HasColumnType("int");

                    b.HasKey("DishId");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.DishRating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RatingId"));

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("RatingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RatingId");

                    b.HasIndex("UserId");

                    b.ToTable("DishRatings");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.Menu", b =>
                {
                    b.Property<int>("MenuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MenuId"));

                    b.Property<DateTime>("MenuDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MenuDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MenuId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.MenuDish", b =>
                {
                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<int>("DishId")
                        .HasColumnType("int");

                    b.HasKey("MenuId", "DishId");

                    b.HasIndex("DishId");

                    b.ToTable("MenuDishes");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderRemark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Paid")
                        .HasColumnType("bit");

                    b.Property<bool>("Served")
                        .HasColumnType("bit");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.OrderDish", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("DishId")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "DishId");

                    b.HasIndex("DishId");

                    b.ToTable("OrderDishes");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResetTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.DishRating", b =>
                {
                    b.HasOne("TalanLunch.Domain.Entities.User", "User")
                        .WithMany("DishRatings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.MenuDish", b =>
                {
                    b.HasOne("TalanLunch.Domain.Entities.Dish", "Dish")
                        .WithMany("MenuDishes")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TalanLunch.Domain.Entities.Menu", "Menu")
                        .WithMany("MenuDishes")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.Order", b =>
                {
                    b.HasOne("TalanLunch.Domain.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.OrderDish", b =>
                {
                    b.HasOne("TalanLunch.Domain.Entities.Dish", "Dish")
                        .WithMany("OrderDishes")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TalanLunch.Domain.Entities.Order", "Order")
                        .WithMany("OrderDishes")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.Dish", b =>
                {
                    b.Navigation("MenuDishes");

                    b.Navigation("OrderDishes");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.Menu", b =>
                {
                    b.Navigation("MenuDishes");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.Order", b =>
                {
                    b.Navigation("OrderDishes");
                });

            modelBuilder.Entity("TalanLunch.Domain.Entities.User", b =>
                {
                    b.Navigation("DishRatings");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
