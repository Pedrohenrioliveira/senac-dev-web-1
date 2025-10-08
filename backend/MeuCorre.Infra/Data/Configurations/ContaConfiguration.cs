using MeuCorre.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuCorre.Infra.Data.Configurations
{
    public class ContaConfiguration : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("Contas");

            // Chave primária
            builder.HasKey(c => c.Id);

            // Campos obrigatórios
            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Tipo)
                .IsRequired();

            builder.Property(c => c.Saldo)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(c => c.UsuarioId)
                .IsRequired();

            builder.Property(c => c.Ativo)
                .IsRequired();

            builder.Property(c => c.DataCriacao)
                .IsRequired();

            // Campos opcionais
            builder.Property(c => c.Limite)
                .HasColumnType("decimal(10,2)");

            builder.Property(c => c.DiaFechamento);

            builder.Property(c => c.DiaVencimento);

            builder.Property(c => c.Cor)
                .HasMaxLength(7);

            builder.Property(c => c.Icone)
                .HasMaxLength(20);

            builder.Property(c => c.TipoLimite)
                .HasMaxLength(20);

            builder.Property(c => c.DataAtualizacao);

            // Relacionamento com Usuario
            builder.HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(c => c.UsuarioId);
            builder.HasIndex(c => c.Tipo);
            builder.HasIndex(c => c.Ativo);
        }
    }
}
