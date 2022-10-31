using CrudWithMongoDb.Data;
using CrudWithMongoDb.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<DatabaseSetting>(options =>
{
    options.ConnectionString = builder.Configuration.GetSection("StudentStoreDatabaseSettings:ConnectionString").Value;
    options.DatabaseName = builder.Configuration.GetSection("StudentStoreDatabaseSettings:DatabaseName").Value;
    options.StudentCoursesCollectionName = builder.Configuration.GetSection("StudentStoreDatabaseSettings:StudentCoursesCollectionName").Value;

});

builder.Services.AddScoped<IDatabaseSetting>(x => x.GetRequiredService<IOptions<DatabaseSetting>>().Value);
builder.Services.AddSingleton<IMongoClient>(x => new MongoClient(builder.Configuration.GetValue<string>("StudentStoreDatabaseSettings:ConnectionString")));
builder.Services.AddScoped<IStudentService, StudentService>();




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
