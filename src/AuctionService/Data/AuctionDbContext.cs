using System;
using AuctionService.Entites;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Auction> Auctions { get; set; } // creating table for the auctions

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxStateEntity();
    }

}
