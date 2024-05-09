using StudentManagement_BYMongoDB.Models;

namespace StudentManagement_BYMongoDB.Services
{
    public interface IStudentServices
    {
        List<Students> Get();
        Students Get(string Id);
        Students Create(Students student);
        void Update(string Id, Students student);
        void Remove(string Id);
    }
}
