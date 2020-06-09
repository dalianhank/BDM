using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BDM.Data.Migrations
{
    public partial class InitalCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Brokers",
                columns: table => new
                {
                    ClientName = table.Column<string>(nullable: false),
                    NPN = table.Column<string>(maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    Suffix = table.Column<string>(nullable: true),
                    SSN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokers", x => new { x.ClientName, x.NPN });
                    table.ForeignKey(
                        name: "FK_Brokers_Client_ClientName",
                        column: x => x.ClientName,
                        principalTable: "Client",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientName = table.Column<string>(nullable: true),
                    ParentClientName = table.Column<string>(nullable: false),
                    ParentNPN1 = table.Column<string>(nullable: false),
                    ParentNPN = table.Column<string>(nullable: true),
                    EmailAddressType = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Email_Client_ClientName",
                        column: x => x.ClientName,
                        principalTable: "Client",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Email_Brokers_ParentClientName_ParentNPN1",
                        columns: x => new { x.ParentClientName, x.ParentNPN1 },
                        principalTable: "Brokers",
                        principalColumns: new[] { "ClientName", "NPN" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Email_ClientName",
                table: "Email",
                column: "ClientName");

            migrationBuilder.CreateIndex(
                name: "IX_Email_ParentClientName_ParentNPN1",
                table: "Email",
                columns: new[] { "ParentClientName", "ParentNPN1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "Brokers");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
