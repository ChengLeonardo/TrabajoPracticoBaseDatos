use `5to_Trivago` ;
SELECT 'Creando SF' AS 'Estado';
delimiter $$

DROP PROCEDURE IF EXISTS obtener_usuario$$
CREATE PROCEDURE obtener_usuario(
    IN mail VARCHAR(60),
    IN contra CHAR(64)
)
BEGIN
    SELECT U.*, R.*
    FROM Usuario U
    INNER JOIN Rol R USING(idRol)
    WHERE U.Mail = mail AND U.Contrasena = SHA2(contra, 256);
END$$


drop function if exists verificacion_mail_registrado$$
create function verificacion_mail_registrado(unMail varchar(60))
returns bool reads sql data
begin
declare existe bool;
    set existe = false;
    if(
exists(
select *
            from Usuario
            where Mail = unMail
)
)
    then
set existe = true;
end if;
    return true;
end$$

drop function if exists DisponibilidadFecha$$
create function DisponibilidadFecha(unIdHabitacion int unsigned, unaEntrada datetime, unaSalida datetime )
returns bool reads sql data
begin
declare disponible bool;
    set disponible = true;
    if(
exists(
select *
            from Reserva
            where (Entrada >= unaEntrada and Entrada <= unaSalida)
            or (Entrada <= unaEntrada and Salida >= unaEntrada)
            and idHabitacion = unIdHabitacion
            and unaEntrada < now()
        )
    )
then
set disponible = false;
end if;
    return disponible;
end$$

drop function if exists HabitacionesDisponiblesTipo$$
create function HabitacionesDisponiblesTipo(unIdTipo int unsigned, unidHotel int unsigned, unaEntrada datetime, unaSalida datetime)
returns tinyint unsigned reads sql data
begin
declare disponibles tinyint unsigned ;
select count(*) into disponibles
from Habitacion
where idHotel = unIdHotel and idTipo = unIdTipo;
return disponibles;
end$$
