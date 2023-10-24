using System.ComponentModel.DataAnnotations;

namespace ToDoListApplication.Models
{
    public class Priority
    {
        [Key]
        public string PriorityId { get; set; } = string.Empty;

        public string PriorityName { get; set; } = string.Empty;
    }
}
