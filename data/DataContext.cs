using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StreetEye.models;
using StreetEye.models.enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace StreetEye.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<HistoricoUsuario> HistoricoUsuarios { get; set; }
        public DbSet<Semaforo> Semaforos { get; set; }
        public DbSet<StatusSemaforo> StatusSemaforos { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }
        public DbSet<UsuarioImagem> UsuariosImagem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ** VERIFICAR FOREIGN KEYS **

            // utilizadores
            {
                modelBuilder.Entity<Utilizador>()
                .ToTable("utilizadores");

                modelBuilder.Entity<Utilizador>()
                    .HasKey(u => u.Id);
                
                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Id)
                    .ValueGeneratedOnAdd();
                
                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Nome)
                    .HasMaxLength(50);
                
                modelBuilder.Entity<Utilizador>()
                    .HasIndex(u => u.Nome);

                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Tipo)
                    .HasConversion(new EnumToStringConverter<TipoUtilizador>()); //converte o enum em string);
                
                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Endereco)
                    .HasMaxLength(40);
                
                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.NumeroEndereco)
                    .HasMaxLength(10);

                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Telefone)
                    .HasMaxLength(20);

                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Complemento)
                    .HasMaxLength(20);

                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Bairro)
                    .HasMaxLength(25);
                
                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Cidade)
                    .HasMaxLength(25);
                
                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.UF)
                    .HasColumnType("char(2)");

                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.CEP)
                    .HasColumnType("char(9)");
                
                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Latitude)
                    .HasMaxLength(20);

                modelBuilder.Entity<Utilizador>()
                    .Property(u => u.Longitude)
                    .HasMaxLength(20);
            }

            // usuarios
            {
                modelBuilder.Entity<Usuario>()
                    .ToTable("usuarios");

                modelBuilder.Entity<Usuario>()
                    .HasKey(u => u.Id);

                modelBuilder.Entity<Usuario>()
                    .Property(u => u.Id)
                    .ValueGeneratedOnAdd();

                modelBuilder.Entity<Usuario>()
                    .Property(u => u.Id);

                modelBuilder.Entity<Usuario>()
                    .Property(u => u.Email)
                    .HasMaxLength(50);

                modelBuilder.Entity<Usuario>()
                    .Property(u => u.PasswordHash)
                    .HasMaxLength(255);
                
                modelBuilder.Entity<Usuario>()
                    .Property(u => u.PasswordSalt)
                    .HasMaxLength(255);

                // foreign de historicos para usuarios
                modelBuilder.Entity<Usuario>()
                    .HasMany(u => u.Historicos)
                    .WithOne(u => u.Usuario)
                    .HasForeignKey(u => u.IdUsuario);
            }
        
            // historico_usuarios
            {
                modelBuilder.Entity<HistoricoUsuario>()
                    .ToTable("historico_usuarios");
                
                modelBuilder.Entity<HistoricoUsuario>()
                    .HasKey(hu => new {hu.IdUsuario, hu.Momento});

                modelBuilder.Entity<HistoricoUsuario>()
                    .Property(hu => hu.Latitude)
                    .HasMaxLength(20);

                modelBuilder.Entity<HistoricoUsuario>()
                    .Property(hu => hu.Longitude)
                    .HasMaxLength(20);
            }

            // semaforos 
            {
                modelBuilder.Entity<Semaforo>()
                    .ToTable("semaforos");
                
                modelBuilder.Entity<Semaforo>()
                    .HasKey(s => s.Id);
                
                modelBuilder.Entity<Semaforo>()
                    .Property(s => s.Id)
                    .ValueGeneratedOnAdd();
                
                modelBuilder.Entity<Semaforo>()
                    .Property(s => s.Descricao)
                    .HasMaxLength(10);

                modelBuilder.Entity<Semaforo>()
                    .Property(s => s.Endereco)
                    .HasMaxLength(40);

                modelBuilder.Entity<Semaforo>()
                    .Property(s => s.Numero)
                    .HasMaxLength(10);

                modelBuilder.Entity<Semaforo>()
                    .Property(s => s.ViaCruzamento)
                    .HasMaxLength(40);
                
                modelBuilder.Entity<Semaforo>()
                    .Property(s => s.Latitude)
                    .HasMaxLength(20);
                
                modelBuilder.Entity<Semaforo>()
                    .Property(s => s.Longitude)
                    .HasMaxLength(20);

                // foreign de status para semaforo
                modelBuilder.Entity<Semaforo>()
                    .HasMany(s => s.Status)
                    .WithOne(s => s.Semaforo)
                    .HasForeignKey(s => s.IdSemaforo);
            }
       
            // status_semaforos
            {
                modelBuilder.Entity<StatusSemaforo>()
                    .ToTable("status_semaforo");
                
                modelBuilder.Entity<StatusSemaforo>()
                    .HasKey(ss => new {ss.IdSemaforo, ss.Momento});

            }
        
            // responsaveis
            {
                modelBuilder.Entity<Responsavel>()
                    .ToTable("responsaveis");

                modelBuilder.Entity<Responsavel>()
                    .HasKey(r => new {r.IdUtilizador, r.IdResponsavel});
            }

            // usuarioImagem
            {
                modelBuilder.Entity<UsuarioImagem>()
                    .ToTable("usuario_imagem");

                modelBuilder.Entity<UsuarioImagem>()
                    .HasKey(ui => ui.IdUsuario);
                
                modelBuilder.Entity<UsuarioImagem>()
                    .Property(ui => ui.Imagem)
                    .HasColumnType("varbinary(max)");
            }
        }
    }
}