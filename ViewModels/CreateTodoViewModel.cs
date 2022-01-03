using System.ComponentModel.DataAnnotations;

namespace TodoList.ViewModels
{
    public class CreateTodoViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}