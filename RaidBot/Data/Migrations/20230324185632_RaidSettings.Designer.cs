﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RaidBot.Data;

#nullable disable

namespace RaidBot.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230324185632_RaidSettings")]
    partial class RaidSettings
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("DSharpPlus.Entities.DiscordUser", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvatarHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("BannerHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Flags")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsBot")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsSystem")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Locale")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("MfaEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OAuthFlags")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PremiumType")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RaidSettingsId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Verified")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RaidSettingsId");

                    b.ToTable("DiscordUser");
                });

            modelBuilder.Entity("RaidBot.entities.GuildSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("RaidChannelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GuildSettings");
                });

            modelBuilder.Entity("RaidBot.entities.RaidSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("Dps")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Healer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Info")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<ulong?>("RaidLeaderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Tank")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Tier")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RaidLeaderId");

                    b.ToTable("RaidSettings");
                });

            modelBuilder.Entity("DSharpPlus.Entities.DiscordUser", b =>
                {
                    b.HasOne("RaidBot.entities.RaidSettings", null)
                        .WithMany("member")
                        .HasForeignKey("RaidSettingsId");
                });

            modelBuilder.Entity("RaidBot.entities.RaidSettings", b =>
                {
                    b.HasOne("DSharpPlus.Entities.DiscordUser", "RaidLeader")
                        .WithMany()
                        .HasForeignKey("RaidLeaderId");

                    b.Navigation("RaidLeader");
                });

            modelBuilder.Entity("RaidBot.entities.RaidSettings", b =>
                {
                    b.Navigation("member");
                });
#pragma warning restore 612, 618
        }
    }
}
