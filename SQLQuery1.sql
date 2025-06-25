create database e_learning
use e_learning

create table Docentes(
Docente_ID int identity primary key not null,
Nombre varchar(50) not null,
Apellido varchar(50) not null,
CorreoElectronico varchar(100) not null unique,
FechaRegistro date not null
)

create table Cursos(
Curso_ID int identity primary key not null,
Titulo varchar(50) not null,
Descripcion varchar(max) not null,
DuracionEstimada int,
NotaMinima decimal(5,2) not null,
Docente_ID int not null,
foreign key(Docente_ID) references Docentes(Docente_ID)
)

create table Estudiantes(
Estudiante_ID int identity primary key not null,
Nombre varchar(50) not null,
Apellido varchar(50) not null,
CorreoElectronico varchar(100) not null unique,
FechaRegistro DateTime not null
)

create table Modulos(
Modulo_ID int identity primary key not null,
Titulo varchar(50) not null,
Descripcion varchar(max) not null,
Orden int not null,
Puntos int not null,
Curso_ID int not null,
foreign key (Curso_ID) references Cursos(Curso_ID)
)

create table Clases(
Clase_ID int identity primary key not null,
Titulo varchar(100) not null,
Contenido varchar(max) not null,
Puntos int not null,
Modulo_ID int not null,
foreign key (Modulo_ID) references Modulos(Modulo_ID)
)

create table Inscripciones(
Inscripcion_ID int identity primary key not null,
FechaInscripcion date not null,
Progreso decimal(5,2) not null DEFAULT (0.00),
CalificacionFinal decimal(5,2) not null DEFAULT (0.00),
Estado varchar(20) not null,
Estudiante_ID int not null,
Curso_ID int not null,
foreign key (Estudiante_ID) references Estudiantes(Estudiante_ID),
foreign key (Curso_ID) references Cursos(Curso_ID),
CONSTRAINT Inscripciones_Estado CHECK (Estado IN ('En curso', 'Aprobado', 'Reprobado')),
CONSTRAINT Inscripciones_EstudianteCurso UNIQUE (Estudiante_ID, Curso_ID)
)

create table NotaClases(
NotaClase_ID int identity primary key not null,
PuntosObtenidos int not null,
Inscripcion_ID int not null,
Clase_ID int not null,
foreign key (Inscripcion_ID) references Inscripciones(Inscripcion_ID),
foreign key (Clase_ID) references Clases(Clase_ID)
)

create table Certificaciones(
Certificacion_ID int identity primary key not null,
FechaEmision date not null,
NumeroCertificado varchar(50) not null unique,
Inscripcion_ID int not null,
foreign key (Inscripcion_ID) references Inscripciones(Inscripcion_ID)
)

create table BitacoraAuditoria(
Bitacora_ID int identity primary key not null,
Usuario varchar(100) not null,
FechaHora DateTime not null,
Operacion varchar(10) not null,
Tabla varchar(50) not null,
Datos varchar(max)
)

INSERT INTO Docentes (Nombre, Apellido, CorreoElectronico, FechaRegistro) VALUES
('Laura', 'González', 'laura.gonzalez@ejemplo.com', '2024-01-10'),
('Carlos', 'Ramírez', 'carlos.ramirez@ejemplo.com', '2024-01-12'),
('Ana', 'Martínez', 'ana.martinez@ejemplo.com', '2024-01-15'),
('Jorge', 'Pérez', 'jorge.perez@ejemplo.com', '2024-01-18');

INSERT INTO Cursos (Titulo, Descripcion,  DuracionEstimada, NotaMinima, Docente_ID) VALUES
('Programación Básica', 'Curso introductorio de programación', 40, 70.00, 1),
('Bases de Datos', 'Fundamentos de bases de datos relacionales',  50, 75.00, 2),
('Desarrollo Web', 'Curso de diseño y desarrollo web',  60, 70.00, 3),
('Lógica Computacional', 'Razonamiento lógico aplicado a la computación', 70, 65.00, 4);

INSERT INTO Estudiantes (Nombre, Apellido, CorreoElectronico, FechaRegistro) VALUES
('Mateo', 'Faccio', 'mateofacciopereyra@gmail.com', '2024-02-01'),
('Paolo', 'Lemos', 'paololemos@gmail.com', '2024-02-03'),
('Sofía', 'Torres', 'sofia.torres@gmail.com', '2024-02-05'),
('Pedro', 'Luna', 'pedro.luna@gmail.com', '2024-02-07');

INSERT INTO Modulos (Titulo, Descripcion, Orden, Puntos, Curso_ID) VALUES
('Variables y Tipos', 'Introducción a variables y tipos de datos', 1, 10, 1),
('Consultas SQL', 'Consultas básicas con SELECT', 1, 15, 2),
('HTML y CSS', 'Diseño web básico', 1, 20, 3),
('Proposiciones', 'Lógica proposicional', 1, 10, 4);

