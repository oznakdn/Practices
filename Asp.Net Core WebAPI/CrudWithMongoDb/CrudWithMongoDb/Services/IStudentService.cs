using CrudWithMongoDb.Entities;

namespace CrudWithMongoDb.Services
{
    public interface IStudentService
    {
        List<Student> GetStudents();
        Student GetStudent(string id);
        Student Create(Student student);
        void Update(string id, Student student);
        void Remove (string id);
    }
}
