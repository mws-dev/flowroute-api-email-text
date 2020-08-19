using Microsoft.EntityFrameworkCore.Migrations;

namespace FlowrouteApi.Migrations
{
    public partial class initialdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomingRoute",
                columns: table => new
                {
                    Phone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingRoute", x => new { x.Phone, x.Email });
                });

            migrationBuilder.CreateTable(
                name: "OutgoingRoute",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingRoute", x => new { x.Phone, x.Email });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingRoute");

            migrationBuilder.DropTable(
                name: "OutgoingRoute");
        }
    }
}
