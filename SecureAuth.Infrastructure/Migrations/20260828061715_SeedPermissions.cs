using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            const long userRoleId = 1;
            const long adminRoleId = 2;

            migrationBuilder.InsertData(
               schema: "security",
                table: "RoleClaims",
                columns: new[] { "Id", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { 1, userRoleId, "permission", "user.read" });

            migrationBuilder.InsertData(
                schema: "security",
                table: "RoleClaims",
                columns: new[] { "Id", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { 2, adminRoleId, "permission", "user.read" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "security",
                table: "RoleClaims",
                keyColumn:  "Id",
                keyValue:  1 );

            migrationBuilder.DeleteData(
                schema: "security",
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue:  2 );
        }
    }
}
