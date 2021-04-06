using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MColoring
{
	class MColor
	{
		public const int INFINITY = 30000;
		public static int[,] W;                  // The adjacency matrix
		public static int numVertices;           // Number of vertices        
		private static int numsol;
		private static int[] vcolor;
		private static int numcolors;
		private static StreamWriter wtr;
		private static StreamReader rdr;


        // method createGraph:
        // Converts the graph data set to a 2D array representing the adjacency matrix
        //
        // arguments:
        //	Streamreader rdr - A Streamreader initialized to read lines from the directed graph data set
        //
        public static void createGraph(StreamReader rdr)
        {

            string directionality;
            string line;
            string[] s; // for split            
            int i, j;

            try
            {
                // get the data from the file and put it into variables, skipping over some unused sections
                line = rdr.ReadLine();                // finds "D" or "U" in the file, describing the graph as Directed or Undirected
                s = line.Split(null);
                directionality = s[0];                // pass null to use whitespace. The "D" or "U" are found at s[0]

                line = rdr.ReadLine();
                s = line.Split(null);
                numVertices = Convert.ToInt32(s[0]);  // like with the directionality, the number of vertices is at s[0]

                line = rdr.ReadLine();

                // Dynamically allocate matrix
                W = new int[numVertices, numVertices];

                // Populate the empty adjacency matrix with default values
                for (i = 0; i < numVertices; i++)
                {
                    for (j = 0; j < numVertices; j++)
                    {

                        if (i == j)
                        {
                            W[i, j] = 0;        // The weight of a path from a vertex to itself should be 0
                        }
                        else
                        {
                            W[i, j] = INFINITY; // Infinity is the default value for any other cells
                        }

                    }
                }

                line = rdr.ReadLine(); // reads in first row of data
                s = line.Split(null);
                i = 0;  // start reading the data at row 0

                if (directionality == "D")
                {
                    while (Convert.ToInt32(s[0]) != -1)
                    {
                        W[Convert.ToInt32(s[0]), Convert.ToInt32(s[1])] = Convert.ToInt32(s[2]);
                        i++;
                        line = rdr.ReadLine(); // get next row
                        s = line.Split(null);
                    }
                }

                else if (directionality == "U")
                {
                    while (Convert.ToInt32(s[0]) != -1)
                    {
                        // Add each edge to the adjacency matrix in both directions
                        W[Convert.ToInt32(s[0]), Convert.ToInt32(s[1])] = Convert.ToInt32(s[2]);
                        W[Convert.ToInt32(s[1]), Convert.ToInt32(s[0])] = Convert.ToInt32(s[2]);
                        i++;
                        line = rdr.ReadLine(); // get next row
                        s = line.Split(null);
                    }
                }


                rdr.Close();

                return;

            } // end try

            catch (IOException e)
            {
                Console.WriteLine("Some I/O problem", e.ToString());
            }

        }

        // method printGraph:
        //	Prints the adjacency matrix and the number of vertices in the matrix
        //
        public static void printGraph()
		{

			Console.WriteLine("Total Vertices: " + numVertices + "\n"); // finds the number of vertices using the first row's length

			for (int i = 0; i < numVertices; i++)
			{
				for (int j = 0; j < numVertices; j++)
				{
					if (W[i, j] == INFINITY)
					{
						Console.Write($"{"oo",-3}");
					}
					else
					{
						Console.Write($"{W[i, j],-3}");
					}

				}
				Console.Write("\n");
			}
		}

		// method promising:
		// Determines whether a vertex can be colored differently to reach a solution to the m-coloring problem.
		// 
		// arguments:
		//  int vertexIndex - Index of the vertex being checked
		public static bool promising(int vertexIndex)
        {
            int nextIndex = 0;
            bool isPromising = true;
            while (nextIndex < vertexIndex && isPromising)
            {
                // Can the node be colored differently to find a solution?
                if (W[vertexIndex, nextIndex] != INFINITY &&		// If the given node and the next node are adjacent...
					vcolor[vertexIndex] == vcolor[nextIndex])		// and they have the same color...
                {
                    isPromising = false;						    // then the given node doesn't lead to a solution
                }
                nextIndex++;
            }
            return isPromising;
        }

        // method m_coloring:
        // Makes use of backtracking to efficiently traverse the tree of possible solutions to the m_coloring problem.
        // Uses promising() to decide whether to continue down a branch of the tree or deem it a dead end.
        
        // arguments:
        //	int vertexIndex - Either -1, to start m-coloring, or the index m-coloring is currently working with
        public static void m_coloring(int vertexIndex)
        {
            for (int color = 1; color <= numcolors; color++) {

                // Update the color of the next vertex so that it can be included in possible solutions
                vcolor[vertexIndex + 1] = color;

                // If the next vertex can be turned a promising color...
                if (promising(vertexIndex + 1))
                {
                    // If the tree has reached a node at the end of a complete solution...
                    if (vertexIndex + 1 == numVertices - 1)
                    {
                        numsol += 1;
                        wtr.WriteLine("{0}", string.Join("  ", vcolor));
                    }

                    // Otherwise, continue looking for solutions to m_coloring
                    else
                    {
                        vcolor[vertexIndex + 1] = color;
                        m_coloring(vertexIndex + 1);
                    }
                }

            }

        }


        static void Main(string[] args)
		{
			String filename1 = "results.txt";   // output file
			String filename2 = "ColorA.txt";    // input file

			try
			{
				wtr = new StreamWriter(filename1);
				wtr.WriteLine("Colors are:\n");
				rdr = new StreamReader(filename2);

				createGraph(rdr);

				printGraph();

				numcolors = 4;
				numsol = 0;
                vcolor = new int[numVertices];
                m_coloring(-1);  // 0 is start vertex

				wtr.WriteLine("Number of solutions: " + numsol);
				wtr.Close();
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("One of your files weas not found.");
			}
		}
	}
}
