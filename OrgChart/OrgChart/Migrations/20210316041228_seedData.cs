using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChart.Migrations
{
    public partial class seedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "FirstName", "LastName", "Email", "Phone", "Office", "ManagerId" },
                values: new object[,]
                {
                    { 1,  "Uriel", "Fuentes", "mail@test.com", "1234-567-890", "4th Building, 403",  0},
                    {2, "Juan", "Perez",  "mail@test.com", "1234-567-890", "4th Building, 403",  1},
                    {3, "Rosa", "Martinez",  "mail@test.com",  "1234-567-890", "4th Building, 403", 1},
                    {4,  "Pedro", "Gomez","mail@test.com","1234-567-890","4th Building, 403",  2},
                    {5,"Jose",  "Gonzalez", "mail@test.com",  "1234-567-890", "4th Building, 403", 2}
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
