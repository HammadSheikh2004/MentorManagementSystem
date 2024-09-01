using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Mentor_Management_System.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Mentor_Management_SystemUser class
public class Mentor_Management_SystemUser : IdentityUser
{
    [Required]
    [DisplayName("FirstName")]
    public string FirstName { get; set; }

    [Required]
    [DisplayName("LastName")]
    public string LastName { get; set; }
    [Required]
    [DisplayName("Phone")]
    public string phone { get; set; }
}

