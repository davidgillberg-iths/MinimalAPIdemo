var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Simulerad databas
List<User> users = new()
{
    new User { Id = 1, Name = "Alice" },
    new User { Id = 2, Name = "Bob" },
    new User { Id = 3, Name = "Charlie" }
};

// GET - Hämta alla användare
app.MapGet("/users", () => users);

// GET - Hämta en användare baserat på ID
app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound($"Ingen användare med ID {id}.");
});

// GET - Hämta en användare baserat på namn (query parameter)
app.MapGet("/users/search", (string name) =>
{
    var user = users.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    return user is not null ? Results.Ok(user) : Results.NotFound($"Ingen användare med namnet {name}.");
});

// POST - Lägg till en ny användare
app.MapPost("/users", (User user) =>
{
    if (users.Any(u => u.Id == user.Id))
        return Results.BadRequest("Användar-ID redan upptaget.");

    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

// PUT - Uppdatera en användare
app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound("Användaren hittades inte.");

    user.Name = updatedUser.Name;
    return Results.Ok(user);
});

// DELETE - Ta bort en användare
app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound("Användaren hittades inte.");

    users.Remove(user);
    return Results.Ok($"Användare med ID {id} togs bort.");
});

app.Run();

// Enkel User-klass
record User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
