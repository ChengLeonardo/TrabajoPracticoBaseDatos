-- Elimina la base de datos si existe
DROP DATABASE IF EXISTS `5to_Trivago`;

-- Crea la base de datos
CREATE DATABASE IF NOT EXISTS `5to_Trivago`;
USE `5to_Trivago`;

-- -----------------------------------------------------
-- Tabla: Pais
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Pais`;

CREATE TABLE `Pais` (
  `idPais` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idPais`),
  UNIQUE KEY `nombre_UNIQUE` (`Nombre`)
);

-- -----------------------------------------------------
-- Tabla: Ciudad
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Ciudad`;

CREATE TABLE `Ciudad` (
  `idCiudad` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idPais` INT UNSIGNED NOT NULL,
  `Nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idCiudad`),
  UNIQUE KEY `Ciudad_UNIQUE` (`idPais`, `Nombre`),
  CONSTRAINT `fk_Ciudad_Pais`
    FOREIGN KEY (`idPais`)
    REFERENCES `Pais` (`idPais`)
    ON DELETE CASCADE
);

-- -----------------------------------------------------
-- Tabla: Hotel
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Hotel`;

CREATE TABLE `Hotel` (
  `idHotel` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idCiudad` INT UNSIGNED NOT NULL,
  `Nombre` VARCHAR(45) NOT NULL,
  `Direccion` VARCHAR(90) NOT NULL,
  `Telefono` VARCHAR(20) NOT NULL,
  `Email` VARCHAR(40) NOT NULL,
  `URL` VARCHAR(90),
  PRIMARY KEY (`idHotel`),
  UNIQUE KEY `Telefono_UNIQUE` (`Telefono`),
  UNIQUE KEY `Direccion_UNIQUE` (`Direccion`),
  UNIQUE KEY `URL_UNIQUE` (`URL`),
  CONSTRAINT `fk_Hotel_Ciudad`
    FOREIGN KEY (`idCiudad`)
    REFERENCES `Ciudad` (`idCiudad`)
    ON DELETE CASCADE
);

-- -----------------------------------------------------
-- Tabla: TipoHabitacion
-- -----------------------------------------------------
DROP TABLE IF EXISTS `TipoHabitacion`;

CREATE TABLE `TipoHabitacion` (
  `idTipo` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idTipo`),
  UNIQUE KEY `Nombre_UNIQUE` (`Nombre`)
);

-- -----------------------------------------------------
-- Tabla: Habitacion
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Habitacion`;

CREATE TABLE `Habitacion` (
  `idHabitacion` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idHotel` INT UNSIGNED NOT NULL,
  `idTipo` INT UNSIGNED NOT NULL,
  `nroHabitacion` INT UNSIGNED NOT NULL,
  `PrecioPorNoche` DECIMAL(10,2) UNSIGNED NOT NULL,
  PRIMARY KEY (`idHabitacion`),
  UNIQUE KEY `Habitacion_UNIQUE` (`idHotel`, `nroHabitacion`, `idTipo`),
  CONSTRAINT `fk_Habitacion_Hotel`
    FOREIGN KEY (`idHotel`)
    REFERENCES `Hotel` (`idHotel`)
    ON DELETE CASCADE,
  CONSTRAINT `fk_Habitacion_TipoHabitacion`
    FOREIGN KEY (`idTipo`)
    REFERENCES `TipoHabitacion` (`idTipo`)
);

-- -----------------------------------------------------
-- Tabla: MetodoPago
-- -----------------------------------------------------
DROP TABLE IF EXISTS `MetodoPago`;

CREATE TABLE `MetodoPago` (
  `idMetodoPago` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `TipoMedioPago` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idMetodoPago`),
  UNIQUE KEY `TipoMedioPago_UNIQUE` (`TipoMedioPago`)
);

-- -----------------------------------------------------
-- Tabla: Rol
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Rol`;

CREATE TABLE `Rol` (
  `idRol` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`idRol`),
  UNIQUE KEY `Nombre_UNIQUE` (`Nombre`)
);

-- -----------------------------------------------------
-- Tabla: Usuario
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Usuario`;

CREATE TABLE `Usuario` (
  `idUsuario` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idRol` INT UNSIGNED NOT NULL,
  `Nombre` VARCHAR(45) NOT NULL,
  `Apellido` VARCHAR(45) NOT NULL,
  `Mail` VARCHAR(60) NOT NULL,
  `Contrasena` CHAR(64) NOT NULL,
  PRIMARY KEY (`idUsuario`),
  UNIQUE KEY `Mail_UNIQUE` (`Mail`),
  CONSTRAINT `fk_Usuario_Rol`
    FOREIGN KEY (`idRol`)
    REFERENCES `Rol` (`idRol`)
);

-- -----------------------------------------------------
-- Tabla: Reserva
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Reserva`;

CREATE TABLE `Reserva` (
  `idReserva` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idHabitacion` INT UNSIGNED NOT NULL,
  `idMetodoPago` INT UNSIGNED NOT NULL,
  `idUsuario` INT UNSIGNED NOT NULL,
  `Entrada` DATETIME NOT NULL,
  `Salida` DATETIME NOT NULL,
  `Precio` DECIMAL(10,2) UNSIGNED NOT NULL,
  `Telefono` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`idReserva`),
  CONSTRAINT `fk_Reserva_Habitacion`
    FOREIGN KEY (`idHabitacion`)
    REFERENCES `Habitacion` (`idHabitacion`),
  CONSTRAINT `fk_Reserva_MetodoPago`
    FOREIGN KEY (`idMetodoPago`)
    REFERENCES `MetodoPago` (`idMetodoPago`),
  CONSTRAINT `fk_Reserva_Usuario`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `Usuario` (`idUsuario`)
);

-- -----------------------------------------------------
-- Tabla: Comentario
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Comentario`;

CREATE TABLE `Comentario` (
  `idComentario` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idHabitacion` INT UNSIGNED NOT NULL,
  `Comentario` VARCHAR(255) NOT NULL,
  `Calificacion` TINYINT UNSIGNED NOT NULL CHECK (`Calificacion` BETWEEN 1 AND 10),
  `Fecha` DATETIME NOT NULL,
  PRIMARY KEY (`idComentario`),
  CONSTRAINT `fk_Comentario_Habitacion`
    FOREIGN KEY (`idHabitacion`)
    REFERENCES `Habitacion` (`idHabitacion`)
);
