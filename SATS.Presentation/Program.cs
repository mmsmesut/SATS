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

// JWT Ayarlarýný `appsettings.json` dosyasýndan çekme
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];


// JWT Authentication'ý ekleme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//JWT taþýyýcý (Bearer) kimlik doðrulamasýný kullanýr.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //özelliði, kimlik doðrulama gerektiren durumlarda hangi þemanýn kullanýlacaðýný belirle
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //(Zorunlu Deðil): Token'daki issuer'ý (token'ý kim oluþturduysa) doðrular. Güvenliði saðlamak için genellikle true yapýlýr.
        ValidateAudience = true,//(Zorunlu Deðil): Token'daki audience'ý (token'ýn hedef kitlesi) doðrular. Güvenliði artýrmak için genellikle true yapýlýr.
        ValidateLifetime = true, //Token'ýn süresinin dolup dolmadýðýný kontrol eder. Token süresinin doðrulanmasýný saðlamak için genellikle true yapýlýr.
        ValidateIssuerSigningKey = true,//Token'ý imzalamak için kullanýlan anahtarýn doðruluðunu kontrol eder. Güvenliði saðlamak için zorunludur.
        ValidIssuer = issuer, //Token'ýn bu deðeri içermesi beklenir.
        ValidAudience = audience, //Token'ýn bu deðeri içermesi beklenir.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),//: Token'ýn imzalanmasýnda kullanýlan anahtar. Bu anahtar, token'ýn geçerli olup olmadýðýný doðrulamak için kullanýlýr.
        ClockSkew = TimeSpan.FromMinutes(5)  // Token süresi dolduðunda bir miktar esneklik saðlar. Örneðin, 5 dakikalýk bir kayma toleransý, token süresi dolmuþ olsa bile bu süre içinde hala geçerli sayýlmasýný saðlar.
    };
});

builder.Services.AddAuthorization();

builder.Services.AddTransient<IEmailSender, EmailSender>();


// Swagger servisini ekleyin ve JWT Authorization için yapýlandýrýn
builder.Services.AddSwaggerGen(options => // AddSwaggerGen: (API belgeleri oluþturma aracý) yapýlandýrmak için kullanýlýr
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme //Bu metod, Swagger'da güvenlik þemasýný (JWT) nasýl tanýmlayacaðýnýzý yapýlandýrýr. Her bir parça þunlarý yapar:
    {
        Name = "Authorization", //Bu, kimlik doðrulama için kullanýlacak baþlýðýn adýný belirtir.
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,//Bu, güvenlik þemasýnýn HTTP tabanlý olduðunu belirtir
        Scheme = "Bearer", // Bu, kullanýlan kimlik doðrulama þemasýnýn Bearer token þemasý olduðunu belirtir. Yani, token'lar HTTP baþlýðýnda gönderilecektir.
        BearerFormat = "JWT",//Bu, Bearer token'ýnýn JWT formatýnda olduðunu belirtir.
        In = Microsoft.OpenApi.Models.ParameterLocation.Header, // Bu, token'ýn HTTP baþlýðýnda gönderilmesi gerektiðini belirtir. Query string veya diðer yerlerde gönderilmez.
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
        // Swagger UI'deki giriþ alaný için bir açýklama saðlar. Kullanýcýlara JWT token'larýný Bearer <token> formatýnda girmeleri gerektiðini söyler. Örneðin, token 12345abcdef ise, Bearer 12345abcdef þeklinde girilmelidir
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement//API uç noktalarý için hangi güvenlik þemalarýnýn gerektiðini belirtir. JWT token'ýnýn gerekli olduðunu belirtmek için kullanýlýr.
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



//// RabbitMQ ve MassTransit konfigürasyonu
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

// MassTransit yapýlandýrmasýný ekleyin
builder.Services.AddMassTransitConfiguration(builder.Configuration);


var app = builder.Build();

//Swagger:1
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware sýrasýna authentication ve authorization ekleme
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

/*
 Zorunluluk Durumu
Zorunlu Olanlar:

ValidateIssuerSigningKey: Token'ýn geçerli olup olmadýðýný doðrulamak için imza anahtarýnýn kontrol edilmesi zorunludur.
IssuerSigningKey: Token'ýn imzalanmasýnda kullanýlan anahtar, doðrulama sürecinin ayrýlmaz bir parçasýdýr.
Zorunlu Olmayanlar:

ValidateIssuer, ValidateAudience, ValidateLifetime: Bu doðrulamalar güvenliði artýrmak için önerilir, ancak zorunlu deðildir. Bu doðrulamalar, token'ýn belirli bir issuer, audience, veya belirli bir süre için geçerli olup olmadýðýný kontrol eder. Eðer uygulamanýz bu tür doðrulamalara ihtiyaç duymuyorsa, bu seçenekleri devre dýþý býrakabilirsiniz.
 */