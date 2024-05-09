using MongoDB.Driver;
using StudentManagement_BYMongoDB.Models;

namespace StudentManagement_BYMongoDB.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly IMongoCollection<Students> _student;

        public StudentServices( IStudentStoreDatabaseSettings settings, IMongoClient mongoClient )
        {
            //var settings = IStudentStoreDatabaseSettings)
            var dataBase = mongoClient.GetDatabase(settings.DatabaseName);
            _student = dataBase.GetCollection<Students>(settings.StudentCoursesCollectionName);

        }
        public Students Create(Students student)
        {
            _student.InsertOne(student);

            return student;
        }

        public List<Students> Get()
        {
            return _student.Find(students =>  true).ToList();
        }

        public Students Get(string Id)
        {
            return _student.Find(students => students.Id == Id ).FirstOrDefault();
        }

        public void Remove(string Id)
        {
            _student.DeleteOne(students => students.Id == Id);
        }

        public void Update(string Id, Students student)
        {
            _student.ReplaceOne(students => students.Id == Id, student);
        }
    }
}
