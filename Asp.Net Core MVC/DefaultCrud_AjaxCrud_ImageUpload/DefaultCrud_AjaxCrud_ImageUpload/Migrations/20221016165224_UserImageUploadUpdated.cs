using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefaultCrud_AjaxCrud_ImageUpload.Migrations
{
    public partial class UserImageUploadUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Users",
                type: "TEXT",
                maxLength: 255,
                nullable: true,
                defaultValue:"Noimage.jpg"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Users");
        }
    }
}
