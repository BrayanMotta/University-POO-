using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace University.BL.Models
{
    [Table("Department", Schema = "dbo")]
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }

        public Instructor Instructor { get; set; }
    }
}
