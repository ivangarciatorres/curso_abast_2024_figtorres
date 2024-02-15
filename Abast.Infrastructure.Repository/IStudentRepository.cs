using Abast.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abast.Infrastructure.Repository
{
    public interface IStudentRepository
    {
        Task<Student> Add(Student student);
        Task<List<Student>> GetAll();
    }
}
