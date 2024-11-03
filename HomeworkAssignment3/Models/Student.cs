using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeworkAssignment3.Models // Replace with the actual namespace for your models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; } // Optional: You could also use an enum for Gender

        [MaxLength(20)]
        public string Class { get; set; }

        [Range(0, int.MaxValue)]
        public int Point { get; set; } // Represents the points or credits earned by the student

        // Navigation Properties
        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    }
}
