using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce_Clean_Arch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedVersionToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "AspNetUsers");
        }
    }
}