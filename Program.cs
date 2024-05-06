using chairs_dotnet7_api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");

//TODO: ASIGNACION DE RUTAS A LOS ENDPOINTS
chairs.MapPost("/chair", CreateChair);
chairs.MapGet("/chair", GetChairs);
chairs.MapGet("/chair/{name}", GetChairByName);
chairs.MapPut("/chair/{id}", UpdateChair);


app.Run();

//TODO: ENDPOINTS SOLICITADOS
static async Task<IResult> CreateChair(DataContext db, Chair chair)
{
    Chair? chairFound = await db.FindAsync<Chair>(chair.Id);
    if (chairFound is not null)
        return TypedResults.BadRequest("The chair already exists");
    await db.AddAsync(chair);
    await db.SaveChangesAsync();
    return TypedResults.Created("Chair created successfully");
}
static async IResult GetChairs(DataContext db)
{
    var chairs = await db.
}
