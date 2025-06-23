CREATE DATABASE Proyecto_BD;
GO

USE Proyecto_BD;
GO

-- 1. EFMigrationsHistory
CREATE TABLE __EFMigrationsHistory (
    MigrationId NVARCHAR(150) NOT NULL PRIMARY KEY,
    ProductVersion NVARCHAR(32) NOT NULL
);

-- 2. Tablas base
CREATE TABLE Direccion (
    ID_Direccion INT PRIMARY KEY IDENTITY,
    Calle NVARCHAR(100) NOT NULL,
    Numero NVARCHAR(10) NOT NULL,
    Colonia NVARCHAR(50) NOT NULL,
    CodigoPostal NVARCHAR(10) NOT NULL
);

CREATE TABLE Consultorio (
    ID_Consultorio INT PRIMARY KEY IDENTITY,
    Piso NVARCHAR(50) NOT NULL,
    Numero_Consultorio NVARCHAR(50) NOT NULL
);

CREATE TABLE Especialidad (
    ID_Especialidad INT PRIMARY KEY IDENTITY,
    Descripcion NVARCHAR(100) NOT NULL,
    PrecioCita REAL NOT NULL
);

CREATE TABLE EstatusCita (
    ID_Estatus_Cita INT PRIMARY KEY IDENTITY,
    Estatus_Cita NVARCHAR(50) NOT NULL
);

CREATE TABLE Jornada (
    ID_Jornada INT PRIMARY KEY IDENTITY,
    Hora_Entrada TIME NOT NULL,
    Hora_Salida TIME NOT NULL,
    Descripcion NVARCHAR(MAX) NOT NULL DEFAULT ''
);

CREATE TABLE CitasHorario (
    ID_Horario INT PRIMARY KEY IDENTITY,
    Hora_Cita TIME NOT NULL,
    JornadaHorario BIT NOT NULL DEFAULT 0
);

CREATE TABLE Medicina (
    ID_Medicina INT PRIMARY KEY IDENTITY,
    Cantidad INT NOT NULL,
    Precio_Medicina REAL NOT NULL,
    Nombre_Medicina NVARCHAR(100) NOT NULL
);

CREATE TABLE Servicio (
    ID_Servicio INT PRIMARY KEY IDENTITY,
    Descripcion NVARCHAR(100) NOT NULL,
    Precio_Servicio REAL NOT NULL
);

CREATE TABLE TipoSangre (
    ID_Tipo_Sangre INT PRIMARY KEY IDENTITY,
    Tipo_Sangre NVARCHAR(10) NOT NULL
);

CREATE TABLE TiposUsuario (
    ID_Tipo_Usuario INT PRIMARY KEY IDENTITY,
    Tipo_Usuario NVARCHAR(50) NOT NULL
);

-- 3. Usuario y entidades dependientes
CREATE TABLE Usuario (
    ID_Usuario INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(50) NOT NULL,
    Apellido_Paterno NVARCHAR(50) NOT NULL,
    Apellido_Materno NVARCHAR(50) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    CURP NVARCHAR(18) NOT NULL,
    Fecha_Nacimiento DATETIME2 NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Fecha_Registro DATETIME2 NOT NULL,
    ID_Tipo_Usuario INT NOT NULL,
    Estado_Usuario BIT NOT NULL,
    FOREIGN KEY (ID_Tipo_Usuario) REFERENCES TiposUsuario(ID_Tipo_Usuario)
);

CREATE TABLE Medico (
    ID_Medico INT PRIMARY KEY IDENTITY,
    ID_Usuario INT NOT NULL,
    Cedula NVARCHAR(50) NOT NULL,
    ID_Especialidad INT NOT NULL,
    ID_Consultorio INT NOT NULL,
    ID_Jornada INT NOT NULL,
    FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario),
    FOREIGN KEY (ID_Especialidad) REFERENCES Especialidad(ID_Especialidad),
    FOREIGN KEY (ID_Consultorio) REFERENCES Consultorio(ID_Consultorio),
    FOREIGN KEY (ID_Jornada) REFERENCES Jornada(ID_Jornada)
);

CREATE TABLE Paciente (
    ID_Paciente INT PRIMARY KEY IDENTITY,
    ID_Usuario INT NOT NULL,
    ID_Tipo_Sangre INT NOT NULL,
    Peso REAL NOT NULL,
    Alergia NVARCHAR(MAX) NOT NULL,
    Estatura REAL NOT NULL,
    FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario),
    FOREIGN KEY (ID_Tipo_Sangre) REFERENCES TipoSangre(ID_Tipo_Sangre)
);

