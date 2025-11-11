delimiter $$
use `5to_Trivago`$$
SELECT 'Creando Triggers' AS 'Estado'$$

drop trigger if exists befInsReserva$$
create trigger befInsReserva before insert on Reserva
for each row
begin
declare disponible bool;
select DisponibilidadFecha(New.idHabitacion, New.Entrada, New.Salida) into disponible;
if(disponible = false)
then
signal sqlstate '45000'
set message_text = "La fecha solicitada no disponible";
end if;
end$$

drop trigger if exists befInsUsuario$$
create trigger befInsUsuario before insert on Usuario
for each row
begin
set New.Contrasena = sha2(New.Contrasena, 256);
end$$


DELIMITER //

drop trigger if exists beforeInsHabitacion //
CREATE TRIGGER beforeInsHabitacion
BEFORE INSERT ON Habitacion
FOR EACH ROW
BEGIN
  DECLARE next_num INT;

  -- obtener el siguiente número para ese curso
  SELECT COALESCE(MAX(nroHabitacion), 0) + 1
  INTO next_num
  FROM Habitacion
  WHERE idTipo = NEW.idTipo
    AND idHotel = NEW.idHotel;

  SET NEW.nroHabitacion = next_num;
END //

DELIMITER ;
