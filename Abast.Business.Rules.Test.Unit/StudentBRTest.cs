using Abast.Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abast.Business.Rules.Test.Unit
{

    [TestClass]
    public class StudentBRTest
    {

         
        private Guid seed; 

        [TestInitialize]
        public void Initialize()
        {
            this.seed = Guid.NewGuid();
             
        }

        [TestCleanup]
        public void Cleanup()
        { 
        }

        [DataTestMethod]
        [DataRow("María", "García Pérez", "1995-03-10", 28)]
        [DataRow("Carlos", "Martínez López", "1998-08-15", 25)]
        [DataRow("Laura", "Fernández Rodríguez", "1990-05-20", 33)]
        [DataRow("Javier", "Sánchez González", "1987-11-05", 36)]
        [DataRow("Ana", "López Martín", "2002-02-18", 21)]
        public async Task Add_Student_Should_Return_Updated_Student(string name, string surname, string birthdate, int expectedAge)
        {
            DateTime bday = DateTime.Parse(birthdate);
            var student = new Student { Id = 1, Name = name, Surname = surname, Birthday = bday, Age = expectedAge };
            var mockStudentRules = new Mock<IStudentBR>();

            mockStudentRules.Setup(x => x.Add(It.IsAny<Student>()))
                            .ReturnsAsync((Student s) => s);

            mockStudentRules.Setup(x => x.GetAll())
                            .ReturnsAsync(new List<Student> { student });

            var _brules = mockStudentRules.Object;


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

            var mockStudentRules = new Mock<IStudentBR>();

            mockStudentRules.Setup(x => x.Add(It.IsAny<Student>()))
                            .ReturnsAsync((Student s) => s);

            mockStudentRules.Setup(x => x.GetAll())
                            .ReturnsAsync(students.ToList());

            var _brules = mockStudentRules.Object;

            Assert.IsNotNull(students);
            Student randomStudent = students.First();

            var allStudents = await _brules.GetAll();

            Assert.IsNotNull(allStudents);
            Assert.IsTrue(allStudents.Count > 3);
            Assert.IsTrue(allStudents.Contains(randomStudent));
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
