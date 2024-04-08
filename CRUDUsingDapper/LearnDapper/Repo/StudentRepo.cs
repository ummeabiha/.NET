using Dapper;
using LearnDapper.Model;
using LearnDapper.Model.Data;

namespace LearnDapper.Repo
{
    public class StudentRepo: IStudentRepo
    {
        private readonly DapperDBContext context;
        public StudentRepo(DapperDBContext context) {
            this.context = context;
        }

        public async Task<List<Students>> GetAll(){
            string query = "SELECT * FROM students";
            using(var connection= this.context.CreateConnection())
            {
                var stulist= await connection.QueryAsync<Students>(query);
                return stulist.ToList();
            }
        }


        public async Task <Students> GetById(int student_Id)
        {
            string query = "SELECT * FROM students WHERE student_Id=@student_Id";
            using (var connection = this.context.CreateConnection())
            {
                var stulist = await connection.QueryFirstOrDefaultAsync<Students>(query, new { student_Id });
                return stulist;
            }
        }

        public async Task <string> AddRecord(Students student)
        {
            string response = string.Empty;
            string query = "INSERT INTO students (student_Name, student_Age, student_Domain) VALUES (@student_Name, @student_Age, @student_Domain)";
            var parameters = new DynamicParameters();
            parameters.Add("student_Name", student.student_Name, System.Data.DbType.String);
            parameters.Add("student_Age", student.student_Age, System.Data.DbType.String);
            parameters.Add("student_Domain", student.student_Domain, System.Data.DbType.String);

            using (var connection = this.context.CreateConnection())
            {
                var stulist = await connection.ExecuteAsync(query, parameters);
                response= "pass";
            }

            return response;
        }


        public async Task<string> UpdateRecord(Students student, int student_Id)
        {
            string response = string.Empty;
            string query = "UPDATE students set student_Name=@student_Name, student_Age=@student_Age, student_Domain=@student_Domain WHERE student_Id=@student_Id";
            var parameters = new DynamicParameters();
            parameters.Add("student_Id", student.student_Id, System.Data.DbType.Int32);
            parameters.Add("student_Name", student.student_Name, System.Data.DbType.String);
            parameters.Add("student_Age", student.student_Age, System.Data.DbType.String);
            parameters.Add("student_Domain", student.student_Domain, System.Data.DbType.String);

            using (var connection = this.context.CreateConnection())
            {
                var stulist = await connection.ExecuteAsync(query, parameters);
                response = "pass";
            }

            return response;
        }

        public async Task<string> UpdateRecordUsingPatch(Students student, int student_Id){
            string response = string.Empty;
            string query = "UPDATE students set student_Name=@student_Name, student_Age=@student_Age, student_Domain=@student_Domain WHERE student_Id=@student_Id";
            var parameters = new DynamicParameters();
            parameters.Add("student_Id", student.student_Id, System.Data.DbType.Int32);
            parameters.Add("student_Name", student.student_Name, System.Data.DbType.String);
            parameters.Add("student_Age", student.student_Age, System.Data.DbType.String);
            parameters.Add("student_Domain", student.student_Domain, System.Data.DbType.String);

            using (var connection = this.context.CreateConnection())
            {
                var stulist = await connection.ExecuteAsync(query, parameters);
                response = "pass";
            }

            return response;
        }

        /*
        public async Task<string> DeleteRecord(int student_Id)
        {
            string response = string.Empty;
            string query = "DELETE FROM students WHERE student_Id= @student_Id";

            using (var connection = this.context.CreateConnection())
            {
                var stulist = await connection.ExecuteAsync(query, new {student_Id});
                response = "pass";
            }

            return response;
        }
        */

        public async Task<Students> DeleteRecord(int student_Id)
        {
            try
            {
                var delRec = await GetById(student_Id);
                //var delRec= await GetById(student_Id);
                string query = "DELETE FROM students WHERE student_Id = @student_Id";

                using (var connection = this.context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new { student_Id });
                    //Console.WriteLine($"One Record has been deleted successfully");
                    //return delRec;
                    return delRec;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; 
            }
        }
    }
}
