using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Context
{
    public class AppDemonstracaoContext : DbContext
    {
        public AppDemonstracaoContext(DbContextOptions options) : base (options) 
        {
        }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Endereco> Enderecos { get; set; }

        public DbSet<Fornecedor> Fornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Caso alguma Property do tipo string não tenha sido mapeada, será definida como varchar(100)
            foreach (var property in modelBuilder
                .Model
                .GetEntityTypes()
                .SelectMany(
                    e => e.GetProperties()
                        .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            // Aplica todas as configurações da Fluent API, configuradas em Mappings
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDemonstracaoContext).Assembly);

            // Desabilita o Cascade Delete Behavior
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) 
                    relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}
