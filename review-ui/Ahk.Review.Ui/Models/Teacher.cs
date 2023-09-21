using DTOs;

namespace Ahk.Review.Ui.Models;

public class Teacher
{
    public Teacher()
    {

    }
    public Teacher(TeacherDTO teacherDTO)
    {
        this.Id = teacherDTO.Id;
        this.Name = teacherDTO.Name;
        this.Neptun = teacherDTO.Neptun;
        this.GithubUser = teacherDTO.GithubUser;
        this.EduEmail = teacherDTO.EduEmail;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Neptun { get; set; }
    public string EduEmail { get; set; }
    public string GithubUser { get; set; }
}
