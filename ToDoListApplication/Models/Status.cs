using System.ComponentModel.DataAnnotations;

namespace ToDoListApplication.Models
{
    public class Status
    {
        [Key]
        public string StatusId { get; set; } = string.Empty;

        public string StatusName { get; set; } = string.Empty;
    }
}
