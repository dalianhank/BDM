using Microsoft.EntityFrameworkCore.Migrations;

namespace BDM.Data.Migrations
{
    public partial class EmailRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Email_Brokers_ParentClientName_ParentNPN1",
                table: "Email");

            migrationBuilder.DropIndex(
                name: "IX_Email_ClientName",
                table: "Email");

            migrationBuilder.DropIndex(
                name: "IX_Email_ParentClientName_ParentNPN1",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "ParentClientName",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "ParentNPN1",
                table: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "ParentNPN",
                table: "Email",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Email",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Email_ClientName_ParentNPN_EmailAddressType",
                table: "Email",
                columns: new[] { "ClientName", "ParentNPN", "EmailAddressType" });

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Brokers_ClientName_ParentNPN",
                table: "Email",
                columns: new[] { "ClientName", "ParentNPN" },
                principalTable: "Brokers",
                principalColumns: new[] { "ClientName", "NPN" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Email_Brokers_ClientName_ParentNPN",
                table: "Email");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Email_ClientName_ParentNPN_EmailAddressType",
                table: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "ParentNPN",
                table: "Email",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Email",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ParentClientName",
                table: "Email",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParentNPN1",
                table: "Email",
                type: "character varying(30)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Email_ClientName",
                table: "Email",
                column: "ClientName");

            migrationBuilder.CreateIndex(
                name: "IX_Email_ParentClientName_ParentNPN1",
                table: "Email",
                columns: new[] { "ParentClientName", "ParentNPN1" });

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Brokers_ParentClientName_ParentNPN1",
                table: "Email",
                columns: new[] { "ParentClientName", "ParentNPN1" },
                principalTable: "Brokers",
                principalColumns: new[] { "ClientName", "NPN" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
