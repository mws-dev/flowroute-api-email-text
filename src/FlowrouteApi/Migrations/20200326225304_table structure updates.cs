using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlowrouteApi.Migrations
{
    public partial class tablestructureupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingRoute",
                table: "OutgoingRoute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomingRoute",
                table: "IncomingRoute");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "OutgoingRoute",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OutgoingRoute",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "IncomingRoute",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "IncomingRoute",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "IncomingRoute",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingRoute",
                table: "OutgoingRoute",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomingRoute",
                table: "IncomingRoute",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingRoute_Email",
                table: "OutgoingRoute",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingRoute",
                table: "OutgoingRoute");

            migrationBuilder.DropIndex(
                name: "IX_OutgoingRoute_Email",
                table: "OutgoingRoute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomingRoute",
                table: "IncomingRoute");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OutgoingRoute");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "IncomingRoute");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "OutgoingRoute",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "IncomingRoute",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "IncomingRoute",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingRoute",
                table: "OutgoingRoute",
                column: "Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomingRoute",
                table: "IncomingRoute",
                columns: new[] { "Phone", "Email" });
        }
    }
}
