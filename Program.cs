using fonebook.Data;
using fonebook.Repositories.Interfaces;
using fonebook.Repositories.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add interface scope here
builder.Services.AddScoped<IContactsService, ContactsService>(); // Always add Interface scope before any other thing

/*builder.Services.AddDbContext<FonebookAPIDbContext>(options => 
    options.UseInMemoryDatabase("FonebookDb")  
); */
builder.Services.AddDbContext<FonebookAPIDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("FonebookApiConnectionString"))
);

// Add Authentication
string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                // tokenKeyString != null ? tokenKeyString : ""
                tokenKeyString ?? "" // simplified version of above expression
            )),
            ValidateIssuer = false, // for SSO or to validate where the token came from, set to true
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
