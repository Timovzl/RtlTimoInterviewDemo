﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RtlTimo.InterviewDemo.Infrastructure.Databases;

#nullable disable

namespace RtlTimo.InterviewDemo.Infrastructure.Databases.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20231227173039_ShowAndPersonAndAppearance")]
    partial class ShowAndPersonAndAppearance
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("Latin1_General_100_BIN2")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RtlTimo.InterviewDemo.Domain.Persons.Appearance", b =>
                {
                    b.Property<long>("PersonId")
                        .HasColumnType("bigint");

                    b.Property<long>("ShowId")
                        .HasColumnType("bigint");

                    b.HasKey("PersonId", "ShowId");

                    b.HasIndex("ShowId", "PersonId");

                    b.ToTable("Appearances");
                });

            modelBuilder.Entity("RtlTimo.InterviewDemo.Domain.Persons.Person", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<DateTime>("ModificationDateTime")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .UseCollation("Latin1_General_100_CI_AS");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("ModificationDateTime");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("RtlTimo.InterviewDemo.Domain.Productions.Show", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModificationDateTime")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .UseCollation("Latin1_General_100_CI_AS");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("ModificationDateTime");

                    b.ToTable("Shows");
                });

            modelBuilder.Entity("RtlTimo.InterviewDemo.Domain.Persons.Appearance", b =>
                {
                    b.HasOne("RtlTimo.InterviewDemo.Domain.Persons.Person", null)
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RtlTimo.InterviewDemo.Domain.Productions.Show", null)
                        .WithMany()
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