INSERT INTO Clases (Titulo, Contenido, Puntos, Modulo_ID) VALUES
('Tipos de Datos', 'Contenido sobre tipos de datos', 5, 1),
('SELECT Básico', 'Uso básico de SELECT en SQL', 7, 2),
('Etiquetas HTML', 'Introducción a etiquetas HTML', 10, 3),
('Tabla de Verdad', 'Construcción de tablas de verdad', 5, 4);

INSERT INTO Inscripciones (FechaInscripcion, Progreso, CalificacionFinal, Estado, Estudiante_ID, Curso_ID) VALUES
('2024-03-01', '50.00', 75.00, 'En curso', 1, 1),
('2024-03-02', '70.00', 85.00, 'En curso', 2, 2),
('2024-03-03', '100.00', 95.00, 'Aprobado', 3, 3),
('2024-03-04', '40.00',0.00 ,'En curso', 4, 4);

INSERT INTO NotaClases (PuntosObtenidos, Inscripcion_ID, Clase_ID) VALUES
(5, 8, 1),
(7, 9, 2),
(10, 10, 3),
(4, 11, 4);


select * from Inscripciones
INSERT INTO Certificaciones (FechaEmision, NumeroCertificado, Inscripcion_ID) VALUES
('2024-05-01', 'CERT-20240501-001', 10);



CREATE FUNCTION Fn_CalcularCalificacionFinal
(
    @EstudianteID INT,
    @CursoID INT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @PuntosTotales INT;
    DECLARE @PuntosObtenidos INT;
    DECLARE @CalificacionFinal DECIMAL(5,2);
    DECLARE @InscripcionID INT;

    SELECT @InscripcionID = Inscripcion_ID
    FROM Inscripciones
    WHERE Estudiante_ID = @EstudianteID AND Curso_ID = @CursoID;

    SELECT @PuntosTotales = SUM(C.Puntos)
    FROM Clases C
    JOIN Modulos M ON C.Modulo_ID = M.Modulo_ID
    WHERE M.Curso_ID = @CursoID;

    SELECT @PuntosObtenidos = SUM(NC.PuntosObtenidos)
    FROM NotaClases NC
    JOIN Clases C ON NC.Clase_ID = C.Clase_ID
    JOIN Modulos M ON C.Modulo_ID = M.Modulo_ID
    WHERE NC.Inscripcion_ID = @InscripcionID;

    IF @PuntosTotales IS NULL OR @PuntosTotales = 0
        SET @CalificacionFinal = 0.00;
    ELSE
        SET @CalificacionFinal = CAST(@PuntosObtenidos AS DECIMAL(5,2)) * 100.0 / @PuntosTotales;

    RETURN @CalificacionFinal;
END;




CREATE PROCEDURE CalcularEstadoYActualizarInscripcion
    @EstudianteID INT,
    @CursoID INT
AS
BEGIN
    DECLARE @InscripcionID INT;
    DECLARE @CalificacionFinal DECIMAL(5,2);
    DECLARE @NotaMinima DECIMAL(5,2);
    DECLARE @NuevoEstado VARCHAR(20);
    DECLARE @ClasesTotales INT;
    DECLARE @ClasesCompletadas INT;
    DECLARE @Progreso DECIMAL(5,2);

    SELECT @InscripcionID = Inscripcion_ID
    FROM Inscripciones
    WHERE Estudiante_ID = @EstudianteID AND Curso_ID = @CursoID;

    -- Usar función
    SET @CalificacionFinal = dbo.Fn_CalcularCalificacionFinal(@EstudianteID, @CursoID);

    SELECT @NotaMinima = NotaMinima
    FROM Cursos
    WHERE Curso_ID = @CursoID;

    SELECT @ClasesTotales = COUNT(C.Clase_ID)
    FROM Clases C
    JOIN Modulos M ON C.Modulo_ID = M.Modulo_ID
    WHERE M.Curso_ID = @CursoID;

    SELECT @ClasesCompletadas = COUNT(DISTINCT NC.Clase_ID)
    FROM NotaClases NC
    JOIN Clases C ON NC.Clase_ID = C.Clase_ID
    JOIN Modulos M ON C.Modulo_ID = M.Modulo_ID
    WHERE M.Curso_ID = @CursoID AND NC.Inscripcion_ID = @InscripcionID;

    IF @ClasesTotales IS NULL OR @ClasesTotales = 0
        SET @Progreso = 0.00;
    ELSE
        SET @Progreso = CAST(@ClasesCompletadas AS DECIMAL(5,2)) * 100.0 / @ClasesTotales;

    IF @ClasesCompletadas < @ClasesTotales
        SET @NuevoEstado = 'En curso';
    ELSE IF @CalificacionFinal >= @NotaMinima
        SET @NuevoEstado = 'Aprobado';
    ELSE
        SET @NuevoEstado = 'Reprobado';

    UPDATE Inscripciones
    SET CalificacionFinal = @CalificacionFinal,
        Estado = @NuevoEstado,
        Progreso = @Progreso
    WHERE Inscripcion_ID = @InscripcionID;

    SELECT 
        @CalificacionFinal AS CalificacionFinal,
        @NuevoEstado AS EstadoFinal,
        @Progreso AS ProgresoFinal;
END;