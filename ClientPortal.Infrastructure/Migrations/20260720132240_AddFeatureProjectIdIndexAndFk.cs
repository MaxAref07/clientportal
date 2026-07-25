using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatureProjectIdIndexAndFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Features_ProjectId",
                table: "Features",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Projects_ProjectId",
                table: "Features",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Projects_ProjectId",
                table: "Features");

            migrationBuilder.DropIndex(
                name: "IX_Features_ProjectId",
                table: "Features");
        }
    }
}
