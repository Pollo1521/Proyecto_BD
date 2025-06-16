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
CREATE TABLE [Consultorio] (
    [ID_Consultorio] int NOT NULL IDENTITY,
    [Piso] nvarchar(max) NOT NULL,
    [Numero_Consultorio] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Consultorio] PRIMARY KEY ([ID_Consultorio])
);

CREATE TABLE [Especialidad] (
    [ID_Especialidad] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(max) NOT NULL,
    [PrecioCita] real NOT NULL,
    CONSTRAINT [PK_Especialidad] PRIMARY KEY ([ID_Especialidad])
);

CREATE TABLE [EstatusCita] (
    [ID_Estatus_Cita] int NOT NULL IDENTITY,
    [Estatus_Cita] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_EstatusCita] PRIMARY KEY ([ID_Estatus_Cita])
);

CREATE TABLE [Jornada] (
    [ID_Jornada] int NOT NULL IDENTITY,
    [Hora_Entrada] datetime2 NOT NULL,
    [Hora_Salida] datetime2 NOT NULL,
    CONSTRAINT [PK_Jornada] PRIMARY KEY ([ID_Jornada])
);

CREATE TABLE [Medicina] (
    [ID_Medicina] int NOT NULL IDENTITY,
    [Cantidad] int NOT NULL,
    [Precio_Medicina] real NOT NULL,
    [Nombre_Medicina] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Medicina] PRIMARY KEY ([ID_Medicina])
);

CREATE TABLE [Servicio] (
    [ID_Servicio] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(max) NOT NULL,
    [Precio_Servicio] real NOT NULL,
    CONSTRAINT [PK_Servicio] PRIMARY KEY ([ID_Servicio])
);

CREATE TABLE [TipoSangre] (
    [ID_Tipo_Sangre] int NOT NULL IDENTITY,
    [Tipo_Sangre] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_TipoSangre] PRIMARY KEY ([ID_Tipo_Sangre])
);

CREATE TABLE [TiposUsuario] (
    [ID_Tipo_Usuario] int NOT NULL IDENTITY,
    [Tipo_Usuario] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_TiposUsuario] PRIMARY KEY ([ID_Tipo_Usuario])
);

CREATE TABLE [Usuario] (
    [ID_Usuario] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [Apellido_Paterno] nvarchar(max) NOT NULL,
    [Apellido_Materno] nvarchar(max) NOT NULL,
    [Correo] nvarchar(max) NOT NULL,
    [CURP] nvarchar(18) NOT NULL,
    [Fecha_Nacimiento] datetime2 NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Fecha_Registro] datetime2 NOT NULL,
    [ID_Tipo_Usuario] int NOT NULL,
    [Estado_Usuario] bit NOT NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY ([ID_Usuario]),
    CONSTRAINT [FK_Usuario_TiposUsuario_ID_Tipo_Usuario] FOREIGN KEY ([ID_Tipo_Usuario]) REFERENCES [TiposUsuario] ([ID_Tipo_Usuario]) ON DELETE NO ACTION
);

