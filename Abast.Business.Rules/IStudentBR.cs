using Abast.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abast.Business.Rules
{
    public interface IStudentBR
    {
        Task<Student> Add(Student student);
        Task<List<Student>> GetAll();
    }
}
