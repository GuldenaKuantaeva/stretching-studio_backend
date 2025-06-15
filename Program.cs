using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using StretchingStudioAPI.Data;
using StretchingStudioAPI.Middleware.Transformers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("https://guldenakuantaeva-stretching-studio-front-4f32.twc1.net")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Настраиваем Kestrel явно
//builder.WebHost.ConfigureKestrel(options =>
//{
//   options.ListenAnyIP(5004); // HTTP
//    options.ListenAnyIP(7229, listenOptions =>
//    {
//        listenOptions.UseHttps();
//    });
//});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    // в проде заменить получение connection string на переменные окружения
    builder.Services.AddDbContext<AuthContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnection")));

    builder.Services.AddDbContext<BookingServiceContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("BookingServiceConnection")));

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policyBuilder =>
        {
            policyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

builder.Services.AddControllers(options =>
    options.Conventions.Add(new RouteTokenTransformerConvention(new ToKebabParameterTransformer())));
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<AuthContext>();

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
