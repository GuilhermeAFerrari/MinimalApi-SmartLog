using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiSmartLog.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSecondary = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "varchar(200)", nullable: false),
                    StackTrace = table.Column<string>(type: "varchar(200)", nullable: true),
                    Request = table.Column<string>(type: "varchar(200)", nullable: true),
                    Response = table.Column<string>(type: "varchar(200)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
