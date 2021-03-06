﻿// <auto-generated />
using FlowrouteApi.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlowrouteApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200225185917_initial database")]
    partial class initialdatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FlowrouteApi.DataModels.IncomingRoute", b =>
                {
                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Phone", "Email");

                    b.ToTable("IncomingRoute");
                });

            modelBuilder.Entity("FlowrouteApi.DataModels.OutgoingRoute", b =>
                {
                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Phone", "Email");

                    b.ToTable("OutgoingRoute");
                });
#pragma warning restore 612, 618
        }
    }
}
