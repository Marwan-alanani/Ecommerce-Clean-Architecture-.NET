using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce_Clean_Arch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamedVersionInOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Version",
                table: "OutboxMessages",
                newName: "AggregateVersion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AggregateVersion",
                table: "OutboxMessages",
                newName: "Version");
        }
    }
}