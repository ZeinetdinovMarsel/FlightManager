using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Services : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketServices",
                table: "TicketServices");

            migrationBuilder.DropIndex(
                name: "IX_TicketServices_TicketId",
                table: "TicketServices");

            migrationBuilder.DropColumn(
                name: "ServiceCost",
                table: "TicketServices");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "TicketServices");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TicketServices",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "TicketServices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketServices",
                table: "TicketServices",
                columns: new[] { "TicketId", "ServiceId" });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketServices_ServiceId",
                table: "TicketServices",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketServices_Services_ServiceId",
                table: "TicketServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketServices_Services_ServiceId",
                table: "TicketServices");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketServices",
                table: "TicketServices");

            migrationBuilder.DropIndex(
                name: "IX_TicketServices_ServiceId",
                table: "TicketServices");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "TicketServices");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TicketServices",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<float>(
                name: "ServiceCost",
                table: "TicketServices",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "TicketServices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketServices",
                table: "TicketServices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TicketServices_TicketId",
                table: "TicketServices",
                column: "TicketId");
        }
    }
}
