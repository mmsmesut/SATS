using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SATS.Business.Mappings;
using SATS.Business.Repositories;
using SATS.Business.Repositories.Interfaces;
using SATS.Core;
using SATS.Data;
using SATS.Presentation.Infrustructore.MassTransit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Swagger:1
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(StudentProfile).Assembly);
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetPagedListQueryHandler<StudentDto, Student>>());

builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<ICourseRepository, CourseRepository>();

// MediatR configuration
var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(assembly => assembly.FullName.Contains("SATS.Business"))
    .ToArray();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

#region MyRegion
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

#endregion

//Db
builder.Services.AddDbContext<SATSAppDbContext>(options =>
options.UseNpgsql(@"Host=localhost;Database=localdb;Username=postgres;Password=mms;Search Path=sats"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<SATSAppDbContext>()
    .AddDefaultTokenProviders();


//ReferenceHandler.Preserve option in your JsonSerializerOptions to handle circular references. This tells the serializer to preserve object references, 
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true; // Optional: for pretty print
});


builder.Services.AddScoped<TokenService>();

// JWT Ayarlar�n� `appsettings.json` dosyas�ndan �ekme
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];


// JWT Authentication'� ekleme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//JWT ta��y�c� (Bearer) kimlik do�rulamas�n� kullan�r.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //�zelli�i, kimlik do�rulama gerektiren durumlarda hangi �eman�n kullan�laca��n� belirle
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //(Zorunlu De�il): Token'daki issuer'� (token'� kim olu�turduysa) do�rular. G�venli�i sa�lamak i�in genellikle true yap�l�r.
        ValidateAudience = true,//(Zorunlu De�il): Token'daki audience'� (token'�n hedef kitlesi) do�rular. G�venli�i art�rmak i�in genellikle true yap�l�r.
        ValidateLifetime = true, //Token'�n s�resinin dolup dolmad���n� kontrol eder. Token s�resinin do�rulanmas�n� sa�lamak i�in genellikle true yap�l�r.
        ValidateIssuerSigningKey = true,//Token'� imzalamak i�in kullan�lan anahtar�n do�rulu�unu kontrol eder. G�venli�i sa�lamak i�in zorunludur.
        ValidIssuer = issuer, //Token'�n bu de�eri i�ermesi beklenir.
        ValidAudience = audience, //Token'�n bu de�eri i�ermesi beklenir.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),//: Token'�n imzalanmas�nda kullan�lan anahtar. Bu anahtar, token'�n ge�erli olup olmad���n� do�rulamak i�in kullan�l�r.
        ClockSkew = TimeSpan.FromMinutes(5)  // Token s�resi doldu�unda bir miktar esneklik sa�lar. �rne�in, 5 dakikal�k bir kayma tolerans�, token s�resi dolmu� olsa bile bu s�re i�inde hala ge�erli say�lmas�n� sa�lar.
    };
});

builder.Services.AddAuthorization();

builder.Services.AddTransient<IEmailSender, EmailSender>();


// Swagger servisini ekleyin ve JWT Authorization i�in yap�land�r�n
builder.Services.AddSwaggerGen(options => // AddSwaggerGen: (API belgeleri olu�turma arac�) yap�land�rmak i�in kullan�l�r
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme //Bu metod, Swagger'da g�venlik �emas�n� (JWT) nas�l tan�mlayaca��n�z� yap�land�r�r. Her bir par�a �unlar� yapar:
    {
        Name = "Authorization", //Bu, kimlik do�rulama i�in kullan�lacak ba�l���n ad�n� belirtir.
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,//Bu, g�venlik �emas�n�n HTTP tabanl� oldu�unu belirtir
        Scheme = "Bearer", // Bu, kullan�lan kimlik do�rulama �emas�n�n Bearer token �emas� oldu�unu belirtir. Yani, token'lar HTTP ba�l���nda g�nderilecektir.
        BearerFormat = "JWT",//Bu, Bearer token'�n�n JWT format�nda oldu�unu belirtir.
        In = Microsoft.OpenApi.Models.ParameterLocation.Header, // Bu, token'�n HTTP ba�l���nda g�nderilmesi gerekti�ini belirtir. Query string veya di�er yerlerde g�nderilmez.
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
        // Swagger UI'deki giri� alan� i�in bir a��klama sa�lar. Kullan�c�lara JWT token'lar�n� Bearer <token> format�nda girmeleri gerekti�ini s�yler. �rne�in, token 12345abcdef ise, Bearer 12345abcdef �eklinde girilmelidir
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement//API u� noktalar� i�in hangi g�venlik �emalar�n�n gerekti�ini belirtir. JWT token'�n�n gerekli oldu�unu belirtmek i�in kullan�l�r.
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


//get claims->payload info 
builder.Services.AddHttpContextAccessor();



//// RabbitMQ ve MassTransit konfig�rasyonu
//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<StudentConsumer>();
//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("localhost", "/", h =>
//        {
//            h.Username("guest");
//            h.Password("guest");
//        });

//        cfg.ConfigureEndpoints(context);
//    });
//});
//builder.Services.AddMassTransitHostedService();

// MassTransit yap�land�rmas�n� ekleyin
builder.Services.AddMassTransitConfiguration(builder.Configuration);


var app = builder.Build();

//Swagger:1
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware s�ras�na authentication ve authorization ekleme
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

/*
 Zorunluluk Durumu
Zorunlu Olanlar:

ValidateIssuerSigningKey: Token'�n ge�erli olup olmad���n� do�rulamak i�in imza anahtar�n�n kontrol edilmesi zorunludur.
IssuerSigningKey: Token'�n imzalanmas�nda kullan�lan anahtar, do�rulama s�recinin ayr�lmaz bir par�as�d�r.
Zorunlu Olmayanlar:

ValidateIssuer, ValidateAudience, ValidateLifetime: Bu do�rulamalar g�venli�i art�rmak i�in �nerilir, ancak zorunlu de�ildir. Bu do�rulamalar, token'�n belirli bir issuer, audience, veya belirli bir s�re i�in ge�erli olup olmad���n� kontrol eder. E�er uygulaman�z bu t�r do�rulamalara ihtiya� duymuyorsa, bu se�enekleri devre d��� b�rakabilirsiniz.
 */