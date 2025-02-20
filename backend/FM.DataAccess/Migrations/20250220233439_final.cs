using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FederalDistricts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederalDistricts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FederalDistrictId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Airports_FederalDistricts_FederalDistrictId",
                        column: x => x.FederalDistrictId,
                        principalTable: "FederalDistricts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionEntity",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    PermissionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionEntity", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissionEntity_PermissionEntity_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "PermissionEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionEntity_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleEntity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleEntity", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoleEntity_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleEntity_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Destination = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AvailableSeats = table.Column<int>(type: "integer", nullable: false),
                    AirplanePhotoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AirportId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Airports_AirportId",
                        column: x => x.AirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketType = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Seat = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FlightId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    TicketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketServices_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FederalDistricts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Центральный федеральный округ" },
                    { 2, "Северо-Западный федеральный округ" },
                    { 3, "Южный федеральный округ" },
                    { 4, "Северо-Кавказский федеральный округ" },
                    { 5, "Приволжский федеральный округ" },
                    { 6, "Уральский федеральный округ" },
                    { 7, "Сибирский федеральный округ" },
                    { 8, "Дальневосточный федеральный округ" }
                });

            migrationBuilder.InsertData(
                table: "PermissionEntity",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Create" },
                    { 2, "Read" },
                    { 3, "Update" },
                    { 4, "Delete" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Cost", "Name" },
                values: new object[,]
                {
                    { 1, 10.00m, "Meal" },
                    { 2, 20.00m, "Extra Luggage" },
                    { 3, 15.00m, "Priority Boarding" },
                    { 4, 5.00m, "In-Flight Entertainment" },
                    { 5, 12.00m, "Wi-Fi Access" },
                    { 6, 25.00m, "Extra Legroom Seat" },
                    { 7, 30.00m, "Airport Transfer" },
                    { 8, 18.00m, "Travel Insurance" },
                    { 9, 40.00m, "Lounge Access" },
                    { 10, 12.00m, "Special Meal Request" }
                });

            migrationBuilder.InsertData(
                table: "Airports",
                columns: new[] { "Id", "City", "FederalDistrictId", "Name" },
                values: new object[,]
                {
                    { 1, "Москва", 1, "Шереметьево" },
                    { 2, "Москва", 1, "Домодедово" },
                    { 3, "Москва", 1, "Внуково" },
                    { 4, "Санкт-Петербург", 2, "Пулково" },
                    { 5, "Ростов-на-Дону", 3, "Платов" },
                    { 6, "Сочи", 3, "Сочи" },
                    { 7, "Минеральные Воды", 4, "Минеральные Воды" },
                    { 8, "Казань", 5, "Казань" },
                    { 9, "Нижний Новгород", 5, "Нижний Новгород" },
                    { 10, "Курган", 6, "Курган" },
                    { 11, "Екатеринбург", 6, "Екатеринбург" },
                    { 12, "Новосибирск", 7, "Толмачёво" },
                    { 13, "Красноярск", 7, "Красноярск" },
                    { 14, "Иркутск", 7, "Иркутск" },
                    { 15, "Владивосток", 8, "Владивосток" },
                    { 16, "Хабаровск", 8, "Хабаровск" },
                    { 17, "Якутск", 8, "Якутск" },
                    { 18, "Калининград", 2, "Калининград" },
                    { 19, "Уфа", 5, "Уфа" },
                    { 20, "Челябинск", 6, "Челябинск" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissionEntity",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "AirplanePhotoUrl", "AirportId", "ArrivalTime", "AvailableSeats", "DepartureTime", "Destination", "FlightNumber" },
                values: new object[,]
                {
                    { 1, "https://admspvoskresenskoe.ru/wp-content/uploads/Ezegodno-9-fevrala-v-Rossii-otmecaetsa-Den'-grazdanskoj-aviacii.jpg", 1, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4835), 180, new DateTime(2025, 2, 21, 1, 34, 39, 235, DateTimeKind.Utc).AddTicks(4829), "Санкт-Петербург", "SU1001" },
                    { 2, "https://vesti-ural.ru/wp-content/uploads/2021/09/1590132398_4.jpeg", 1, new DateTime(2025, 2, 21, 4, 34, 39, 235, DateTimeKind.Utc).AddTicks(4878), 150, new DateTime(2025, 2, 21, 2, 34, 39, 235, DateTimeKind.Utc).AddTicks(4877), "Екатеринбург", "SU1002" },
                    { 3, "https://avatars.mds.yandex.net/get-mpic/5221857/img_id9103962167301146378.jpeg/orig", 1, new DateTime(2025, 2, 21, 2, 34, 39, 235, DateTimeKind.Utc).AddTicks(4881), 200, new DateTime(2025, 2, 21, 0, 34, 39, 235, DateTimeKind.Utc).AddTicks(4880), "Сочи", "SU1003" },
                    { 4, "https://wallpapers.com/images/hd/4k-planes-28pfplts81ps0e4h.jpg", 1, new DateTime(2025, 2, 21, 7, 34, 39, 235, DateTimeKind.Utc).AddTicks(4883), 220, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4882), "Владивосток", "SU1004" },
                    { 5, "https://cdn.culture.ru/images/e47f7cd8-4712-5f93-93c7-ae9c96e2492c", 1, new DateTime(2025, 2, 21, 2, 34, 39, 235, DateTimeKind.Utc).AddTicks(4885), 130, new DateTime(2025, 2, 21, 1, 34, 39, 235, DateTimeKind.Utc).AddTicks(4885), "Калининград", "SU1005" },
                    { 6, "https://exclusive.kz/wp-content/uploads/2022/06/126245.jpg", 2, new DateTime(2025, 2, 21, 5, 34, 39, 235, DateTimeKind.Utc).AddTicks(4889), 190, new DateTime(2025, 2, 21, 2, 34, 39, 235, DateTimeKind.Utc).AddTicks(4888), "Новосибирск", "DP2001" },
                    { 7, "https://cdn1.ozone.ru/s3/multimedia-6/6447919386.jpg", 2, new DateTime(2025, 2, 21, 4, 34, 39, 235, DateTimeKind.Utc).AddTicks(4891), 160, new DateTime(2025, 2, 21, 1, 34, 39, 235, DateTimeKind.Utc).AddTicks(4891), "Красноярск", "DP2002" },
                    { 8, "https://habrastorage.org/files/d7f/c1a/c07/d7fc1ac07c244551b7198a5caae9d687.jpg", 2, new DateTime(2025, 2, 21, 8, 34, 39, 235, DateTimeKind.Utc).AddTicks(4893), 210, new DateTime(2025, 2, 21, 4, 34, 39, 235, DateTimeKind.Utc).AddTicks(4892), "Иркутск", "DP2003" },
                    { 9, "https://avatars.mds.yandex.net/get-mpic/5042167/img_id145303521372402621.jpeg/orig", 2, new DateTime(2025, 2, 21, 7, 34, 39, 235, DateTimeKind.Utc).AddTicks(4895), 180, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4894), "Хабаровск", "DP2004" },
                    { 10, "https://s0.rbk.ru/v6_top_pics/media/img/3/57/754755745720573.jpg", 3, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4897), 170, new DateTime(2025, 2, 21, 1, 34, 39, 235, DateTimeKind.Utc).AddTicks(4897), "Ростов-на-Дону", "VP3001" },
                    { 11, "https://cdnstatic.rg.ru/uploads/images/156/67/67/Depositphotos_41367457_m-2015.jpg", 3, new DateTime(2025, 2, 21, 2, 34, 39, 235, DateTimeKind.Utc).AddTicks(4899), 140, new DateTime(2025, 2, 21, 0, 34, 39, 235, DateTimeKind.Utc).AddTicks(4898), "Минеральные Воды", "VP3002" },
                    { 12, "https://cdn1.ozone.ru/s3/multimedia-l/6450210693.jpgg", 3, new DateTime(2025, 2, 21, 4, 34, 39, 235, DateTimeKind.Utc).AddTicks(4901), 200, new DateTime(2025, 2, 21, 2, 34, 39, 235, DateTimeKind.Utc).AddTicks(4900), "Казань", "VP3003" },
                    { 13, "https://www.ixbt.com/img/n1/news/2022/9/6/avia_large.png", 3, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4902), 160, new DateTime(2025, 2, 21, 1, 34, 39, 235, DateTimeKind.Utc).AddTicks(4902), "Нижний Новгород", "VP3004" },
                    { 14, "https://i.pinimg.com/originals/1f/6f/88/1f6f88acd68ba05da46838932162a14b.jpg", 4, new DateTime(2025, 2, 21, 4, 34, 39, 235, DateTimeKind.Utc).AddTicks(4904), 180, new DateTime(2025, 2, 21, 2, 34, 39, 235, DateTimeKind.Utc).AddTicks(4904), "Уфа", "MR4001" },
                    { 15, "https://cdn1.ozone.ru/multimedia/1022239843.jpg", 4, new DateTime(2025, 2, 21, 6, 34, 39, 235, DateTimeKind.Utc).AddTicks(4907), 190, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4906), "Челябинск", "MR4002" },
                    { 16, "https://static.tildacdn.com/tild6666-3933-4737-b836-616635623763/samolety-krasivye-ka.jpg", 4, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4909), 150, new DateTime(2025, 2, 21, 1, 34, 39, 235, DateTimeKind.Utc).AddTicks(4908), "Самара", "MR4003" },
                    { 17, "https://otvet.imgsmail.ru/download/4e9ff0c84d505cbb79586c493d25132e_h-324.jpg", 4, new DateTime(2025, 2, 21, 5, 34, 39, 235, DateTimeKind.Utc).AddTicks(4911), 170, new DateTime(2025, 2, 21, 2, 34, 39, 235, DateTimeKind.Utc).AddTicks(4910), "Пермь", "MR4004" },
                    { 18, "https://img.goodfon.com/original/2388x1668/a/79/cathay-pacific-boeing-777.jpg", 4, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4913), 160, new DateTime(2025, 2, 21, 1, 34, 39, 235, DateTimeKind.Utc).AddTicks(4913), "Волгоград", "MR4005" },
                    { 19, "https://s9.travelask.ru/uploads/post/000/026/337/main_image/facebook-3c16fde96f14711ae2a04a727c4025c1.jpg", 5, new DateTime(2025, 2, 21, 7, 34, 39, 235, DateTimeKind.Utc).AddTicks(4915), 200, new DateTime(2025, 2, 21, 4, 34, 39, 235, DateTimeKind.Utc).AddTicks(4915), "Омск", "NS5001" },
                    { 20, "https://www.atorus.ru/sites/default/files/styles/head_carousel/public/2021-09/131872.jpg.webp?itok=w02d3ccD", 5, new DateTime(2025, 2, 21, 6, 34, 39, 235, DateTimeKind.Utc).AddTicks(4917), 180, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4916), "Барнаул", "NS5002" },
                    { 21, "https://cdn1.ozone.ru/s3/multimedia-i/6449418186.jpg", 5, new DateTime(2025, 2, 21, 5, 34, 39, 235, DateTimeKind.Utc).AddTicks(4919), 190, new DateTime(2025, 2, 21, 2, 34, 39, 235, DateTimeKind.Utc).AddTicks(4918), "Тюмень", "NS5003" },
                    { 22, "https://img5tv.cdnvideo.ru/webp/shared/files/202102/1_1263737.jpg", 5, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4920), 150, new DateTime(2025, 2, 21, 1, 34, 39, 235, DateTimeKind.Utc).AddTicks(4920), "Махачкала", "NS5004" },
                    { 23, "https://static.life.ru/publications/2022/2/25/622639184735.0912.png", 5, new DateTime(2025, 2, 21, 9, 34, 39, 235, DateTimeKind.Utc).AddTicks(4922), 210, new DateTime(2025, 2, 21, 5, 34, 39, 235, DateTimeKind.Utc).AddTicks(4922), "Улан-Удэ", "NS5005" },
                    { 24, "https://i.pinimg.com/originals/5d/47/b3/5d47b3b01b41bbd587043be61b4f5230.jpg", 5, new DateTime(2025, 2, 21, 6, 34, 39, 235, DateTimeKind.Utc).AddTicks(4924), 160, new DateTime(2025, 2, 21, 3, 34, 39, 235, DateTimeKind.Utc).AddTicks(4923), "Сыктывкар", "NS5006" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "FlightId", "Price", "Seat", "TicketType" },
                values: new object[,]
                {
                    { 1, 1, 150f, "A1", 1 },
                    { 2, 1, 300f, "B1", 2 },
                    { 3, 2, 180f, "C1", 1 },
                    { 4, 2, 500f, "D1", 3 },
                    { 5, 3, 160f, "E1", 1 },
                    { 6, 3, 320f, "F1", 2 },
                    { 7, 4, 170f, "G1", 1 },
                    { 8, 4, 550f, "H1", 3 },
                    { 9, 5, 190f, "I1", 1 },
                    { 10, 5, 330f, "J1", 2 }
                });

            migrationBuilder.InsertData(
                table: "TicketServices",
                columns: new[] { "Id", "ServiceId", "TicketId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 1, 2 },
                    { 5, 2, 2 },
                    { 6, 3, 3 },
                    { 7, 4, 4 },
                    { 8, 1, 5 },
                    { 9, 2, 5 },
                    { 10, 5, 5 },
                    { 11, 6, 5 },
                    { 12, 6, 6 },
                    { 13, 7, 7 },
                    { 14, 8, 8 },
                    { 15, 9, 9 },
                    { 16, 10, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airports_FederalDistrictId",
                table: "Airports",
                column: "FederalDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirportId",
                table: "Flights",
                column: "AirportId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionEntity_PermissionId",
                table: "RolePermissionEntity",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightId",
                table: "Tickets",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketServices_ServiceId",
                table: "TicketServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketServices_TicketId",
                table: "TicketServices",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleEntity_UserId",
                table: "UserRoleEntity",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissionEntity");

            migrationBuilder.DropTable(
                name: "TicketServices");

            migrationBuilder.DropTable(
                name: "UserRoleEntity");

            migrationBuilder.DropTable(
                name: "PermissionEntity");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "FederalDistricts");
        }
    }
}
