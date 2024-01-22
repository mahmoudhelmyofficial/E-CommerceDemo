using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeItemsCountMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCount",
                table: "Cart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemCount",
                table: "Cart",
                type: "int",
                nullable: true);
        }
    }
}
