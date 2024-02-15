using Abast.Common.Model;
using Abast.Infrastructure.Repository;

namespace Abast.Business.Rules.Test
{
    [TestClass]
    public class StudentBRTest
    { 


        private IStudentBR _brules; // Instancia de la implementaci�n de IStudentRepository
        private Guid seed;
        private string tempDirectory;

        [TestInitialize]
        public void Initialize()
        {
            this.seed = Guid.NewGuid();
            this.tempDirectory = Directory.CreateTempSubdirectory("abast_test_" + this.seed.ToString().Substring(0, 5)).FullName;
            this._brules = new StudentBR(this.tempDirectory);
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
        [DataRow("Mar�a", "Garc�a P�rez", "1995-03-10", 28)]
        [DataRow("Carlos", "Mart�nez L�pez", "1998-08-15", 25)]
        [DataRow("Laura", "Fern�ndez Rodr�guez", "1990-05-20", 33)]
        [DataRow("Javier", "S�nchez Gonz�lez", "1987-11-05", 36)]
        [DataRow("Ana", "L�pez Mart�n", "2002-02-18", 21)]
        public async Task Add_Student_Should_Return_Updated_Student(string name, string surname, string birthdate, int expectedAge)
        {
            DateTime bday = DateTime.Parse(birthdate);
            var student = new Student { Name = name, Surname = surname, Birthday = bday};

            var addedStudent = await _brules.Add(student);

            Assert.AreEqual(expectedAge, addedStudent.Age);
            Assert.IsNotNull(addedStudent);
            Assert.IsTrue(addedStudent.Id > 0);

            var allStudents = await _brules.GetAll();

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
                addedStudent = await _brules.Add(student);
                Assert.IsNotNull(addedStudent);
                Assert.IsTrue(addedStudent.Id > 0);
            }


            var allStudents = await _brules.GetAll();

            Assert.IsNotNull(allStudents);
            Assert.IsTrue(allStudents.Count > 3);
            Assert.IsTrue(allStudents.Contains(addedStudent));
        }

        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { new List<Student>()
            {
                new Student { Name =  "Mar�a", Surname ="Garc�a P�rez",Birthday = DateTime.Parse("1995-03-10"), Age =29},
                new Student{Name = "Carlos", Surname="Mart�nez L�pez", Birthday = DateTime.Parse("1998-08-15"), Age = 23 },
                new Student{Name = "Laura", Surname="Fern�ndez Rodr�guez", Birthday = DateTime.Parse("1990-05-20"), Age = 32 },
                new Student{Name = "Javier",Surname ="S�nchez Gonz�lez", Birthday = DateTime.Parse("1987-11-05"), Age = 34 },
                new Student{Name = "Ana", Surname="L�pez Mart�n", Birthday = DateTime.Parse("2002-02-18"), Age = 19 }
            }};
        }
    }
}