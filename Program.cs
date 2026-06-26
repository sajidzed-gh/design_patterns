
using en = UmhBackend.HouseholdEntity;
using fp = UmhBackend.FactoryPattern;
using UmhBackend.Repository;
using UmhBackend.StructuralPatterns.wrapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<en.IHouseholdService, en.HouseholdService>();
builder.Services.AddScoped<fp.IHouseholdService, fp.HouseholdService>();

//builder.Services.AddScoped<IHouseholdRepository, HouseholdRepository>();
builder.Services.AddScoped<HouseholdRepository>();

builder.Services.AddScoped<IHouseholdRepository>(provider => {
    var rawRepo = provider.GetRequiredService<HouseholdRepository>();
    return new LoggingHouseholdRepository(rawRepo);
});


var app = builder.Build();



app.MapGet("/", () => "Peace World!");

app.MapGet("/one", () => new
{
    Status = "Online",
    Engine = " Unified Managed Houshold System",
    LastCalculated = DateTime.UtcNow

});



app.MapGet("/household", () =>  {

    var samplePortFolio = new en.HouseHoldPortfolio(
         "Hid_1245488",
        45000,
        8620000
        );

    return samplePortFolio;
});

app.MapGet("/household/{id}", (string id, en.IHouseholdService householdService) =>
{
    var portfolio = householdService.CalculateHouseHoldWealth(id);
    return Results.Ok(portfolio);
});

app.MapGet("/factoryPattern/household/{id}", (string id, fp.IHouseholdService householdService) =>
{
    var portfolio = householdService.CalculateHouseHoldWealth(id);
    return Results.Ok(portfolio);
});

app.Run();


