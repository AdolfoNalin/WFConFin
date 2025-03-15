using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WFConFin.Migrations
{
    /// <inheritdoc />
    public partial class AccountModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Velue",
                table: "Account",
                newName: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Account",
                newName: "Velue");
        }
    }
}
