using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDbApi.Migrations
{
    /// <inheritdoc />
    public partial class FavouritesJoinedWithMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Favourites_MovieId",
                table: "Favourites",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Movies_MovieId",
                table: "Favourites",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Movies_MovieId",
                table: "Favourites");

            migrationBuilder.DropIndex(
                name: "IX_Favourites_MovieId",
                table: "Favourites");
        }
    }
}
