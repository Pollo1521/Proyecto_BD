CREATE FUNCTION dbo.ObtenerTotalTicket
(
    @ID_Venta INT
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @Total FLOAT;

    SELECT @Total = SUM(t.Subtotal)
    FROM Venta v
    INNER JOIN Ticket t ON v.ID_Ventas = t.ID_Venta
    WHERE v.ID_Ventas = @ID_Venta;

    RETURN ISNULL(@Total, 0);
END;
GO

select * from Venta

SELECT dbo.ObtenerTotalTicket(28) AS Total;