using Abast.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Abast.Infrastructure.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private string fileName = "\\studentsdata.txt";
        private string fileDirectory = Directory.GetCurrentDirectory();
        private string fileURL = string.Empty;

        [LogDetail]
        public StudentRepository(string fileDirectory = null)
        {
            if (!string.IsNullOrEmpty(fileDirectory))
            {
                this.fileURL = fileDirectory + fileName;
            }
            else
            {
                this.fileURL = this.fileDirectory + fileName;
            }
        }

        [LogDetail]
        public async Task<Student> Add(Student student)
        {
            try
            {
                List<Student> allStudents = await this.GetAll();

                if (allStudents == null)
                    allStudents = new List<Student>();

                Student foundStudent = allStudents?.FirstOrDefault(x => x.Id == student.Id);

                if (foundStudent != null)
                {
                    foundStudent.Surname = student.Surname;
                    foundStudent.Birthday = student.Birthday;
                    foundStudent.Name = student.Name;
                }
                else
                {
                    int? newId = allStudents?.Count > 0 ? allStudents?.Max(x => x.Id) : 0;
                    student.Id = newId.GetValueOrDefault() + 1;

                    allStudents.Add(student);
                }

                File.WriteAllText(fileURL, JsonConvert.SerializeObject(allStudents));

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return student;
        }

        public async Task<List<Student>> GetAll()
        {
            if (!File.Exists(fileURL))
            {
                return new List<Student>();
            }

            try
            {
                using (var reader = File.OpenText(fileURL))
                {
                    var jsonText = await reader.ReadToEndAsync();

                    try
                    {
                        return JsonConvert.DeserializeObject<List<Student>>(jsonText);
                    }
                    catch (JsonSerializationException ex)
                    {
                        Console.WriteLine($"Error al deserializar el archivo: {ex.Message}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo: {ex.Message}");
                throw;
            }
        }
    }
}