CREATE TABLE Recepcionista (
    ID_Recepcionista INT PRIMARY KEY IDENTITY,
    ID_Usuario INT NOT NULL,
    ID_Jornada INT NOT NULL,
    FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario),
    FOREIGN KEY (ID_Jornada) REFERENCES Jornada(ID_Jornada)
);

-- 4. Citas y relaciones clínicas
CREATE TABLE Cita (
    ID_Cita INT PRIMARY KEY IDENTITY,
    ID_Paciente INT NOT NULL,
    ID_Medico INT NOT NULL,
    Fecha_Registro DATETIME2 NOT NULL,
    Fecha_Cita DATETIME2 NOT NULL,
    ID_Cita_Horario INT NOT NULL,
    ID_Estatus_Cita INT NOT NULL,
    FOREIGN KEY (ID_Paciente) REFERENCES Paciente(ID_Paciente),
    FOREIGN KEY (ID_Medico) REFERENCES Medico(ID_Medico),
    FOREIGN KEY (ID_Estatus_Cita) REFERENCES EstatusCita(ID_Estatus_Cita),
    FOREIGN KEY (ID_Cita_Horario) REFERENCES CitasHorario(ID_Horario)
);

CREATE TABLE Venta (
    ID_Ventas INT PRIMARY KEY IDENTITY,
    Fecha_Venta DATETIME2 NOT NULL,
    ID_Recepcionista INT NOT NULL,
    FOREIGN KEY (ID_Recepcionista) REFERENCES Recepcionista(ID_Recepcionista)
);

CREATE TABLE Pago (
    ID_Pago INT PRIMARY KEY IDENTITY,
    ID_Cita INT NOT NULL,
    Estado_Pago BIT NOT NULL,
    ComprobantePago NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (ID_Cita) REFERENCES Cita(ID_Cita)
);

CREATE TABLE Receta (
    ID_Receta INT PRIMARY KEY IDENTITY,
    ID_Cita INT NOT NULL,
    Diagnostico NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (ID_Cita) REFERENCES Cita(ID_Cita)
);

CREATE TABLE Ticket (
    ID_Ticket INT PRIMARY KEY IDENTITY,
    ID_Venta INT NOT NULL,
    Tipo_Item BIT NOT NULL,
    ID_Item INT NOT NULL,
    Cantidad INT NOT NULL DEFAULT 0,
    Subtotal REAL NOT NULL DEFAULT 0,
    FOREIGN KEY (ID_Venta) REFERENCES Venta(ID_Ventas)
);

CREATE TABLE Tratamiento (
    ID_Tratamiento INT PRIMARY KEY IDENTITY,
    ID_Receta INT NOT NULL,
    Medicamento NVARCHAR(MAX) NOT NULL,
    Indicaciones NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (ID_Receta) REFERENCES Receta(ID_Receta)
);

-- 5. Índices
CREATE INDEX IX_Cita_Estatus ON Cita(ID_Estatus_Cita);
CREATE INDEX IX_Cita_Medico ON Cita(ID_Medico);
CREATE INDEX IX_Cita_Paciente ON Cita(ID_Paciente);
CREATE INDEX IX_Cita_Horario ON Cita(ID_Cita_Horario);
CREATE INDEX IX_Ticket_Venta ON Ticket(ID_Venta);
CREATE INDEX IX_Usuario_Tipo ON Usuario(ID_Tipo_Usuario);
CREATE INDEX IX_Medico_Consultorio ON Medico(ID_Consultorio);
CREATE INDEX IX_Medico_Especialidad ON Medico(ID_Especialidad);
CREATE INDEX IX_Medico_Jornada ON Medico(ID_Jornada);
CREATE INDEX IX_Paciente_TipoSangre ON Paciente(ID_Tipo_Sangre);
CREATE INDEX IX_Receta_Cita ON Receta(ID_Cita);
CREATE INDEX IX_Tratamiento_Receta ON Tratamiento(ID_Receta);
CREATE INDEX IX_Pago_Cita ON Pago(ID_Cita);
CREATE INDEX IX_Venta_Recepcionista ON Venta(ID_Recepcionista);