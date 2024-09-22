using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDesceptionField_ProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Time-travelers, steampunk robots, dinosaurs, Martian invaders, superheroes, and adventures beyond imagining await you in CHANCE FORTUNE OUT OF TIME, the latest volume in a series praised by Publishers Weekly, VOYA, Young Adult Books Central, and many others. The secret is out: alleged superhuman Chance Fortune is only a normal boy named Josh Blevins. Will his friends and teammates, the Outlaws, band together and accept Josh for who he is so that the future may be saved? Or will old prejudices divide and conquer, robbing Josh, the Outlaws, and the world at large of a chance for a better tomorrow? Find out in CHANCE FORTUNE OUT OF TIME, the long-awaited sequel to CHANCE FORTUNE AND THE OUTLAWS and CHANCE FORTUNE IN THE SHADOW ZONE.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Jensen. Lydia is a scholar, but books are her downfall when she meddles in the plots of the most powerful man in the Celendor Empire. Her life in danger, she flees west to the far side of the Endless Seas and finds herself entangled in a foreign war where her burgeoning powers are sought by both sides.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "This book is based on true events, turned into a fictional story. On her journey to Idaho, in the late 1860's's, with her husband and adopted daughter, Philomena is captured by a war party of Sioux. She does not know if her husband is alive or dead, when Ottawa, takes her, her daughter, her friend, Milly and Milly's son as captives. As Philomena tries to deal with the horrors that befall her, an adopted son of Chief Ottawa, Wechela, whom the Sioux captured as a child from the Shoshone, befriends her and helps lift her burden.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "Angie Ross, aged fifteen, sadistic, brutal, evil, wicked... The press had been the first to brand this teenager with all the usual condemnations and who could blame them? But, what drove Angie to commit such horrendous crimes? It was Susan Raynor's job to unravel the human story behind the monstrous act and to assess whether Angie was the sadistic beast that society believed she was. Angie’s dreams of her 'cotton candy' home, far away from the horrors of her childhood, depended solely on the outcome of this assessment but how could she tell a story so carefully locked away?");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Mila, the seal, has found a special rock in the middle of the sea. One day, after a funny accident, she meets Charlie the seagull. The two become friends and together they share precious time on the rock until Charlie has to leave with his family.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "\"Leaves and Wonders\" is a captivating collection of short stories that invites readers into a world where the ordinary and the extraordinary collide. Each tale is a tapestry of wonder, woven from the threads of imagination and the beauty found in the everyday. With lyrical prose and enchanting narratives, this book is a journey through the realms of possibility, where leaves rustle with secrets, and wonders await around every corner.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");
        }
    }
}
