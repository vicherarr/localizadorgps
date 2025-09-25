using System.Reflection;
using System.Text;
using LocalizadorGps.Infraestructura.Autenticacion;
using LocalizadorGps.Infraestructura.Servicios;
using LocalizadorGps.Presentacion.Filtros;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opciones =>
{
    opciones.Filters.Add<ManejadorExcepcionesFiltro>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Localización de Flotas",
        Version = "v1",
        Description = "Servicios REST para gestionar vehículos, dispositivos y ubicaciones en tiempo real."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    var seguridad = new OpenApiSecurityScheme
    {
        Name = "Autorización",
        Description = "Ingrese el token JWT con el prefijo 'Bearer'.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", seguridad);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { seguridad, Array.Empty<string>() }
    });
});

builder.Services.AgregarInfraestructura(builder.Configuration);

var opcionesJwt = builder.Configuration.GetSection(OpcionesJwt.Seccion).Get<OpcionesJwt>() ?? new OpcionesJwt();
var clave = Encoding.UTF8.GetBytes(opcionesJwt.ClaveSecreta.Length >= 16 ? opcionesJwt.ClaveSecreta : "ClaveSecretaMuySegura123!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = opcionesJwt.Emisor,
            ValidAudience = opcionesJwt.Audiencia,
            IssuerSigningKey = new SymmetricSecurityKey(clave),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("Administrador", politica => politica.RequireRole("Administrador"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "Documentación API Localizador GPS";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
