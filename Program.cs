using FluentValidation.AspNetCore;
using GiveHearth.Config;
using GiveHearth.Context;
using GiveHearth.Interfaces;
using GiveHearth.Repositories;
using GiveHearth.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers()
    .AddFluentValidation();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiceRegister, ServiceRegister>();
builder.Services.AddScoped<IServiceEmail, ServiceEmail>();

builder.Services.AddScoped<IRepositoryRegister, RepositoryRegister>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.Configure<EmailSettings>(
        builder.Configuration.GetSection("EmailSettings")
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
