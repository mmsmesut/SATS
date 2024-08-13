using Microsoft.EntityFrameworkCore;
using SATS.Business.Handlers;
using SATS.Business.Repositories;
using SATS.Business.Repositories.Interfaces;
using SATS.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Swagger:1
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


try
{
    builder.Services.AddTransient<IStudentRepository, StudentRepository>();


    //Mediator
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetStudentsQueryHandler).Assembly));
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetStudentByIdQueryHandler).Assembly));
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    //Db
    builder.Services.AddDbContext<SATSAppDbContext>(options =>
        options.UseNpgsql(@"Host=localhost;Database=localdb;Username=postgres;Password=mms;Search Path=sats")
    );

}
catch (AggregateException ex)
{
    foreach (var innerException in ex.InnerExceptions)
    {
        Console.WriteLine(innerException.Message);
    }
}

var app = builder.Build();

//Swagger:1
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();
app.Run();
