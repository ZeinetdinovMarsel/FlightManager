using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketServices",
                table: "TicketServices");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TicketServices",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketServices",
                table: "TicketServices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TicketServices_TicketId",
                table: "TicketServices",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketServices",
                table: "TicketServices");

            migrationBuilder.DropIndex(
                name: "IX_TicketServices_TicketId",
                table: "TicketServices");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TicketServices",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketServices",
                table: "TicketServices",
                columns: new[] { "TicketId", "ServiceId" });
        }
    }
}
