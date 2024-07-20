using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }
        [HttpGet]
        public  async Task<IActionResult> GetTodoItems()
        {
            var items = await _context.TodoItems.ToArrayAsync();
            if (items == null || items.Length==0)
            {
                return NoContent();
            }
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) 
            { 
                return NoContent();
            }
            return Ok(todoItem);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTodo(TodoItem todo)
        {
            if(todo == null)
            {
                return BadRequest();
            }
            _context.TodoItems.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoItem), new { id= todo.id }, todo);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(TodoItem todo, int id)
        {
            //var item = await _context.TodoItems.FindAsync(id);
            if (id != todo.id)
            {
                return BadRequest();
            }
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if(item == null)
            {
                return BadRequest();
            }
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
