using System;
using Microsoft.EntityFrameworkCore;
using BlockchainDemo.Entities.Models;

namespace BlockchainDemo.Data
{
    public class BlockContext : DbContext
    {
        public BlockContext(DbContextOptions<BlockContext> options)
               : base(options)
        {
        }

        public DbSet<Block> BlockItems { get; set; }
    }
}
