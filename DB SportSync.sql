CREATE DATABASE SportSync

USE SportSync
use master

DROP DATABASE SportSync

/*************************************************************/
/* Control de Usuarios  */

CREATE TABLE Usuarios (
    ID_usuario INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50),
    Ap_paterno NVARCHAR(50),
    Ap_materno NVARCHAR(50),
    Correo NVARCHAR(100) UNIQUE,
    Pass NVARCHAR(MAX),
    Fecha_creacion DATETIME
);

CREATE TABLE Roles (
    ID_rol INT PRIMARY KEY IDENTITY(1,1),
    Nombre_rol NVARCHAR(50),
    Descripción NVARCHAR(255)
);

CREATE TABLE Permisos (
    ID_permiso INT PRIMARY KEY IDENTITY(1,1),
    Nombre_permiso NVARCHAR(50),
    Descripción NVARCHAR(255)
);

CREATE TABLE Roles_Permisos (
    ID_rol_permiso INT PRIMARY KEY IDENTITY(1,1),
    ID_rol INT,
    ID_permiso INT,
    FOREIGN KEY (ID_rol) REFERENCES Roles(ID_rol),
    FOREIGN KEY (ID_permiso) REFERENCES Permisos(ID_permiso)
);

CREATE TABLE Usuarios_Roles (
    ID_usuario_rol INT PRIMARY KEY IDENTITY(1,1),
    ID_usuario INT,
    ID_rol INT,
    FOREIGN KEY (ID_usuario) REFERENCES Usuarios(ID_usuario),
    FOREIGN KEY (ID_rol) REFERENCES Roles(ID_rol)
);

/*************************************************************/
/* Equipos, Torneos y Jugadores  */

CREATE TABLE Deportes (
    ID_deporte INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100)
);

CREATE TABLE Torneos (
    ID_torneo INT PRIMARY KEY IDENTITY(1,1),
    IMG_torneo VARBINARY(MAX), -- Almacenamiento de imagen en la base de datos
    Nombre NVARCHAR(100),
    ID_deporte INT,
    Categoria NVARCHAR(50),
    Fecha_inicio DATETIME,
    Fecha_fin DATETIME,
    FOREIGN KEY (ID_deporte) REFERENCES Deportes(ID_deporte)
);

CREATE TABLE Equipos (
    ID_equipo INT PRIMARY KEY IDENTITY(1,1),
    IMG_equipo VARBINARY(MAX), -- Almacenamiento de imagen en la base de datos
    Nombre NVARCHAR(100),
    ID_deporte INT,
    Categoria NVARCHAR(50),
    FOREIGN KEY (ID_deporte) REFERENCES Deportes(ID_deporte)
);

CREATE TABLE Jugadores (
    ID_jugador INT PRIMARY KEY IDENTITY(1,1),
	IMG_jugador VARBINARY(MAX),
    Nombre NVARCHAR(100),
    ID_equipo INT,
    Posición NVARCHAR(50),
    Edad INT,
    Numero INT,
    Descripcion NVARCHAR(255),
    FOREIGN KEY (ID_equipo) REFERENCES Equipos(ID_equipo)
);

CREATE TABLE Equipos_Torneos (
    ID_equipo_torneo INT PRIMARY KEY IDENTITY(1,1),
    ID_equipo INT,
    ID_torneo INT,
    FOREIGN KEY (ID_equipo) REFERENCES Equipos(ID_equipo),
    FOREIGN KEY (ID_torneo) REFERENCES Torneos(ID_torneo)
);


/*************************************************************/
/* Partidos y Fases del Torneo  */

-- Creación de la tabla Partidos
CREATE TABLE Partidos (
    ID_partido INT PRIMARY KEY IDENTITY(1,1),
    ID_torneo INT,
    Fecha DATE,
	Hora TIME,
    Ubicacion NVARCHAR(255),
    ID_equipo_local INT,
    ID_equipo_visitante INT,
    ID_organizador INT,
    Estado NVARCHAR(50),
    FOREIGN KEY (ID_torneo) REFERENCES Torneos(ID_torneo),
    FOREIGN KEY (ID_equipo_local) REFERENCES Equipos(ID_equipo),
    FOREIGN KEY (ID_equipo_visitante) REFERENCES Equipos(ID_equipo),
    FOREIGN KEY (ID_organizador) REFERENCES Usuarios(ID_usuario),
);


-- Creación de la tabla Fases
CREATE TABLE Fases (
    ID_fase INT PRIMARY KEY IDENTITY(1,1),
    ID_torneo INT,
    Nombre_fase NVARCHAR(255),
    Tipo_fase NVARCHAR(50),
    Fecha_inicio DATETIME,
    Fecha_fin DATETIME,
    FOREIGN KEY (ID_torneo) REFERENCES Torneos(ID_torneo)
);

-- Creación de la tabla Partidos_Fase
CREATE TABLE Partidos_Fase (
    ID_partido_fase INT PRIMARY KEY IDENTITY(1,1),
    ID_partido INT,
    ID_fase INT,
    FOREIGN KEY (ID_partido) REFERENCES Partidos(ID_partido),
    FOREIGN KEY (ID_fase) REFERENCES Fases(ID_fase)
);

/*************************************************************/
/* Arbitraje  */

-- Creación de la tabla Asignaciones_Arbitraje
CREATE TABLE Asignaciones_Arbitraje (
    ID_asignacion INT PRIMARY KEY IDENTITY(1,1),
    ID_partido INT,
    ID_arbitro INT,
    Fecha_asignacion DATETIME,
    FOREIGN KEY (ID_partido) REFERENCES Partidos(ID_partido),
    FOREIGN KEY (ID_arbitro) REFERENCES Usuarios(ID_usuario)
);

