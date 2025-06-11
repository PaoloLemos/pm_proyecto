Create Database VozDelEste

CREATE TABLE Clientes (
    CI VARCHAR(20) NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL
);

CREATE TABLE Clima (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL,
    Temperatura DECIMAL(5,2),
    Descripcion NVARCHAR(100),
    Icono NVARCHAR(50)
);

CREATE TABLE ClimaPronostico (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaHora DATETIME NOT NULL,
    Temperatura DECIMAL(5,2),
    Descripcion NVARCHAR(100),
    Icono NVARCHAR(50)
);

CREATE TABLE Programas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Imagen NVARCHAR(255),
    Descripcion NVARCHAR(MAX)
);

CREATE TABLE Comentarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteCI VARCHAR(20),
    ProgramaId INT,
    Comentario NVARCHAR(MAX),
    Fecha DATETIME NOT NULL,
    FOREIGN KEY (ClienteCI) REFERENCES Clientes(CI),
    FOREIGN KEY (ProgramaId) REFERENCES Programas(Id)
);

CREATE TABLE Conductores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProgramaId INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Bio NVARCHAR(MAX),
    FOREIGN KEY (ProgramaId) REFERENCES Programas(Id)
);

CREATE TABLE Cotizaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL,
    TipoMoneda NVARCHAR(3),
    Valor DECIMAL(18,4) NOT NULL,
    CHECK (TipoMoneda IN ('USD', 'EUR', 'BRL'))
);

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Contrasena NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(20) NOT NULL,
    CHECK (Rol IN ('cliente', 'editor', 'administrador'))
);

CREATE TABLE LogsAcciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT,
    Fecha DATETIME NOT NULL,
    Accion NVARCHAR(100),
    Entidad NVARCHAR(50),
    EntidadId INT,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

CREATE TABLE Noticias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Contenido NVARCHAR(MAX),
    FechaPublicacion DATE NOT NULL,
    Imagen NVARCHAR(255)
);

CREATE TABLE Patrocinadores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    [Plan] INT NOT NULL
);

CREATE TABLE ProgramacionHoraria (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProgramaId INT NOT NULL,
    DiaSemana NVARCHAR(20) NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    FOREIGN KEY (ProgramaId) REFERENCES Programas(Id)
);
