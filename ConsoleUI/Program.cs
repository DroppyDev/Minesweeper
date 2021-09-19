using Engine;
using static System.Console;

namespace ConsoleUI
{
    public class Program
    {
        public static void Main()
        {
            Field[,] fields = new[,]
            {
                { new Field(false), new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(true),  new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false), new Field(false) },
                { new Field(false), new Field(false), new Field(false), new Field(true) }
            };

            Plane plane = new(fields);

            PrintPlane(plane);
            WriteLine();

            do
            {
                string position = ReadLine();
                string[] pos = position.Split(" ");
                int row = int.Parse(pos[0]);
                int column = int.Parse(pos[1]);
                if (pos.Length == 3)
                    plane.Mark(row, column);
                else
                    plane.Open(row, column);
                WriteLine();
                PrintPlane(plane);
                WriteLine();
            } while (!plane.Finished);
            
            WriteLine($"Game over! You {(plane.IsSuccess ? "win" : "lose")}!");
        }

        private static void PrintPlane(Plane plane)
        {
            Field[,] fields = plane.GetFields();
            for (int row = 0; row < plane.RowCount; row++)
            {
                for (int column = 0; column < plane.ColumnCount; column++)
                    Write(fields[row, column] != null
                        ? fields[row, column].HasMine == true
                            ? " X"
                            : " " + fields[row, column].AdjacentMines
                        : " _");
                WriteLine();
            }
        }
    }
}
