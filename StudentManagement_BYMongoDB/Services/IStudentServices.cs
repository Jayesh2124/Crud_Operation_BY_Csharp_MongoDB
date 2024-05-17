using StudentManagement_BYMongoDB.Models;

namespace StudentManagement_BYMongoDB.Services
{
    public interface IStudentServices
    {
        List<Students> Get();
        Students Get(string Id);
        Students Create(Students student);
        List<Students> CreateMultipleStudents(List<Students> students);
        void Update(string Id, Students student);
        void Remove(string Id);
    }
}
