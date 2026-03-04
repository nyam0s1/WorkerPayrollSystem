using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor_Training.Migrations
{
    /// <inheritdoc />
    public partial class FinalPayrollIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KraPin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NssfNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NhifNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hourlyPay = table.Column<double>(type: "float", nullable: false),
                    hoursWorked = table.Column<int>(type: "int", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HouseAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransportAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsMarried = table.Column<bool>(type: "bit", nullable: false),
                    HasDisability = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payslips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollRunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HouseAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransportAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NssfAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxablePay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShaAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HousingLevyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PersonalRelief = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payslips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatutoryConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShaRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NssfLowerRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NssfLowerLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NssfUpperLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HousingLevy = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PersonalRelief = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatutoryConfigs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Payslips");

            migrationBuilder.DropTable(
                name: "StatutoryConfigs");

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hourlyPay = table.Column<double>(type: "float", nullable: false),
                    hoursWorked = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });
        }
    }
}
