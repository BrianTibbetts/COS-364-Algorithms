using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQueensProject
{
    public class NQueens
    {
        public static StreamWriter wtr;
        public static int[] queenCols;      // Each queen's position on the chessboard
                                            // queenCols's indices represent the rows and its values represent the columns
        public static int numSolutions = 0;

        // method promising:
        // Checks whether a tile in the given row is safe from attacks by other queens.
        // 
        // arguments:
        //  int queenRow - The row being checked
        public static bool promising(int queenRow)
        {
            int rowIndex = 0;
            bool isSafe = true;
            while (rowIndex < queenRow && isSafe)
            {
                // Would the queen be under attack?
                if (queenCols[queenRow] == queenCols[rowIndex] ||                                // The queens could be in the same column...
                    Math.Abs(queenCols[queenRow] - queenCols[rowIndex]) == queenRow - rowIndex)  // or diagonal from one another
                    {
                        isSafe = false;
                    }
                rowIndex++;
            }
            return isSafe;
        }

        // method playQueens:
        // Traverses the tree of possible solutions to nQueens recursively with backtracking.
        //
        // arguments:
        //  int i - Either -1 (the start of the algorithm) or the index of a row on the chessboard
        //      i is a row that has been checked by promising().
        //      Checking rows with promising() allows the algorithm to find and skip over dead end solutions.
        public static void playQueens(int i)
        {
            int j;

            for (j = 0; j < queenCols.Length; j++)
            {
                // First, update the position of the queen in the next row so it can be checked for attackers.
                queenCols[i + 1] = j;   

                // if the next row has a promising tile for a queen to be placed on...
                if (promising(i + 1))
                {
                    // if there is a solution to nQueens in the next row...
                    if (i + 1 == queenCols.Length - 1)
                    {
                        numSolutions++;
                        Console.WriteLine("Solution #" + numSolutions + ":");
                        Console.WriteLine("[{0}]", string.Join(", ", queenCols));
                    }

                    // otherwise, continue looking for solutions with the next row
                    else
                    {
                        playQueens(i + 1);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            string filename = null;

            Console.WriteLine("Enter the number of queens you want\n");
            int numQueens = Convert.ToInt32(Console.ReadLine());

            try
            {
                filename = args[0];
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("You must enter a filename on the command line.");
                e.ToString();
            }

            try
            {
                wtr = new StreamWriter(filename);
                queenCols = new int[numQueens];
                playQueens(-1);
                wtr.WriteLine("The number of solutions is " + numSolutions);
                wtr.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File {0} was not found ", filename);
                e.ToString();
            }

            return;
        }  // end Main

    } // end class NQueens
}  // end namespace NQueensProject
