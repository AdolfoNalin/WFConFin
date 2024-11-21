using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WFConFin.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PersonaId",
                table: "Account",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Account_PersonaId",
                table: "Account",
                column: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Persona_PersonaId",
                table: "Account",
                column: "PersonaId",
                principalTable: "Persona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Persona_PersonaId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_PersonaId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "PersonaId",
                table: "Account");
        }
    }
}
