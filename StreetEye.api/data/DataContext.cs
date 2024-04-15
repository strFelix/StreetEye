using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StreetEye.models;
using StreetEye.models.enums;


namespace StreetEye.data;
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

        #region Utilizador
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

        modelBuilder.Entity<Utilizador>()
            .HasMany(u => u.Usuarios)
            .WithOne(u => u.Utilizador)
            .HasForeignKey(u => u.IdUtilizador);
        #endregion

        #region Usuario
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
            .Ignore(u => u.Password);

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
        #endregion

        #region Historico Usuarios
        modelBuilder.Entity<HistoricoUsuario>()
            .ToTable("historico_usuarios");

        modelBuilder.Entity<HistoricoUsuario>()
            .HasKey(hu => new { hu.IdUsuario, hu.Momento });

        modelBuilder.Entity<HistoricoUsuario>()
            .Property(hu => hu.Latitude)
            .HasMaxLength(20);

        modelBuilder.Entity<HistoricoUsuario>()
            .Property(hu => hu.Longitude)
            .HasMaxLength(20);

        // foreign de historicoUsuario para semaforo
        modelBuilder.Entity<HistoricoUsuario>()
            .HasOne(hu => hu.Semaforo)
            .WithMany(s => s.Historicos)
            .HasForeignKey(hu => hu.IdSemaforo);
        #endregion

        #region Semaforos
        Semaforo semaforo = new Semaforo
        {
            Id = 1,
            Descricao = "Principal",
            IntervaloAberto = 20,
            IntervaloFechado = 40,
            Endereco = "Rua Alcantara",
            Numero = "113",
            ViaCruzamento = "Guilherme Cotching",
            Latitude = "-23.519502072656618", 
            Longitude = "-46.59639509306988"
        };
        modelBuilder.Entity<Semaforo>().HasData(semaforo);

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
        #endregion

        #region Status Semaforo
        modelBuilder.Entity<StatusSemaforo>()
            .ToTable("status_semaforo");

        modelBuilder.Entity<StatusSemaforo>()
            .HasKey(ss => new { ss.IdSemaforo, ss.Momento });
        #endregion

        #region Responsavel
        modelBuilder.Entity<Responsavel>()
            .ToTable("responsaveis");

        modelBuilder.Entity<Responsavel>()
            .HasKey(r => new { r.IdUtilizador, r.IdResponsavel });

        //foreign de utilizadores para responsaveis
        modelBuilder.Entity<Responsavel>()
            .HasOne<Utilizador>(r => r.Utilizador)
            .WithMany(u => u.Responsaveis)
            .HasForeignKey(r => r.IdUtilizador)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Responsavel>()
            .HasOne<Utilizador>(r => r.ResponsavelUtilizador)
            .WithMany()
            .HasForeignKey(r => r.IdResponsavel)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
        #endregion

        #region usuario Imagem
        modelBuilder.Entity<UsuarioImagem>()
                .ToTable("usuario_imagem");

        modelBuilder.Entity<UsuarioImagem>()
            .HasKey(ui => ui.IdUsuario);

        modelBuilder.Entity<UsuarioImagem>()
            .Property(ui => ui.Imagem)
            .HasColumnType("varbinary(max)");

        // foreign de usuarioImagem para usuarios
        modelBuilder.Entity<UsuarioImagem>()
          .HasOne(ui => ui.Usuario)
          .WithOne(u => u.UsuarioImagem)
          .HasForeignKey<UsuarioImagem>(ui => ui.IdUsuario);
        #endregion

    }
}