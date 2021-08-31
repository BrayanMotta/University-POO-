using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace University.BL.Models
{
    [Table("OfficeAssignment", Schema = "dbo")]
    public class OfficeAssignment
    {
        [Key]
        public int InstructorID { get; set; }
        public string Location { get; set; }
    }
}
