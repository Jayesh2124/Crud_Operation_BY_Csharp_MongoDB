using Microsoft.VisualBasic;
using MongoDB.Driver;
using StudentManagement_BYMongoDB.Models;

namespace StudentManagement_BYMongoDB.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly IMongoCollection<Students> _student;
        public StudentServices(IStudentStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            //var settings = IStudentStoreDatabaseSettings)
            var dataBase = mongoClient.GetDatabase(settings.DatabaseName);
            _student = dataBase.GetCollection<Students>(settings.StudentCoursesCollectionName);
        }

        /* Data Creating Methods  */
        public Students Create(Students student)
        {
            _student.InsertOne(student);
            return student;
        }
        public List<Students> CreateMultipleStudents(List<Students> students)
        {
            _student.InsertMany(students);
            return students;
        }

        /* Getting data from MongoDB */
        public List<Students> Get()
        {
            return _student.Find(students => true).ToList();
        }
        public Students Get(string Id)
        {
            return _student.Find(students => students.Id == Id).FirstOrDefault();
        }

        public List<Students> getFilterdStudents()
        {
            // TODO: Create a new variable named `filter`below:
            var filter = Builders<Students>.
                Filter.Eq("gender", "male");

            // TODO: Create a new variable named `documents`below:
            var documents = _student.Find(filter).ToList();
            return documents;
        }

        public List<Students> GetGraduatedStudents()
        {
            var filter = Builders<Students>.Filter.Eq("graduated", true);


            var result = _student.Aggregate()
                                .Match(filter)
                                .SortByDescending(s => s.Age);
            return result.ToList();
        }

        public IList<dynamic> GetStudentsDetailsOnly()
        {
            var matchBalanceStage = Builders<Students>.Filter.Lt(user => user.Age, 1500);
            var matchAccountStage = Builders<Students>.Filter.Eq(user => user.isGraduated, true);

            var projectionStage = Builders<Students>.Projection.Expression(a =>
                new {
                    StudentId = a.Id,
                    Graduated = a.isGraduated == true ? "Graduated" : "Student",
                    Age = a.Age,
                    Adult = a.Age < 18 ? "Teenager" : "Adult"
                });

            var aggregate = _student.Aggregate()
                           .Match(matchBalanceStage)
                           .Match(matchAccountStage)
                           .SortByDescending(a => a.Age)
                           .Project(projectionStage);

            return aggregate.ToList<dynamic>();
        }


        /* Data Removeing Methods  */
        public void Remove(string Id)
        {
            _student.DeleteOne(students => students.Id == Id);
        } 
        
        public void RemoveMany(string Id)
        {
            // using filter veriable
            //var filter = Builders<Students>.Filter.Lt("age", 20);
            //_student.DeleteOne(filter);

            _student.DeleteMany(s => s.Age < 20);

        }

        /* Data Updting Methods  */
        public void Update(string Id, Students student)
        {
            // using Replace One Method
            //_student.ReplaceOne(students => students.Id == Id, student);

            // using UpdateOne Method with seprate logic
            var filter = Builders<Students>.Filter.Eq("age", 20);
            var update = Builders<Students>.Update.Set("graduated", false);
            _student.UpdateOne(filter, update);

        }

        public async void UpdateManyStudentsGraduation()
        {
            var filter = Builders<Students>.Filter.Eq("age", 30);
            var update = Builders<Students>.Update.Set("graduated", true);
            var result = await _student.UpdateManyAsync(filter, update);

        }
    }
}
