<h1 align="center">E.T. Nº12 D.E. 1º "Libertador Gral. José de San Martín"</h1>
<p align="center">
  <img src="https://et12.edu.ar/imgs/computacion/vamoaprogramabanner.png">
</p>

Material de consumo para las consultas de Base de Datos.

## DER

<img src =https://github.com/user-attachments/assets/21362105-7167-4b4f-918d-c02b37c3b250>

# Ejemplo MinimalAPI

## Pre-requisitos 📋

  * SDK .NET 8
  * Visual Studio Code
  * MySQL Server 8.0.40

## Comenzando 🚀

Clonar el repositorio github, desde Github Desktop o ejecutar en la terminal o CMD:

```
git clone https://github.com/ChengLeonardo/TrabajoPracticoBaseDatos.git
```
Cambiar al branch actual:

```
git switch MinimalApi
```

## Despliegue 📦

1.  Abrir la terminal en el directorio raíz e instalar la BD por ejemplo con ingresando al SGBD por terminal `mysql -u 5to_agbd -p`, luego ingresar la pass y una vez dentro del shell de MySQL, ejecutar el comando `SOURCE script.sql`.

2.  Crear el archivo `appsettings.json` dentro del directorio `MinimalApi` con el contenido:

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "ConnectionStrings": {
        "MySQL": "Server=localhost;User=root;Password=root;Database=5to_trivago;"
      }
    }
    ```

3.  Abrir la terminal en el directorio donde están los scripts, entrar al directorio `MinimaApi` y ejecutar el comando `dotnet run`. Esto va a dejar en una terminal corriendo el servicio de tu API REST.

4.  Podemos ver la documentación y consumir nuestra API REST, ingresando a **http://localhost:5250/scalar/**. Utiliza esta interfaz para interactuar directamente con los endpoints de la API. Por ejemplo, puedes realizar peticiones GET al endpoint `/pais` para consumir la API.

## Colaboradores

| Año   | División| Participante                                                                |                                                                                                       |
| :---: | :---:   |       :---                                                                  | :---
| 2024  | 5° 7°   | Mario Rojas ([@MarioRojas]())
| 2024  | 5° 7°   | Luz Mabel ([@LuzMabel]())            
| 2024  | 5° 7°   | Leonardo Cheng ([@ChengLeonardo](https://github.com/ChengLeonardo))
