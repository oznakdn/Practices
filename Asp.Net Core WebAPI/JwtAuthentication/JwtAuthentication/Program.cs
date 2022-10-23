using JwtAuthentication.Data;
using JwtAuthentication.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenHandler = JwtAuthentication.JWT.TokenHandler;

var builder = WebApplication.CreateBuilder(args);




/* Herhangi bir servis'e ulasmak icin
   var service = builder.Services.BuildServiceProvider().GetService<Product>();
*/

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlite(builder.Configuration["ConnectionStrings:AppDbContext"]));
builder.Services.AddScoped<ITokenHandler, TokenHandler>();

// JWT conf.
var _jwtSetting = builder.Configuration.GetSection("JWTSetting"); // appsettings.json
builder.Services.Configure<JWTSetting>(_jwtSetting); // JWTSetting class

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = true;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidIssuer = builder.Configuration["JWTSetting:Issuer"],
        ValidAudience = builder.Configuration["JWTSetting:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSetting:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow :false,
        ClockSkew = TimeSpan.Zero
    };
});

/* From database origin list Default Cors
var dbContext = builder.Services.BuildServiceProvider().GetService<AppDbContext>();
List<string> origins = new List<string>();
if (dbContext != null)
{
    var originDatas = dbContext.Origins.where(o => o.IsActive).ToList();
    if (originDatas != null && originDatas.Count > 0)
    {
        originDatas.foreach (item =>
        {
            origins.Add(item.OriginName);
        }) ;
    }
}

builder.Services.AddCors(opt => opt.AddDefaultPolicy(conf =>
{
    conf.WithOrigins(origins.ToArray());
    conf.AllowAnyMethod();
    conf.AllowAnyHeader();
}));

*/


/* Default Cors 
builder.Services.AddCors(opt => opt.AddDefaultPolicy(conf =>
{
    conf.WithOrigins("http://locahost:4200", "http://locahost:3000");
    conf.AllowAnyMethod();
    conf.AllowAnyHeader();
}));
*/


/* Custom Cors
builder.Services.AddCors(setup => setup.AddPolicy("CustomCorsPolicy", option =>
{
    option.WithOrigins("http://locahost:4200", "http://locahost:3000")
   .AllowAnyHeader()
   .AllowAnyMethod();
}));
*/


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/* Default Cors
app.UseCors();
*/

/* Custom Cors
app.UseCors("CustomCorsPolicy");
*/

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
