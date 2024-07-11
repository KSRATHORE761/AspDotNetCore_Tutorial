using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDbContext>(opt =>opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config=>
    {
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
    }
);

var app = builder.Build();
if(app.Environment.IsDevelopment()){
    app.UseOpenApi();
    app.UseSwaggerUi(config=>{
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

var todo = app.MapGroup("todoItems");

// todo.MapGet("/", async(TodoDbContext db)=> await db.Todos.ToListAsync());

// todo.MapGet("/Complete", async(TodoDbContext db) => await db.Todos.Where(t=>t.isCompleted).ToListAsync());
// todo.MapGet("/{id}", async(TodoDbContext db, int id) =>{
//     var item = await db.Todos.FindAsync(id);
//     if(item == null){
//        return Results.NotFound();
//     }
//     return Results.Ok(item);
// });

// todo.MapPost("/", async(TodoDbContext db, Todo todoItem)=>{
//     db.Todos.Add(todoItem);
//     await db.SaveChangesAsync(); 
//     return Results.Created($"/todoItems/{todoItem.Id}", todoItem);
// });

// todo.MapPut("/{id}", async(TodoDbContext db, int id, Todo inputTodoitem)=>{
//     var item = await db.Todos.FindAsync(id);
//     if(item==null){
//         return Results.NotFound();
//     }
//     item.Title = inputTodoitem.Title;
//     item.isCompleted= inputTodoitem.isCompleted;
//     await db.SaveChangesAsync();
//     return Results.NoContent();
// });

// todo.MapDelete("/{id}", async(TodoDbContext db, int id) =>{
//     var item = await db.Todos.FindAsync(id);
//     if(item==null){
//         return Results.NotFound();      
//     }
//     db.Todos.Remove(item);
//     await db.SaveChangesAsync();
//     return Results.NoContent();
// });

todo.MapGet("/", GetTodos);
todo.MapGet("/Complete", GetCompletedTodos);
todo.MapGet("/{id}", GetTodo);
todo.MapPost("/", CreateTodo);
todo.MapPut("/{id}", UpdateTodo);

todo.MapDelete("/{id}", DeleteTodo);

static async Task<IResult> GetTodos(TodoDbContext db) => TypedResults.Ok(await db.Todos.Select(x=>new TodoItemDTO(x)).ToListAsync());
static async Task<IResult> GetCompletedTodos(TodoDbContext db) => TypedResults.Ok(await db.Todos.Where(x=>x.isCompleted).Select(x=>new TodoItemDTO(x)).ToListAsync());  
static async Task<IResult> GetTodo(TodoDbContext db, int id)
{
    var item = await db.Todos.FindAsync(id);
    if (item==null){
        return TypedResults.NotFound();
    }
    return TypedResults.Ok(new TodoItemDTO(item));
}

static async Task<IResult> CreateTodo(TodoDbContext db, TodoItemDTO todoItemDTO)
{
    var todo = new Todo{
        Title = todoItemDTO.Title,
        isCompleted = todoItemDTO.IsCompleted,
    };
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/todoItems/{todo.Id}", todoItemDTO);
}

static async Task<IResult> UpdateTodo(TodoDbContext db, int id, TodoItemDTO todoItemDTO)
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return TypedResults.NotFound();
    todo.Title = todoItemDTO.Title;
    todo.isCompleted = todoItemDTO.IsCompleted;
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(TodoDbContext db, int id)
{
    var todo = await db.Todos.FindAsync(id);
    if(todo==null){
        return TypedResults.NotFound();
    }
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return TypedResults.Ok();
}
app.Run();


