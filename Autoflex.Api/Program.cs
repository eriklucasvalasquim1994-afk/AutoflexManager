using Autoflex.Domain.Interfaces;
using Autoflex.Infrastructure.Data;
using Autoflex.Infrastructure.Repositories;
using Autoflex.Application.Interfaces; 
using Autoflex.Application.Services;  
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar o Contexto do Banco (Postgres)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrar os Repositórios e Services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRawMaterialRepository, RawMaterialRepository>();
builder.Services.AddScoped<IProductionService, ProductionService>(); // Importante!

builder.Services.AddCors();
builder.Services.AddControllers();

// 3. Configurar Swagger (Substituindo o OpenApi para ter a interface visual)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddCors(options => {
     options.AddPolicy("AllowAll", policy => {
         policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
     });
 });

var app = builder.Build();

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();

// 4. Ativar Swagger em qualquer ambiente para facilitar seu teste agora
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Autoflex API V1");
    c.RoutePrefix = string.Empty; // Isso faz o Swagger abrir direto no localhost:7154/ sem precisar digitar nada!
});

app.UseCors("AllowReact");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();