using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

//DI
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

//Pipeline
app.MapGet("/todoitems", async (TodoDb context) => Results.Ok(await context.Todos.ToListAsync()));

app.MapGet("/todoitems/complete", async (TodoDb context) => Results.Ok(await context.Todos.Where(todo => todo.IsComplete).ToListAsync()));

app.MapGet("/todoitems/{id}", async (TodoDb context, [FromRoute] int id) => {
    var existing = await context.Todos.FirstOrDefaultAsync(todo => todo.Id.Equals(id));
    if (existing == null) {
        return Results.NotFound();
    }
    return Results.Ok(existing);
});

app.MapPost("/todoitems", async (TodoDb context, [FromBody] TodoItem item) => {
    if (item != null) {
        await context.Todos.AddAsync(item);
    }
    await context.SaveChangesAsync();
    return Results.Created($"/todoitems/{item.Id}", item);
});

app.MapPut("/todoitems", async (TodoDb context, TodoItem item) => {
    var existing = await context.Todos.FindAsync(item.Id);
    if (existing == null) {
        return Results.NotFound();
    }
    existing.IsComplete = item.IsComplete;
    existing.Name = item.Name;
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (TodoDb context, int id) => {
    var existing = await context.Todos.FindAsync(id);
    if (existing == null) {
        return Results.NotFound();
    }
    context.Remove(existing);
    await context.SaveChangesAsync();
    return Results.Ok();
});

await app.RunAsync();