-- Creación de la tabla Evaluaciones_Arbitraje
CREATE TABLE Evaluaciones_Arbitraje (
    ID_evaluacion INT PRIMARY KEY IDENTITY(1,1),
    ID_asignacion INT,
	ID_equipo_ganador INT,
    ID_equipo_perdedor INT,
	Resultado_equipo_ganador INT,
    Resultado_equipo_perdedor INT,
    FOREIGN KEY (ID_asignacion) REFERENCES Asignaciones_Arbitraje(ID_asignacion),
	FOREIGN KEY (ID_equipo_ganador) REFERENCES Equipos(ID_equipo),
    FOREIGN KEY (ID_equipo_perdedor) REFERENCES Equipos(ID_equipo)
);

/*************************************************************/
/* Noticias  */

-- Creación de la tabla Noticias
CREATE TABLE Noticias (
    ID_noticia INT PRIMARY KEY IDENTITY(1,1),
    Titulo NVARCHAR(255),
    Contenido NVARCHAR(MAX), -- NVARCHAR(MAX) permite almacenar contenido extenso
    Fecha_publicacion DATETIME,
    ID_autor INT,
    FOREIGN KEY (ID_autor) REFERENCES Usuarios(ID_usuario)
);

-- Creación de la tabla Noticias_Equipos
CREATE TABLE Noticias_Equipos (
    ID_noticia_equipo INT PRIMARY KEY IDENTITY(1,1),
    ID_noticia INT,
    ID_equipo INT,
    FOREIGN KEY (ID_noticia) REFERENCES Noticias(ID_noticia),
    FOREIGN KEY (ID_equipo) REFERENCES Equipos(ID_equipo)
);

-- Creación de la tabla Noticias_Torneos
CREATE TABLE Noticias_Torneos (
    ID_noticia_torneo INT PRIMARY KEY IDENTITY(1,1),
    ID_noticia INT,
    ID_torneo INT,
    FOREIGN KEY (ID_noticia) REFERENCES Noticias(ID_noticia),
    FOREIGN KEY (ID_torneo) REFERENCES Torneos(ID_torneo)
);

/*************************************************************/
/* Finanzas  */

-- Creación de la tabla Patrocinadores
CREATE TABLE Patrocinadores (
    ID_patrocinador INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(255),
    Descripcion NVARCHAR(MAX),
    Concepto NVARCHAR(255),
    Monto DECIMAL(10, 2), -- Ajusta la precisión según sea necesario
    Tipo_apoyo NVARCHAR(255),
    Telefono NVARCHAR(20),
    ID_torneo INT,
    Fecha_creacion DATE,
	ID_contador INT
    FOREIGN KEY (ID_torneo) REFERENCES Torneos(ID_torneo),
	FOREIGN KEY (ID_contador) REFERENCES Usuarios(ID_usuario)
);

-- Creación de la tabla Gastos
CREATE TABLE Gastos (
    ID_gasto INT PRIMARY KEY IDENTITY(1,1),
    ID_torneo INT,
    Concepto NVARCHAR(255),
    Monto DECIMAL(10, 2), -- Ajusta la precisión según sea necesario
    Fecha DATE,
	ID_contador INT
    FOREIGN KEY (ID_torneo) REFERENCES Torneos(ID_torneo),
	FOREIGN KEY (ID_contador) REFERENCES Usuarios(ID_usuario)
);

-- Creación de la tabla Inscripciones
CREATE TABLE Inscripciones (
    ID_inscripcion INT PRIMARY KEY IDENTITY(1,1),
    ID_torneo INT,
    ID_equipo INT,
    Estado NVARCHAR(50),
    Fecha DATE,
	ID_contador INT
    FOREIGN KEY (ID_torneo) REFERENCES Torneos(ID_torneo),
    FOREIGN KEY (ID_equipo) REFERENCES Equipos(ID_equipo),
	FOREIGN KEY (ID_contador) REFERENCES Usuarios(ID_usuario)
);

-- Creación de la tabla Arbitraje
CREATE TABLE Arbitraje (
    ID_arbitraje INT PRIMARY KEY IDENTITY(1,1),
    ID_torneo INT,
    ID_partido INT,
    ID_partido_fase INT,
    ID_equipo INT,
    Cantidad DECIMAL(10, 2),
    Estado NVARCHAR(50),
	ID_contador INT
	FOREIGN KEY (ID_torneo) REFERENCES Torneos(ID_torneo),
    FOREIGN KEY (ID_partido) REFERENCES Partidos(ID_partido),
    FOREIGN KEY (ID_partido_fase) REFERENCES Partidos_Fase(ID_partido_fase),
    FOREIGN KEY (ID_equipo) REFERENCES Equipos(ID_equipo),
	FOREIGN KEY (ID_contador) REFERENCES Usuarios(ID_usuario)
);

/*************************************************************/
/* Resultados Finales  */

CREATE TABLE Resultados_Torneo (
    ID_resultado INT PRIMARY KEY IDENTITY(1,1),
    ID_torneo INT,
    ID_equipo_ganador INT,
    ID_equipo_subcampeon INT,
    ID_equipo_tercero INT,
    Fecha_resultado DATE,
    FOREIGN KEY (ID_torneo) REFERENCES Torneos(ID_torneo),
    FOREIGN KEY (ID_equipo_ganador) REFERENCES Equipos(ID_equipo),
    FOREIGN KEY (ID_equipo_subcampeon) REFERENCES Equipos(ID_equipo),
    FOREIGN KEY (ID_equipo_tercero) REFERENCES Equipos(ID_equipo)
);