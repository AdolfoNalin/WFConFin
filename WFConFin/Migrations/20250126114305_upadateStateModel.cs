using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WFConFin.Migrations
{
    /// <inheritdoc />
    public partial class upadateStateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_State_StateSigla",
                table: "City");

            migrationBuilder.DropIndex(
                name: "IX_City_StateSigla",
                table: "City");

            migrationBuilder.RenameColumn(
                name: "Sigla",
                table: "State",
                newName: "Acronym");

            migrationBuilder.AddColumn<string>(
                name: "StateAcronym",
                table: "City",
                type: "character varying(2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_City_StateAcronym",
                table: "City",
                column: "StateAcronym");

            migrationBuilder.AddForeignKey(
                name: "FK_City_State_StateAcronym",
                table: "City",
                column: "StateAcronym",
                principalTable: "State",
                principalColumn: "Acronym");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_State_StateAcronym",
                table: "City");

            migrationBuilder.DropIndex(
                name: "IX_City_StateAcronym",
                table: "City");

            migrationBuilder.DropColumn(
                name: "StateAcronym",
                table: "City");

            migrationBuilder.RenameColumn(
                name: "Acronym",
                table: "State",
                newName: "Sigla");

            migrationBuilder.CreateIndex(
                name: "IX_City_StateSigla",
                table: "City",
                column: "StateSigla");

            migrationBuilder.AddForeignKey(
                name: "FK_City_State_StateSigla",
                table: "City",
                column: "StateSigla",
                principalTable: "State",
                principalColumn: "Sigla",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
