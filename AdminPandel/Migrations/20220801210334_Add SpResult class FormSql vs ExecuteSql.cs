using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminPandel.Migrations
{
    public partial class AddSpResultclassFormSqlvsExecuteSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = "CREATE PROC dbo.GetStudents @FirstName nvarchar(255) AS " +
                "SELECT FirstName,LastName,Age,CourseName" +
                "FROM Students s   " +
                "INNER JOIN CourseStudent cs ON cs.studentsid = s.id" +
                "INNER JOIN courses c ON c.courseid = cs.coursescourseid" +
                "WHERE s.Firstname like @FirstName + '%'; ";

            migrationBuilder.Sql(sql);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql = "DROP PROCEDURE dbo.GetStudents";
            migrationBuilder.Sql(sql);
        }
    }
}
