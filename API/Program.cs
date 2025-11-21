using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FluentValidation.AspNetCore;
using RestaurantReservation.Infrastructure;
using RestaurantReservation.Application.Interfaces;
using RestaurantReservation.Application.Services;
using RestaurantReservation.Application.Validators;
using RestaurantReservation.Application.Interfaces.IServices;
using RestaurantReservation.API.Middleware;
using Microsoft.OpenApi;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers & Validation
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAddressDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAddressDtoValidatorNullable>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateAddressDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRestaurantDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateRestaurantDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTableDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateTableDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateReservationDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo { Title = "Restaurant Reservation API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    /** The reference in OpenApiSecurityScheme has been remove 
        since .Net10 so I need to wait for the update at the alternative to reference
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    */
});

// JWT Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key not configured")))
    };
});

builder.Services.AddAuthorization();

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
