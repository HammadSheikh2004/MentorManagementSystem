using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Mentor_Management_System.Models
{
    public class UserInfoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_Info_Id { get; set; }
        [Required]
        [StringLength(150)]
        public string School_Name { get; set; }
        [Required]
        [StringLength(150)]
        public string College_Name { get; set; }
        [Required]
        public int School_Marks { get; set; }
        [Required]
        public int College_Marks { get; set; }
        [Required]
        [StringLength(150)]
        public string CNIC { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Course { get; set; }
        [Required]
        public string User_Image { get; set; }
        public int User_Id { get; set; }
        [ForeignKey(nameof(User_Id))]
        public virtual UserModel Users { get; set; }

    }
}
