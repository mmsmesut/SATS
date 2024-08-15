using Microsoft.EntityFrameworkCore;
using SATS.Business.Mappings;
using SATS.Business.Repositories;
using SATS.Business.Repositories.Interfaces;
using SATS.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Swagger:1
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


try
{




    builder.Services.AddAutoMapper(typeof(StudentProfile).Assembly);

    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetPagedListQueryHandler<StudentDto, Student>>());


    builder.Services.AddTransient<IStudentRepository, StudentRepository>();
    builder.Services.AddTransient<ICourseRepository, CourseRepository>();

    // MediatR configuration
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(assembly => assembly.FullName.Contains("SATS.Business"))
        .ToArray();
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

    //Mediator


    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateStudentCommandHandler).Assembly));
    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteStudentCommandHandler).Assembly));
    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateStudentCommandHandler).Assembly));
    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetStudentsQueryHandler).Assembly));
    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetStudentByIdQueryHandler).Assembly));

    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCourseCommandHandler).Assembly));
    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteCourseCommandHandler).Assembly));
    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCourseByIdQueryHandler).Assembly));
    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateCourseCommandHandler).Assembly));
    //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    // Register generic repository for other entities if needed


    //Db
    builder.Services.AddDbContext<SATSAppDbContext>(options =>
    options.UseNpgsql(@"Host=localhost;Database=localdb;Username=postgres;Password=mms;Search Path=sats")
);


    //ReferenceHandler.Preserve option in your JsonSerializerOptions to handle circular references. This tells the serializer to preserve object references, 
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: for pretty print
    });

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
