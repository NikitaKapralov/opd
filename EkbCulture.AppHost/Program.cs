using EkbCulture.AppHost.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.Swagger;
using Swashbuckle.SwaggerUi;
using Swashbuckle.AspNetCore.Swagger;


var builder = WebApplication.CreateBuilder(args); //делаем builder

// 2. Добавляем сервисы ДО вызова builder.Build()
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer(); // 3. Добавляем Swagger ДО Build()
builder.Services.AddSwaggerGen();

var app = builder.Build(); // 4. Строим приложение

// 5. Настройка middleware ПОСЛЕ Build()
app.UseSwagger();
app.UseSwaggerUI();

// 6. Проверка подключения к БД
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    Console.WriteLine("Database connected successfully!");
}

app.Run(); // 7. Запускаем приложение