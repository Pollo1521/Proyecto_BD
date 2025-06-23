create trigger CambiarAPendiente
on Pago
after update
as
begin
set nocount on;
update Cita
set ID_Estatus_Cita = 2
from Cita
join inserted i on Cita.ID_Cita = i.ID_Cita
join deleted d on i.ID_Pago = d.ID_Pago
where i.Estado_Pago = 1 and d.Estado_Pago = 0;
end;

disable trigger CambiarAPendiente on Pago;