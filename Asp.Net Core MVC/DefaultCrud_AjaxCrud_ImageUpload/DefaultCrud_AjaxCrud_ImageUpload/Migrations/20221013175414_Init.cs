using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefaultCrud_AjaxCrud_ImageUpload.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Firstname = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Lastname = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Locked = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
