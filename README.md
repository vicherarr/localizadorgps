# Backend de Localización de Flotas

API RESTful desarrollada con ASP.NET Core 8 y Entity Framework Core para administrar vehículos, dispositivos Android y sus ubicaciones georreferenciadas en tiempo real.

## Características principales

- Autenticación y autorización mediante JWT Bearer.
- Registro y administración de vehículos y dispositivos móviles.
- Recepción de ubicaciones GPS y consulta de historial por rango de fechas.
- Listado de flota activa con reportes en los últimos 5 minutos.
- Documentación interactiva con Swagger (en español) y manejo centralizado de errores.
- Arquitectura en capas (Dominio, Aplicación, Infraestructura y Presentación) preparada para escalar.

## Requisitos previos

- .NET SDK 8.0
- SQL Server (opcional; por defecto se utiliza una base de datos en memoria para desarrollo). Si se desea utilizar SQL Server, actualiza la cadena de conexión `ConnectionStrings:LocalizadorGps` en `appsettings.json`.

## Estructura de carpetas

```
localizadorgps/
├── LocalizadorGps.sln
├── README.md
└── src/
    ├── Dominio/                 # Entidades y enumeraciones del dominio
    ├── Aplicacion/              # DTOs, servicios y excepciones de negocio
    ├── Infraestructura/         # DbContext, repositorios, JWT y registro de dependencias
    └── Presentacion/            # API ASP.NET Core, controladores, filtros y configuración
```

## Ejecución local

1. Restaurar dependencias y compilar la solución:
   ```bash
   dotnet restore
   dotnet build
   ```
2. Establecer (opcional) la variable de entorno `ASPNETCORE_ENVIRONMENT=Development`.
3. Ejecutar la API:
   ```bash
   dotnet run --project src/Presentacion/LocalizadorGps.Presentacion.csproj
   ```
4. Abrir la documentación Swagger en `https://localhost:5001/swagger`.

## Endpoints destacados

| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/autenticacion/registro` | Registro de un nuevo dispositivo (requiere rol `Administrador`). |
| POST | `/api/autenticacion/inicio-sesion` | Inicia sesión y devuelve un JWT. |
| POST | `/api/vehiculos` | Crea un vehículo (rol `Administrador`). |
| GET | `/api/vehiculos/{id}` | Obtiene los datos de un vehículo. |
| GET | `/api/vehiculos/activos` | Lista vehículos con ubicaciones en los últimos 5 minutos. |
| PUT | `/api/vehiculos/{id}` | Actualiza descripción/estado del vehículo (rol `Administrador`). |
| POST | `/api/ubicaciones` | Registra una nueva muestra de localización. |
| GET | `/api/ubicaciones/vehiculos/{id}/actual` | Obtiene la última ubicación de un vehículo. |
| GET | `/api/ubicaciones/vehiculos/{id}/historial?desdeUtc=&hastaUtc=` | Consulta el historial por rango de fechas UTC. |

Todas las rutas, excepto el inicio de sesión, requieren un token JWT válido enviado en el encabezado `Authorization: Bearer {token}`.

## Seguridad y buenas prácticas

- Hashing de contraseñas con `PasswordHasher<UsuarioDispositivo>`.
- Validaciones de entrada mediante `DataAnnotations` y reglas de negocio centralizadas.
- JWT firmado con HMAC-SHA256 y expiración configurable (`Jwt:MinutosExpiracion`).
- Filtro global `ManejadorExcepcionesFiltro` para respuestas JSON coherentes y registro de errores críticos.

## Despliegue recomendado

1. **Configuración segura**
   - Establece variables de entorno o secretos de producción para `Jwt:ClaveSecreta`, `Jwt:Emisor`, `Jwt:Audiencia` y la cadena de conexión a base de datos.
   - Habilita HTTPS y certificados válidos detrás de un reverse proxy como Nginx o Azure Front Door.
2. **Base de datos**
   - Ejecuta migraciones de EF Core (`dotnet ef migrations add Inicial` / `dotnet ef database update`) en un pipeline controlado.
   - Implementa copias de seguridad y monitoreo del motor (SQL Server, Azure SQL o PostgreSQL ajustando el proveedor EF).
3. **Escalabilidad**
   - Conteneriza la API (Docker) y orquesta con Kubernetes/Azure App Service.
   - Activa logging centralizado (Application Insights, ELK) y métricas de salud (`/health` puede añadirse fácilmente con `MapHealthChecks`).
4. **Hardening**
   - Restringe CORS según los dominios del cliente móvil.
   - Implementa rotación periódica de claves JWT y tokens de renovación si es necesario.

Con esta base podrás extender reglas de negocio, añadir notificaciones en tiempo real (SignalR) o integraciones con paneles de monitoreo.
