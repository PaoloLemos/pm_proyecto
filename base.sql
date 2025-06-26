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
	RolID int NOT NULL
	Foreign key(RolId)REFERENCES Roles(RolId) 
);


create table Roles
(
RolId int identity(1,1) primary key,
Nombre nvarchar(50) not null unique 
)

create table Permisos (
PermisoId INT IDENTITY(1,1) PRIMARY KEY,
Nombre nvarchar(100) not null unique 
)

create table RolesPermisos
(
RolId int NOT NULL,
PermisoId int NOT NULL,
rolPermisoId int Identity(1,1) primary key
foreign key (RolId) REFERENCES Roles(RolId),
foreign key (PermisoId) REFERENCES Permisos(PermisoId)

)




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

--necesitamos ejecutar esto antes si no lo tenemos
Insert Into Roles(Nombre)
values
('Administrador'),
('Editor'),
('Cliente')



-- tabla necesaria para que el programa tengo ususarios , el mas importante el admin, la contraseña es 0000
INSERT INTO Usuarios (Nombre, Email, Contrasena, RolID)
VALUES 
  ('Cliente Uno',    'cliente1@ejemplo.com', 'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 3),
  ('Cliente Dos',    'cliente2@ejemplo.com', 'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 3),
  ('Cliente Tres',   'cliente3@ejemplo.com', 'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 3),
  ('Cliente Cuatro', 'cliente4@ejemplo.com', 'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 3),
  ('Editor Maestro', 'editor@ejemplo.com',    'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 2),
  ('Paolo',  'admin@gmail.com',  'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 1);






  --creamos permisos , puede que aun falten
INSERT INTO Permisos (Nombre) VALUES
  ('Modificar Clientes'),
  ('Ver Clientes'),
  ('Modificar Usuarios'),
  ('Ver Usuarios'),
  ('Modificar Noticias'),
  ('Ver Noticias'),
  ('Modificar Patrocinadores'),
  ('Ver Patrocinadores'),
  ('Modificar Programas'),
  ('Ver Programas');




  --Le damos todos los permisos al admin
  INSERT INTO RolesPermisos (RolID, PermisoID)
SELECT
  1 AS RolID,
  PermisoID
FROM Permisos;



--para ver si anduvo lo anterior
SELECT RP.rolPermisoId, R.Nombre AS Rol, P.Nombre AS Permiso
FROM RolesPermisos RP
JOIN Roles    R ON RP.RolID     = R.RolID
JOIN Permisos P ON RP.PermisoID = P.PermisoID
WHERE RP.RolID = 1;





-- le damos permisos al editor, si no los tiene ejecutamos lo siguiente
 INSERT INTO RolesPermisos (RolID, PermisoID)
values 
(2,5),
(2,6),
(2,9),
(2,10)




select * from Roles

select * from Permisos
