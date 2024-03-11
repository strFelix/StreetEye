IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [semaforos] (
    [Id] int NOT NULL IDENTITY,
    [Descricao] nvarchar(10) NOT NULL,
    [IntervaloAberto] int NOT NULL,
    [IntervaloFechado] int NOT NULL,
    [Endereco] nvarchar(40) NOT NULL,
    [Numero] nvarchar(10) NOT NULL,
    [ViaCruzamento] nvarchar(40) NOT NULL,
    [Latitude] nvarchar(20) NOT NULL,
    [Longitude] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_semaforos] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [utilizadores] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(50) NOT NULL,
    [Tipo] nvarchar(max) NOT NULL,
    [DataNascimento] date NOT NULL,
    [Telefone] nvarchar(20) NOT NULL,
    [Endereco] nvarchar(40) NOT NULL,
    [NumeroEndereco] nvarchar(10) NOT NULL,
    [Complemento] nvarchar(20) NOT NULL,
    [Bairro] nvarchar(25) NOT NULL,
    [Cidade] nvarchar(25) NOT NULL,
    [UF] char(2) NOT NULL,
    [CEP] char(9) NOT NULL,
    [Latitude] nvarchar(20) NOT NULL,
    [Longitude] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_utilizadores] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [status_semaforo] (
    [IdSemaforo] int NOT NULL,
    [Momento] datetime2 NOT NULL,
    [StatusVisor] bit NOT NULL,
    [StatusAudio] bit NOT NULL,
    [Estado] bit NOT NULL,
    CONSTRAINT [PK_status_semaforo] PRIMARY KEY ([IdSemaforo], [Momento]),
    CONSTRAINT [FK_status_semaforo_semaforos_IdSemaforo] FOREIGN KEY ([IdSemaforo]) REFERENCES [semaforos] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [responsaveis] (
    [IdUtilizador] int NOT NULL,
    [IdResponsavel] int NOT NULL,
    CONSTRAINT [PK_responsaveis] PRIMARY KEY ([IdUtilizador], [IdResponsavel]),
    CONSTRAINT [FK_responsaveis_utilizadores_IdUtilizador] FOREIGN KEY ([IdUtilizador]) REFERENCES [utilizadores] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [usuarios] (
    [Id] int NOT NULL IDENTITY,
    [IdUtilizador] int NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [PasswordHash] varbinary(255) NULL,
    [PasswordSalt] varbinary(255) NULL,
    CONSTRAINT [PK_usuarios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_usuarios_utilizadores_IdUtilizador] FOREIGN KEY ([IdUtilizador]) REFERENCES [utilizadores] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [historico_usuarios] (
    [IdUsuario] int NOT NULL,
    [Momento] datetime2 NOT NULL,
    [IdSemaforo] int NOT NULL,
    [Latitude] nvarchar(20) NOT NULL,
    [Longitude] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_historico_usuarios] PRIMARY KEY ([IdUsuario], [Momento]),
    CONSTRAINT [FK_historico_usuarios_semaforos_IdSemaforo] FOREIGN KEY ([IdSemaforo]) REFERENCES [semaforos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_historico_usuarios_usuarios_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [usuarios] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [usuario_imagem] (
    [IdUsuario] int NOT NULL,
    [Imagem] varbinary(max) NULL,
    CONSTRAINT [PK_usuario_imagem] PRIMARY KEY ([IdUsuario]),
    CONSTRAINT [FK_usuario_imagem_usuarios_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [usuarios] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_historico_usuarios_IdSemaforo] ON [historico_usuarios] ([IdSemaforo]);
GO

CREATE INDEX [IX_usuarios_IdUtilizador] ON [usuarios] ([IdUtilizador]);
GO

CREATE INDEX [IX_utilizadores_Nome] ON [utilizadores] ([Nome]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240307185726_initialCreate', N'8.0.2');
GO

COMMIT;
GO

