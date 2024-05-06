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
    Chair? chairFound = await db.Chairs.FindAsync(chair.Id);
    if (chairFound is not null)
        return TypedResults.BadRequest("The chair already exists");
    await db.AddAsync(chair);
    await db.SaveChangesAsync();
    return TypedResults.Created("Chair created successfully");
}
static async Task<IResult> GetChairs(DataContext db)
{
    var chairs = await db.Chairs.ToListAsync<Chair>();
    return TypedResults.Ok(chairs);
}

static async Task<IResult> GetChairByName(string name, DataContext db)
{
    Chair? chairFound = await db.Chairs.Where(c => c.Nombre == name).FirstOrDefaultAsync();
    if (chairFound is null)
        return TypedResults.NotFound();
    return TypedResults.Ok(chairFound);
}