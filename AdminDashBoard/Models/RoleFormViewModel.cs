using System.ComponentModel.DataAnnotations;

namespace AdminDashBoard.Models
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage ="Name is Required")]

        public string RoleName { get; set; }
      
    }
}
