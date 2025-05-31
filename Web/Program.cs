using AutoMapper;
using Business.Implements;
using Business.Interfaces;
using Data.Implements;
using Data.Interfaces;
using Entity.Context;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Utilities.Helpers;
using Utilities.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configuración de Logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Registro de dependencias - Data Layer
builder.Services.AddScoped<IAppointmentData, AppointmentData>();
builder.Services.AddScoped<IDoctorData, DoctorData>();
builder.Services.AddScoped<IPatientData, PatientData>();

// Registro de dependencias - Business Layer
builder.Services.AddScoped<IAppointmentBusiness, AppointmentBusiness>();
builder.Services.AddScoped<IDoctorBusiness, DoctorBusiness>();
builder.Services.AddScoped<IPatientBusiness, PatientBusiness>();

// Registro de utilidades
builder.Services.AddScoped<IGenericIHelpers, GenericHelpers>();
builder.Services.AddScoped<IDatetimeHelper, DatetimeHelper>();
builder.Services.AddScoped<IValidationHelper, ValidationHelper>();

// Configuración de Controllers (si es Web API)
builder.Services.AddControllers();

// Configuración de Swagger para documentación de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Medical Management API",
        Version = "v1",
        Description = "API para gestión de citas médicas, doctores y pacientes"
    });

    // Incluir comentarios XML en Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configuración de CORS (si es necesario)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // Ajusta según tus necesidades
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Configuración de validación con FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Pipeline de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medical Management API v1");
        c.RoutePrefix = string.Empty; // Para que Swagger esté en la raíz
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Aplicar política de CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication(); // Si tienes autenticación
app.UseAuthorization();

app.MapControllers();

// Migración automática de base de datos (opcional - solo para desarrollo)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Aplicar migraciones pendientes
        context.Database.Migrate();

        // Semilla de datos inicial (opcional)
        // await SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al aplicar migraciones o semillas de datos");
    }
}

app.Run();