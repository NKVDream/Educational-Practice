using Microsoft.EntityFrameworkCore;
using NewsApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Настройка базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем поддержку CORS для фронтенда
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("http://localhost:5500", "http://127.0.0.1:5500")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// ВАЖНО: Добавляем Swagger ВНЕ зависимости от окружения
app.UseSwagger();     // ← ЭТО ОБЯЗАТЕЛЬНО
app.UseSwaggerUI();   // ← ЭТО ОБЯЗАТЕЛЬНО

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseSession();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
