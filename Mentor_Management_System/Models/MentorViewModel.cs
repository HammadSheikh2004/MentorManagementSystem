namespace Mentor_Management_System.Models
{
    public class MentorViewModel
    {
        public UserModel User { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public int FieldCount { get; set; }
        public string CourseName { get; set; }
        public StudentModel StudentModel { get; set; }
        public IEnumerable<StudentModel> Students { get; set; }

    }
}