CREATE TABLE [Medico] (
    [ID_Medico] int NOT NULL IDENTITY,
    [ID_Usuario] int NOT NULL,
    [Cedula] nvarchar(max) NOT NULL,
    [ID_Especialidad] int NOT NULL,
    [ID_Consultorio] int NOT NULL,
    [ID_Jornada] int NOT NULL,
    CONSTRAINT [PK_Medico] PRIMARY KEY ([ID_Medico]),
    CONSTRAINT [FK_Medico_Consultorio_ID_Consultorio] FOREIGN KEY ([ID_Consultorio]) REFERENCES [Consultorio] ([ID_Consultorio]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Medico_Especialidad_ID_Especialidad] FOREIGN KEY ([ID_Especialidad]) REFERENCES [Especialidad] ([ID_Especialidad]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Medico_Jornada_ID_Jornada] FOREIGN KEY ([ID_Jornada]) REFERENCES [Jornada] ([ID_Jornada]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Medico_Usuario_ID_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [Usuario] ([ID_Usuario]) ON DELETE NO ACTION
);

CREATE TABLE [Paciente] (
    [ID_Paciente] int NOT NULL IDENTITY,
    [ID_Usuario] int NOT NULL,
    [ID_Tipo_Sangre] int NOT NULL,
    [Peso] real NOT NULL,
    [Alergia] nvarchar(max) NOT NULL,
    [Estatura] real NOT NULL,
    CONSTRAINT [PK_Paciente] PRIMARY KEY ([ID_Paciente]),
    CONSTRAINT [FK_Paciente_TipoSangre_ID_Tipo_Sangre] FOREIGN KEY ([ID_Tipo_Sangre]) REFERENCES [TipoSangre] ([ID_Tipo_Sangre]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Paciente_Usuario_ID_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [Usuario] ([ID_Usuario]) ON DELETE NO ACTION
);

CREATE TABLE [Recepcionista] (
    [ID_Recepcionista] int NOT NULL IDENTITY,
    [ID_Usuario] int NOT NULL,
    [ID_Jornada] int NOT NULL,
    CONSTRAINT [PK_Recepcionista] PRIMARY KEY ([ID_Recepcionista]),
    CONSTRAINT [FK_Recepcionista_Jornada_ID_Jornada] FOREIGN KEY ([ID_Jornada]) REFERENCES [Jornada] ([ID_Jornada]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Recepcionista_Usuario_ID_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [Usuario] ([ID_Usuario]) ON DELETE NO ACTION
);

CREATE TABLE [Cita] (
    [ID_Cita] int NOT NULL IDENTITY,
    [ID_Paciente] int NOT NULL,
    [ID_Medico] int NOT NULL,
    [Fecha_Registro] datetime2 NOT NULL,
    [Fecha_Cita] datetime2 NOT NULL,
    [Hora_Cita] datetime2 NOT NULL,
    [ID_Estatus_Cita] int NOT NULL,
    CONSTRAINT [PK_Cita] PRIMARY KEY ([ID_Cita]),
    CONSTRAINT [FK_Cita_EstatusCita_ID_Estatus_Cita] FOREIGN KEY ([ID_Estatus_Cita]) REFERENCES [EstatusCita] ([ID_Estatus_Cita]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Cita_Medico_ID_Medico] FOREIGN KEY ([ID_Medico]) REFERENCES [Medico] ([ID_Medico]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Cita_Paciente_ID_Paciente] FOREIGN KEY ([ID_Paciente]) REFERENCES [Paciente] ([ID_Paciente]) ON DELETE NO ACTION
);

CREATE TABLE [Venta] (
    [ID_Ventas] int NOT NULL IDENTITY,
    [Fecha_Venta] datetime2 NOT NULL,
    [ID_Recepcionista] int NOT NULL,
    CONSTRAINT [PK_Venta] PRIMARY KEY ([ID_Ventas]),
    CONSTRAINT [FK_Venta_Recepcionista_ID_Recepcionista] FOREIGN KEY ([ID_Recepcionista]) REFERENCES [Recepcionista] ([ID_Recepcionista]) ON DELETE NO ACTION
);

CREATE TABLE [Pago] (
    [ID_Pago] int NOT NULL IDENTITY,
    [ID_Cita] int NOT NULL,
    [Estado_Pago] bit NOT NULL,
    [ComprobantePago] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Pago] PRIMARY KEY ([ID_Pago]),
    CONSTRAINT [FK_Pago_Cita_ID_Cita] FOREIGN KEY ([ID_Cita]) REFERENCES [Cita] ([ID_Cita]) ON DELETE NO ACTION
);

CREATE TABLE [Receta] (
    [ID_Receta] int NOT NULL IDENTITY,
    [ID_Cita] int NOT NULL,
    [Diagnostico] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Receta] PRIMARY KEY ([ID_Receta]),
    CONSTRAINT [FK_Receta_Cita_ID_Cita] FOREIGN KEY ([ID_Cita]) REFERENCES [Cita] ([ID_Cita]) ON DELETE NO ACTION
);

CREATE TABLE [Ticket] (
    [ID_Ticket] int NOT NULL IDENTITY,
    [ID_Venta] int NOT NULL,
    [ID_Medicina] int NOT NULL,
    [ID_Servicio] int NOT NULL,
    CONSTRAINT [PK_Ticket] PRIMARY KEY ([ID_Ticket]),
    CONSTRAINT [FK_Ticket_Medicina_ID_Medicina] FOREIGN KEY ([ID_Medicina]) REFERENCES [Medicina] ([ID_Medicina]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Ticket_Servicio_ID_Servicio] FOREIGN KEY ([ID_Servicio]) REFERENCES [Servicio] ([ID_Servicio]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Ticket_Venta_ID_Venta] FOREIGN KEY ([ID_Venta]) REFERENCES [Venta] ([ID_Ventas]) ON DELETE NO ACTION
);

CREATE TABLE [Tratamiento] (
    [ID_Tratamiento] int NOT NULL IDENTITY,
    [ID_Receta] int NOT NULL,
    [Medicamento] nvarchar(max) NOT NULL,
    [Indicaciones] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Tratamiento] PRIMARY KEY ([ID_Tratamiento]),
    CONSTRAINT [FK_Tratamiento_Receta_ID_Receta] FOREIGN KEY ([ID_Receta]) REFERENCES [Receta] ([ID_Receta]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Cita_ID_Estatus_Cita] ON [Cita] ([ID_Estatus_Cita]);

CREATE INDEX [IX_Cita_ID_Medico] ON [Cita] ([ID_Medico]);

CREATE INDEX [IX_Cita_ID_Paciente] ON [Cita] ([ID_Paciente]);

CREATE INDEX [IX_Medico_ID_Consultorio] ON [Medico] ([ID_Consultorio]);

CREATE INDEX [IX_Medico_ID_Especialidad] ON [Medico] ([ID_Especialidad]);

CREATE INDEX [IX_Medico_ID_Jornada] ON [Medico] ([ID_Jornada]);

CREATE INDEX [IX_Medico_ID_Usuario] ON [Medico] ([ID_Usuario]);

CREATE INDEX [IX_Paciente_ID_Tipo_Sangre] ON [Paciente] ([ID_Tipo_Sangre]);

CREATE INDEX [IX_Paciente_ID_Usuario] ON [Paciente] ([ID_Usuario]);

CREATE INDEX [IX_Pago_ID_Cita] ON [Pago] ([ID_Cita]);

CREATE INDEX [IX_Recepcionista_ID_Jornada] ON [Recepcionista] ([ID_Jornada]);

CREATE INDEX [IX_Recepcionista_ID_Usuario] ON [Recepcionista] ([ID_Usuario]);

CREATE INDEX [IX_Receta_ID_Cita] ON [Receta] ([ID_Cita]);

CREATE INDEX [IX_Ticket_ID_Medicina] ON [Ticket] ([ID_Medicina]);

CREATE INDEX [IX_Ticket_ID_Servicio] ON [Ticket] ([ID_Servicio]);

CREATE INDEX [IX_Ticket_ID_Venta] ON [Ticket] ([ID_Venta]);

CREATE INDEX [IX_Tratamiento_ID_Receta] ON [Tratamiento] ([ID_Receta]);

CREATE INDEX [IX_Usuario_ID_Tipo_Usuario] ON [Usuario] ([ID_Tipo_Usuario]);

CREATE INDEX [IX_Venta_ID_Recepcionista] ON [Venta] ([ID_Recepcionista]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250425050425_initial', N'9.0.6');

CREATE TABLE [Ticket] (
    [ID_Ticket] int NOT NULL IDENTITY,
    [ID_Venta] int NOT NULL,
    [Tipo_item] bit NOT NULL,
    [ID_Item] int NOT NULL,
    CONSTRAINT [PK_Ticket] PRIMARY KEY ([ID_Ticket]),
    CONSTRAINT [FK_Ticket_Venta_ID_Venta] FOREIGN KEY ([ID_Venta]) REFERENCES [Venta] ([ID_Ventas]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Ticket_ID_Venta] ON [Ticket] ([ID_Venta]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250505040435_tickets', N'9.0.6');

ALTER TABLE [Jornada] ADD [descripcion] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250505230323_Jornadas', N'9.0.6');

ALTER TABLE [Ticket] ADD [Cantidad] int NOT NULL DEFAULT 0;

ALTER TABLE [Ticket] ADD [Subtotal] real NOT NULL DEFAULT CAST(0 AS real);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250602063010_tickets2', N'9.0.6');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cita]') AND [c].[name] = N'Hora_Cita');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Cita] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Cita] DROP COLUMN [Hora_Cita];

ALTER TABLE [Cita] ADD [ID_Cita_Horario] int NOT NULL DEFAULT 0;

CREATE TABLE [CitasHorario] (
    [ID_Horario] int NOT NULL IDENTITY,
    [Hora_Cita] time NOT NULL,
    CONSTRAINT [PK_CitasHorario] PRIMARY KEY ([ID_Horario])
);

CREATE INDEX [IX_Cita_ID_Cita_Horario] ON [Cita] ([ID_Cita_Horario]);

ALTER TABLE [Cita] ADD CONSTRAINT [FK_Cita_CitasHorario_ID_Cita_Horario] FOREIGN KEY ([ID_Cita_Horario]) REFERENCES [CitasHorario] ([ID_Horario]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250609092048_horarios', N'9.0.6');

ALTER TABLE [CitasHorario] ADD [JornadaHorario] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250615042802_horarios2', N'9.0.6');

COMMIT;
GO

