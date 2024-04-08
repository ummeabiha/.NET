using LearnDapper.Model;
using System.Threading.Tasks;

namespace LearnDapper.Repo
{
    public interface IStudentRepo
    {
        Task<List<Students>> GetAll();

        Task<Students> GetById(int student_Id);

        Task<string> AddRecord(Students student);

        Task<string> UpdateRecord(Students student, int student_Id);
        Task<string> UpdateRecordUsingPatch(Students student, int student_Id);

        //Task<string> DeleteRecord(int student_Id);
        Task<Students> DeleteRecord(int student_Id);
    }
}
