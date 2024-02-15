using Abast.Common.Model;
using System.Xml.Linq;

namespace Abast.Infrastructure.Repository.Tests
{
    [TestClass]
    public class StudentRepositoryTests
    {

        private IStudentRepository _repository; // Instancia de la implementación de IStudentRepository
        private Guid seed;
        private string tempDirectory;

        [TestInitialize]
        public void Initialize()
        {
            this.seed = Guid.NewGuid();
            this.tempDirectory = Directory.CreateTempSubdirectory("abast_test_" + this.seed.ToString().Substring(0, 5)).FullName;
            this._repository = new StudentRepository(this.tempDirectory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(this.tempDirectory))
            {
                File.Delete(this.tempDirectory);
            }
        }

        [DataTestMethod]
        [DataRow("María", "García Pérez", "1995-03-10", 29)]
        [DataRow("Carlos", "Martínez López", "1998-08-15", 23)]
        [DataRow("Laura", "Fernández Rodríguez", "1990-05-20", 32)]
        [DataRow("Javier", "Sánchez González", "1987-11-05", 34)]
        [DataRow("Ana", "López Martín", "2002-02-18", 19)]
        public async Task Add_Student_Should_Return_Updated_Student(string name, string surname, string birthdate, int age)
        {
            DateTime bday = DateTime.Parse(birthdate);
            var student = new Student { Name = name, Surname = surname, Birthday = bday, Age = age };

            var addedStudent = await _repository.Add(student);

            Assert.IsNotNull(addedStudent);
            Assert.IsTrue(addedStudent.Id > 0);

            var allStudents = await _repository.GetAll();

            Assert.IsNotNull(allStudents);
            Assert.IsTrue(allStudents.Count > 0);
            Assert.IsTrue(allStudents.Contains(addedStudent));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public async Task GetAll_Should_Return_List_Of_Students(IEnumerable<Student> students)
        {
            Assert.IsNotNull(students);
            Student addedStudent = null;

            foreach (var student in students)
            {
                addedStudent = await _repository.Add(student);
                Assert.IsNotNull(addedStudent);
                Assert.IsTrue(addedStudent.Id > 0);
            }
             

            var allStudents = await _repository.GetAll();

            Assert.IsNotNull(allStudents);
            Assert.IsTrue(allStudents.Count > 3);
            Assert.IsTrue(allStudents.Contains(addedStudent));
        }

        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { new List<Student>()
            {
                new Student { Name =  "María", Surname ="García Pérez",Birthday = DateTime.Parse("1995-03-10"), Age =29},
                new Student{Name = "Carlos", Surname="Martínez López", Birthday = DateTime.Parse("1998-08-15"), Age = 23 },
                new Student{Name = "Laura", Surname="Fernández Rodríguez", Birthday = DateTime.Parse("1990-05-20"), Age = 32 },
                new Student{Name = "Javier",Surname ="Sánchez González", Birthday = DateTime.Parse("1987-11-05"), Age = 34 },
                new Student{Name = "Ana", Surname="López Martín", Birthday = DateTime.Parse("2002-02-18"), Age = 19 }
            }};
        }
    }

}