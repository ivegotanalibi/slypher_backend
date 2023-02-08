using Microsoft.AspNetCore.Mvc;
using ToDoProj.Migrations;
using ToDoProj.Models;

namespace ToDoProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToDoController(ApplicationDbContext context)
        {
            _context = context;

            if (_context.Todos.Count() == 0)
            {
                // Create a new Todo if collection is empty,
                // which means you can't delete all Todos.
                _context.Todos.Add(new ToDo { Title = "Item1" , TaskOwner = "Dummyowner" });
                _context.SaveChanges();
            }
        }

        // GET: api/Todos
        [HttpGet]
        public ActionResult<List<ToDo>> GetAll()
        {
            return _context.Todos.ToList();
        }

        // GET: api/Todos/5
        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<ToDo> GetById(int id)
        {
            var todo = _context.Todos.Find(id);
            if (todo == null)
            {
                return NotFound();
            }
            return todo;
        }

        // POST: api/Todos
        [HttpPost]
        public ActionResult<ToDo> Create(ToDo todo)
        {
            _context.Todos.Add(todo);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = todo.Id }, todo);
        }

        // PUT: api/Todos/5
        [HttpPut("{id}")]
        public ActionResult Update(int id, ToDo todo)
        {
            var existingTodo = _context.Todos.Find(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            existingTodo.Title = todo.Title;
            existingTodo.IsComplete = todo.IsComplete;

            _context.Todos.Update(existingTodo);
            _context.SaveChanges();
            return NoContent();
        }
             
        // DELETE: api/Todos/5        
        [HttpDelete("{id}")]
        public ActionResult<ToDo> Delete(int id)
        {
            var existingTodo = _context.Todos.Find(id);
            if (existingTodo == null)
            {
                return NotFound();
            }
             
            _context.Todos.Remove(existingTodo);
            _context.SaveChanges();
            return NoContent();
        }  
    }
}
