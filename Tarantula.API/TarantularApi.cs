using Microsoft.AspNetCore.Mvc;
using Tarantula.Indexer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var dbPath = @"../Tarantula.Runner/bin/Debug/net9.0/index.db";
builder.Services.AddSingleton<TIndexer>(_ => new TIndexer());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UsePathBase("/Tarantula");
app.UseCors();

app.MapControllers();
app.MapGet("/", () => Results.Content(@"
    <html>
    <head><title>Tarantula API</title></head>
    <body style='font-family:sans-serif; text-align:center; margin-top:100px;'>
        <h1>Tarantula API is Alive</h1>
        <p> search endpoint : <code>/api/search?query=example</code>.</p>
    </body>
    </html>
", "text/html"));
app.MapGet("/search", async ([FromQuery] string query, [FromServices] TIndexer indexer) =>
{
    if (string.IsNullOrWhiteSpace(query))
        return Results.BadRequest("Query parameter is required.");
    
    var results = await indexer.Search(query);

    return Results.Ok(results.Select(r => new
    {
        Url = r.Url,
        Relevance = r.Score,
        Meta = r.Meta,
        Title = r.Title,

    }));
});
app.Run();
