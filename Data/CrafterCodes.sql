Create database AppWebTorneo;
-- Crear la tabla Roles
use AppWebTorneo;
CREATE TABLE Roles (
    idRol INT PRIMARY KEY,
    nombreRol VARCHAR(50) NOT NULL
);

-- Insertar algunos roles de ejemplo
INSERT INTO Roles (idRol, nombreRol) VALUES
(1, 'Administrador'),
(2, 'Coach'),
(3, 'Contador'),
(4, 'Arbitro');

-- Crear la tabla Personal
CREATE TABLE Usuarios (
    idUsuarios INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellidoPaterno VARCHAR(50) NOT NULL,
    apellidoMaterno VARCHAR(50) NOT NULL,
    idRol INT,
    correo VARCHAR(100) NOT NULL,
    contraseña VARCHAR(100) NOT NULL,
    CONSTRAINT FK_Personal_Roles FOREIGN KEY (idRol) REFERENCES Roles(idRol)
);

-- Insertar algunos registros de ejemplo en la tabla Personal
INSERT INTO Usuarios (idUsuarios, nombre, apellidoPaterno, apellidoMaterno, idRol, correo, contraseña) VALUES
(1, 'Damaso', 'Damazo', 'Ramirez', 1, 'iCr4zySpace85@example.com', '123456789'),
(2, 'Jonathan Isrrael', 'Caballero', 'Morales', 2, 'Goku@example.com', '123456789'),
(3, 'Victor Abraham', 'Sedeño', 'Gonzalez', 3, 'BerkSed@example.com', '123456789'),
(4, 'Jose Antonio', 'Cordero', 'Daniel', 4, 'JackCord@example.com', '123456789');

CREATE PROCEDURE SP_Login
    @correo VARCHAR(100),
    @contraseña VARCHAR(100)
AS
BEGIN
    SELECT
        U.idPersonal,
        U.nombre,
        U.apellidoPaterno,
        U.apellidoMaterno,
        U.idRol,
        R.nombreRol AS nombreRol,
        U.correo
    FROM
        Usuarios U
    JOIN
        Roles R ON U.idRol = R.idRol
    WHERE
        U.correo = @correo AND U.contraseña = @contraseña;
END;