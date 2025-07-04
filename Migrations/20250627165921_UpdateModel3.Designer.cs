﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend.Data;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(HotelDbContext))]
    [Migration("20250627165921_UpdateModel3")]
    partial class UpdateModel3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("backend.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Adults")
                        .HasColumnType("int");

                    b.Property<string>("CardExpMonth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardExpYear")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CheckIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckOut")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuestFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuestLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<int>("Kids")
                        .HasColumnType("int");

                    b.Property<decimal>("NightlyRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReservationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("Rooms")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalCharge")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Bookings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Adults = 2,
                            CardExpMonth = "12",
                            CardExpYear = "2025",
                            CardFirstName = "John",
                            CardLastName = "Doe",
                            CardNumber = "4111111111111111",
                            CardType = "Visa",
                            CheckIn = new DateTime(2025, 6, 27, 0, 0, 0, 0, DateTimeKind.Local),
                            CheckOut = new DateTime(2025, 6, 29, 0, 0, 0, 0, DateTimeKind.Local),
                            City = "Metropolis",
                            Country = "USA",
                            Email = "john@example.com",
                            GuestFirstName = "John",
                            GuestLastName = "Doe",
                            IsPaid = true,
                            Kids = 0,
                            NightlyRate = 120m,
                            Phone = "1234567890",
                            PostalCode = "12345",
                            ReservationNumber = "ABC123",
                            RoomId = 1,
                            Rooms = 1,
                            Street = "456 Elm St",
                            TotalCharge = 240m
                        });
                });

            modelBuilder.Entity("backend.Models.Hotel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StarRating")
                        .HasColumnType("int");

                    b.Property<int>("TotalRooms")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Hotels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Main St",
                            MainImageUrl = "hotel1.jpg",
                            Name = "Sample Hotel",
                            StarRating = 4,
                            TotalRooms = 2
                        });
                });

            modelBuilder.Entity("backend.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.PrimitiveCollection<string>("Amenities")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HotelId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RackRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            HotelId = 1,
                            ImageUrl = "room1.jpg",
                            Price = 120m,
                            RackRate = 150m,
                            Title = "Deluxe Room"
                        },
                        new
                        {
                            Id = 2,
                            HotelId = 1,
                            ImageUrl = "room2.jpg",
                            Price = 200m,
                            RackRate = 250m,
                            Title = "Suite"
                        });
                });

            modelBuilder.Entity("backend.Models.Booking", b =>
                {
                    b.HasOne("backend.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("backend.Models.Room", b =>
                {
                    b.HasOne("backend.Models.Hotel", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("backend.Models.Hotel", b =>
                {
                    b.Navigation("Rooms");
                });
#pragma warning restore 612, 618
        }
    }
}
