--VISTA DE PACIENTES REGISTRADOS ACTIVOS
Create View Vista_Pacientes as
Select (u.Nombre + ' ' + u.Apellido_Paterno + ' ' + u.Apellido_Materno) as Nombre, u.Correo
from Usuario u
where u.ID_Tipo_Usuario = 2 AND u.Estado_Usuario = 1;

--VISTA DEL INVENTARIO DE FARMACIA
Create view Farmacia_Inventario as
Select m.Nombre_Medicina as Medicamento,
m.Cantidad as 'Unidades Disponibles'
from Medicina m;

--VISTA MEDICOS CONTACTOS
Create View Contacto_Medicos as
select e.Descripcion, (u.Nombre + ' ' + u.Apellido_Paterno + ' ' + u.Apellido_Materno) as Nombre, u.Correo, e.PrecioCita
from Usuario u
join Medico m on u.ID_Usuario = m.ID_Usuario
join Especialidad e on e.ID_Especialidad = m.ID_Especialidad
where u.ID_Tipo_Usuario = 3 AND u.Estado_Usuario = 1;

--VISTA CITAS PENDIENTES
Create view Citas_Pendientes as
select ID_Cita as Cita, CONVERT(varchar, Fecha_Cita, 103) as Fecha, LEFT(CONVERT(varchar, Hora_Cita, 108), 5) as Horario, 
		Numero_Consultorio as Consultorio, (u.Nombre + ' ' + u.Apellido_Paterno + ' ' + u.Apellido_Materno) as Nombre
from Cita c
join Medico m on c.ID_Medico = m.ID_Medico
join Consultorio con on m.ID_Consultorio = con.ID_Consultorio
join Usuario u on m.ID_Usuario = u.ID_Usuario
join CitasHorario ch on c.ID_Cita_Horario = ch.ID_Horario
where c.ID_Estatus_Cita = 2;

--LLAMADO DE VISTAS
select * from Vista_Pacientes;
select * from Farmacia_Inventario;
select * from Contacto_Medicos;
select * from Citas_Pendientes;

---------FUNCIONES----------------

--Funcion Citas Paciente
create function CitasPaciente (@idPaciente int)
returns @tbCitasPaciente Table(
		Cita int, Fecha varchar(100), Horario varchar(100), Consultorio varchar(100), Doctor varchar(100))
as begin
Insert into @tbCitasPaciente
select ID_Cita, CONVERT(varchar, Fecha_Cita, 103), LEFT(CONVERT(varchar, Hora_Cita, 108), 5), 
		Numero_Consultorio, (u.Nombre + ' ' + u.Apellido_Paterno + ' ' + u.Apellido_Materno)
from Cita c
join Medico m on c.ID_Medico = m.ID_Medico
join Consultorio con on m.ID_Consultorio = con.ID_Consultorio
join Usuario u on m.ID_Usuario = u.ID_Usuario
join CitasHorario ch on c.ID_Cita_Horario = ch.ID_Horario
where c.ID_Paciente = @idPaciente;
return
end

--Funcion Precio Medicina
alter function ObtenerPrecioMedicina (@idMedicina int)
returns table
as return (
select concat('El precio de ' , m.Nombre_Medicina , 'es $' , m.Precio_Medicina) as Precio
from Medicina m
where m.ID_Medicina = @idMedicina)

--Funcion Localizar Doctor
create function LocalizarDoctor (@idDoctor int)
returns @tbinfoDoctor Table(
		 Doctor varchar(100), Piso varchar(100), Consultorio varchar(100))
as begin
Insert into @tbinfoDoctor
select (u.Nombre + ' ' + u.Apellido_Paterno + ' ' + u.Apellido_Materno), Piso ,Numero_Consultorio
from Medico m
join Consultorio con on m.ID_Consultorio = con.ID_Consultorio
join Usuario u on m.ID_Usuario = u.ID_Usuario
where m.ID_Medico = @idDoctor;
return
end

--Funcion Medicinas Por Agotarse
create function ObtenerMedicina (@cantidadMinima int)
returns table
as return (
select m.Nombre_Medicina as Medicina, m.Cantidad as 'Unidades Restantes'
from Medicina m
where m.Cantidad <= @cantidadMinima)

--LLAMADO DE FUNCIONES
select * from dbo.CitasPaciente(10);
select * from dbo.ObtenerPrecioMedicina(1);
select * from dbo.LocalizarDoctor(30);
select * from dbo.ObtenerMedicina(10);