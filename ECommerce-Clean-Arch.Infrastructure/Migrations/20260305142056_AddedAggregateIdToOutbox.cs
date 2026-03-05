using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce_Clean_Arch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAggregateIdToOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AggregateId",
                table: "OutboxMessages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AggregateId",
                table: "OutboxMessages");
        }
    }
}