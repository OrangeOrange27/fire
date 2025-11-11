using System.Collections.Concurrent;
using System.Text.Json;
using Server;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var players = new ConcurrentDictionary<string, int>();

// Middleware to read token from header
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("UserToken", out var token))
    {
        context.Items["Token"] = token.ToString();
    }
    await next.Invoke();
});

// POST /auth/login
app.MapPost("/api/auth/login", async context =>
{
    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var token = body.Trim('"');

    players.TryAdd(token, 0);

    await context.Response.WriteAsync("OK");
});

// GET /click
app.MapGet("/api/click", async context =>
{
    if (!context.Items.TryGetValue("Token", out var tokenObj) || tokenObj == null)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Missing token");
        return;
    }

    var token = tokenObj.ToString();

    var count = players.AddOrUpdate(token, 1, (_, current) => current + 1);

    var response = new ClickResponse { RequestCount = count };

    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
});

app.Run();