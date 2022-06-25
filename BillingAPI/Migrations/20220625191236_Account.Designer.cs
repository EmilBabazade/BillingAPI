﻿// <auto-generated />
using System;
using BillingAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BillingAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220625191236_Account")]
    partial class Account
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.6");

            modelBuilder.Entity("BillingAPI.Entities.Balance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("PaymentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("BillingAPI.Entities.Gateway", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("No")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("No")
                        .IsUnique();

                    b.ToTable("Gateways");
                });

            modelBuilder.Entity("BillingAPI.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("GatewayId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("No")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PayableAmount")
                        .HasColumnType("TEXT");

                    b.Property<int>("PaymentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GatewayId");

                    b.HasIndex("No")
                        .IsUnique();

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BillingAPI.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("GatewayId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSuccessfull")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GatewayId");

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("BillingAPI.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BillingAPI.Entities.Balance", b =>
                {
                    b.HasOne("BillingAPI.Entities.Payment", "Payment")
                        .WithOne("Balance")
                        .HasForeignKey("BillingAPI.Entities.Balance", "PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingAPI.Entities.User", "User")
                        .WithMany("Balances")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BillingAPI.Entities.Order", b =>
                {
                    b.HasOne("BillingAPI.Entities.Gateway", "Gateway")
                        .WithMany("Orders")
                        .HasForeignKey("GatewayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingAPI.Entities.Payment", "Payment")
                        .WithOne("Order")
                        .HasForeignKey("BillingAPI.Entities.Order", "PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingAPI.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gateway");

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BillingAPI.Entities.Payment", b =>
                {
                    b.HasOne("BillingAPI.Entities.Gateway", "Gateway")
                        .WithMany("Payments")
                        .HasForeignKey("GatewayId");

                    b.HasOne("BillingAPI.Entities.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gateway");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BillingAPI.Entities.Gateway", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("BillingAPI.Entities.Payment", b =>
                {
                    b.Navigation("Balance");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("BillingAPI.Entities.User", b =>
                {
                    b.Navigation("Balances");

                    b.Navigation("Orders");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
