using Microsoft.EntityFrameworkCore.Migrations;

namespace FlowrouteApi.Migrations
{
    public partial class outgoingupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingRoute",
                table: "OutgoingRoute");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "OutgoingRoute",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingRoute",
                table: "OutgoingRoute",
                column: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingRoute",
                table: "OutgoingRoute");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "OutgoingRoute",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingRoute",
                table: "OutgoingRoute",
                columns: new[] { "Phone", "Email" });
        }
    }
}
