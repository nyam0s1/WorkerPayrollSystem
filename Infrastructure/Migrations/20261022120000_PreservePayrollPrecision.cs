using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor_Training.Migrations
{
    /// <summary>
    /// Preserve fractional statutory rates without replacing saved settings.
    /// Hourly fields remain optional for fixed-salary employees.
    /// </summary>
    public partial class PreservePayrollPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var rate in new[] { "PayeRate", "ShaRate", "NssfLowerRate", "HousingLevy" })
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: rate,
                    table: "StatutoryConfigs",
                    type: "decimal(18,6)",
                    precision: 18,
                    scale: 6,
                    nullable: false,
                    oldClrType: typeof(decimal),
                    oldType: "decimal(18,2)");
            }

            migrationBuilder.AlterColumn<double>(
                name: "hourlyPay",
                table: "Employees",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "hoursWorked",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Refuse a rollback that would discard fractional rates or replace
            // optional hourly values in existing fixed-salary employee records.
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1 FROM [StatutoryConfigs]
                    WHERE [PayeRate] <> ROUND([PayeRate], 2)
                       OR [ShaRate] <> ROUND([ShaRate], 2)
                       OR [NssfLowerRate] <> ROUND([NssfLowerRate], 2)
                       OR [HousingLevy] <> ROUND([HousingLevy], 2)
                )
                    THROW 50000, 'Rollback would lose statutory rate precision. Retain this migration.', 1;

                IF EXISTS (
                    SELECT 1 FROM [Employees]
                    WHERE [hourlyPay] IS NULL OR [hoursWorked] IS NULL
                )
                    THROW 50000, 'Rollback would lose optional hourly values. Retain this migration.', 1;
                """);

            foreach (var rate in new[] { "PayeRate", "ShaRate", "NssfLowerRate", "HousingLevy" })
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: rate,
                    table: "StatutoryConfigs",
                    type: "decimal(18,2)",
                    nullable: false,
                    oldClrType: typeof(decimal),
                    oldType: "decimal(18,6)",
                    oldPrecision: 18,
                    oldScale: 6);
            }

            migrationBuilder.AlterColumn<double>(
                name: "hourlyPay",
                table: "Employees",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "hoursWorked",
                table: "Employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
