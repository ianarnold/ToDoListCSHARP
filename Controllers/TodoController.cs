using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TodoList.Models;
using TodoList.Data;
using Microsoft.EntityFrameworkCore;
using TodoList.ViewModels;

namespace TodoList.Controllers
{
    [ApiController] // Controller de uma API
    [Route("v1")] // Rota V1
    public class TodoController : ControllerBase
    {
        [HttpGet] // Rota do tipo GET
        [Route("todos")] // localhost:porta/v1/todos
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context)
        {
            var todos = await context
            .Todos
            .AsNoTracking() // Para nao trackear a query
            .ToListAsync(); // Para retornar todos items.

            return Ok(todos);
        }

        [HttpGet] // Rota do tipo GET para apenas um item
        [Route("todos/{id}")] // localhost:porta/v1/todos/{id}   --> Recebe apenas um TODO
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id) // --> Id vindo da rota, é um INT.
        {
            var todo = await context
            .Todos
            .AsNoTracking() // Para nao trackear a query
            .FirstOrDefaultAsync(x => x.Id == id); // Para retornar um item só.

            if (todo == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(todo);
            }
        }

        [HttpPost("todos")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var todo = new Todo
            {
                Date = DateTime.Now,
                Done = false,
                Title = model.Title
            };
            
            try
            {
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created($"v1/todos/{todo.Id}", todo);
            }
            catch (Exception e)
            {
                return BadRequest();
            }  
        }

        [HttpPut("todos/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var todo = await context.Todos
            .FirstOrDefaultAsync(x => x.Id == id);
            
            if (todo == null) 
                return NotFound();

            try
            {
                todo.Title = model.Title;

                context.Todos.Update(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception e)
            {
                return BadRequest();
            } 
        }

        [HttpDelete("todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();

                return Ok("Removido com sucesso");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
        }

    }
}
