using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using apbdkol2.Models;

namespace apbdkol2.Data;

public partial class Apbdkol2Context : DbContext
{
    public Apbdkol2Context()
    {
    }

    public Apbdkol2Context(DbContextOptions<Apbdkol2Context> options)
        : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Backpack> Backpacks { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Title> Titles { get; set; }
    public DbSet<CharacterTitle> CharacterTitles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "Item1", Weight = 10 },
            new Item { Id = 2, Name = "Item2", Weight = 11 },
            new Item { Id = 3, Name = "Item3", Weight = 12 },
            new Item { Id = 4, Name = "Item4", Weight = 100 }
        );

        modelBuilder.Entity<Character>().HasData(
            new Character { Id = 1, FirstName = "John", LastName = "Yakuza", CurrentWeight = 43, MaxWeight = 200 },
            new Character { Id = 2, FirstName = "Chun", LastName = "Li", CurrentWeight = 30, MaxWeight = 100 }
        );

        modelBuilder.Entity<Title>().HasData(
            new Title { Id = 1, Name = "Title1" },
            new Title { Id = 2, Name = "Title2" },
            new Title { Id = 3, Name = "Title3" }
        );

        modelBuilder.Entity<Backpack>().HasData(
            new Backpack { CharacterId = 1, ItemId = 1, Amount = 1 },
            new Backpack { CharacterId = 1, ItemId = 2, Amount = 2 },
            new Backpack { CharacterId = 2, ItemId = 3, Amount = 6 }
        );

        modelBuilder.Entity<CharacterTitle>().HasData(
            new CharacterTitle { CharacterId = 1, TitlesId = 1, AcquiredAt = new DateTime(2024, 6, 10) },
            new CharacterTitle { CharacterId = 1, TitlesId = 2, AcquiredAt = new DateTime(2024, 6, 9) }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}