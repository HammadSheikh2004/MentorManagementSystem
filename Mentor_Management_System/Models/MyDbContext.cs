using Microsoft.EntityFrameworkCore;

namespace Mentor_Management_System.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserInfoModel> UserInfo { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<FacultyModel> Faculties { get; set; }
    }
}
