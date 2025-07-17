using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandiMaker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumOfChildforcomment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumOfChildren",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfChildren",
                table: "Comments");
        }
    }
}
