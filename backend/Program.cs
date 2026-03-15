using backend.Contracts;
using backend.Database;
using backend.MappingProfiles;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conString = builder.Configuration.GetConnectionString("DbCon");
builder.Services.AddDbContext<SetGameDbContext>(options =>
    options.UseMySql(conString, ServerVersion.AutoDetect(conString)));

// builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod()));

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICardsRepository, CardsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();