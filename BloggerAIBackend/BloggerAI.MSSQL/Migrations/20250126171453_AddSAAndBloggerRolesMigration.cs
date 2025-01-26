using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloggerAI.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddSAAndBloggerRolesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Roles (Name) VALUES ('SA');");
            migrationBuilder.Sql("INSERT INTO Roles (Name) VALUES ('Blogger');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Roles WHERE Name IN ('SA', 'Blogger');");
        }
    }
}
