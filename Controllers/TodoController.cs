using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApi.DTOs;
using TodoListApi.Models;
using TodoListApi.Repositories;

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;

        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            var todoItems = await _repository.GetAllAsync();
            var todoDTOs = todoItems.Select(item => new TodoItemDTO
            {
                Id = item.Id,
                Title = item.Title,
                IsComplete = item.IsComplete
            });

            return Ok(todoDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(int id)
        {
            var todoItem = await _repository.GetByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            var todoDTO = new TodoItemDTO
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                IsComplete = todoItem.IsComplete
            };

            return Ok(todoDTO);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoDTO)
        {
            var todoItem = new TodoItem
            {
                Title = todoDTO.Title,
                IsComplete = todoDTO.IsComplete
            };

            await _repository.AddAsync(todoItem);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, TodoItemDTO todoDTO)
        {
            if (id != todoDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _repository.GetByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Title = todoDTO.Title;
            todoItem.IsComplete = todoDTO.IsComplete;

            await _repository.UpdateAsync(todoItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _repository.GetByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
