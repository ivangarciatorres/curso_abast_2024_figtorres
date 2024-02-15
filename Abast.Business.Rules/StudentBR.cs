using Abast.Common.Model;
using Abast.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abast.Business.Rules
{
    public class StudentBR : IStudentBR
    {
        private IStudentRepository repository;

        [Log]
        public StudentBR(string fileDirectory = null)
        {
            this.repository = new StudentRepository(fileDirectory);
        }

        [LogDetail]

        public async Task<Student> Add(Student student)
        {
            await CalculateStudentAge(student);
            return await this.repository.Add(student);
        }

        [Log]
        public async Task<List<Student>> GetAll()
        {
            return await this.repository.GetAll();
        }

        [Log]
        private Task CalculateStudentAge(Student student)
        {
            return Task.Run(() =>
            {
                student.Age = DateTime.Now.Year - student.Birthday.Year;
                if (student.Birthday.Date > DateTime.Now.AddYears(-student.Age)) student.Age--;
            });

        }
    }
}
