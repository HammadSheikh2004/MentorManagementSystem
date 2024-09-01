using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentor_Management_System.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_Id { get; set; }
        [Required]
        [StringLength(150)]
        public string User_First_Name { get; set; }
        [Required]
        [StringLength(150)]
        public string User_Last_Name { get; set; }
        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string User_Email { get; set; }
        [Required]
        [StringLength(150)]
        public string User_Phone { get; set; }
        public string APF_Challan { get; set; } 
        public string TF_Challan { get; set; }
        public string Admit_Card { get; set; } 
        public string WellCome_Letter { get; set; }

    }
}
