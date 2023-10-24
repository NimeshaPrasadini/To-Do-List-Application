using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListApplication.Models
{
    public class ToDo
    {
        [Key]
        public int TaskID { get; set; }

        [Required(ErrorMessage = "Task Name is required.")]
        [MaxLength(150), Column(TypeName = "nvarchar(150)")]
        public string TaskName { get; set; } = string.Empty;

        [MaxLength(400), Column(TypeName = "nvarchar(400)")]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; } = DateTime.Now;

        public string PriorityId { get; set; } = string.Empty;
        [ValidateNever]
        public Priority Priority { get; set; } = null!;

        [Required(ErrorMessage = "Please select the task status.")]
        public string StatusId { get; set; } = string.Empty;
        [ValidateNever]       
        public Status Status { get; set; } = null!;

        public bool Overdue => (StatusId == "notstarted" || StatusId == "inprogress") && DueDate < DateTime.Today;

    }
}
