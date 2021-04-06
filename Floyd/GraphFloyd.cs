using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFloyd
{
    class GraphFloyd
    {
        public static int numVertices;      // Number of vertices on the graph
        public static int[,] W;             // The adjacency matrix, with each cell representing an adjacent path (W[0,1] would be the path from vertex 0 to vertex 1)
        public static int[,] D;				// A matrix to track the paths by weight
        public static int[,] P;             // The path matrix, which tracks the vertices used on the shortest paths
        public const int INFINITY = 30000;  // Arbitrary value representing infinity for missing edges in the adjacency matrix

        // method createGraph:
        //	Reads data from 
        // Converts the directed graph data set to a 2D array representing the adjacency matrix (W)
        //
        // arguments:
        //	Streamreader rdr - A Streamreader initialized to read lines from the directed graph data set
        //
        public static void createGraph(StreamReader rdr)
        {

            string line;
            string[] s; // for split            
            int i, j;

            try
            {
                // get the data from the file and put it into variables
                line = rdr.ReadLine(); // The graph type, D for Directed or U for Undirected
                line = rdr.ReadLine(); // reads # rows as a string
                s = line.Split(null); // pass null to use whitespace. The number of vertices is in s[0]

                numVertices = Convert.ToInt32(s[0]);

                line = rdr.ReadLine(); // skips over comment

                // Dynamically allocate matrix
                W = new int[numVertices, numVertices];

                // Populate the empty adjacency matrix with default values
                for (i = 0; i < numVertices; i++) {
                    for (j = 0; j < numVertices; j++) {

                        if (i == j)
                        {
                            W[i, j] = 0;        // The weight of a path from a vertex to itself should be 0
                        }
                        else {
                            W[i, j] = INFINITY; // Infinity is the default value for any other cells
                        }
                        
                    }
                }

                line = rdr.ReadLine(); // reads in first row of data
                s = line.Split(null);
                i = 0;  // start reading the data at row 0

                while (Convert.ToInt32(s[0]) != -1)
                {
                    W[Convert.ToInt32(s[0]), Convert.ToInt32(s[1])] = Convert.ToInt32(s[2]);
                    i++;
                    line = rdr.ReadLine(); // get next row
                    s = line.Split(null);
                }

                rdr.Close();

                return;

            } // end try

            catch (IOException e)
            {
                Console.WriteLine("Some I/O problem", e.ToString());
            }

        } // end CreateGraph	

        // method printGraph:
        //	Prints the adjacency matrix and the number of vertices in the matrix
        //
        public static void printGraph()
        {

            Console.WriteLine("Total Vertices: " + numVertices + "\n");	// finds the number of vertices using the first row's length

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

        // method Floyd:
        //	Performs Floyd's algorithm to find a matrix of shortest paths through the graph
        //
        // The path matrix describes which intermediate vertices can be used to travel the shortest path between each pair of vertices on the graph
        //
        public static void Floyd()
        {
            int i, j, k;

            // Create more matrices of the same size as the adjacency matrix
            D = new int[numVertices, numVertices];
            P = new int[numVertices, numVertices];

            for (i = 0; i < numVertices; i++)
            {
                for (j = 0; j < numVertices; j++)
                {
                    P[i, j] = -1;		// P starts with -1 values
                    D[i, j] = W[i, j];	// D starts with the adjacency matrix values
                }
            }

            for (k = 0; k < numVertices; k++)
            {
                for (i = 0; i < numVertices; i++)
                {
                    for (j = 0; j < numVertices; j++)
                    {

                        if (D[i, k] + D[k, j] < D[i, j])    // Floyd complexity: this comparison occurs n^3 times, where n = numVertex
                        {
                            P[i, j] = k;
                            D[i, j] = D[i, k] + D[k, j];
                        }

                    }
                }
            }

            Console.WriteLine("\nMatrix D:");
            // Print D, the matrix of edge weights, in its final state
            for (i = 0; i < numVertices; i++)
            {
                for (j = 0; j < numVertices; j++)
                {
                    if (D[i, j] == INFINITY)
                    {
                        Console.Write($"{"oo",-3}");
                    }
                    else
                    {
                        Console.Write($"{D[i, j],-3}");
                    }
                }
                Console.Write("\n");
            }

            Console.WriteLine("\nMatrix P:");
            // Print P, the path matrix
            for (i = 0; i < numVertices; i++)
            {
                for (j = 0; j < numVertices; j++)
                {

                    Console.Write($"{P[i, j],-3}");
                }
                Console.Write("\n");
            }

            Console.WriteLine("\nShortest paths between vertices:");
            for (i = 0; i < numVertices; i++)
            {
                for (j = 0; j < numVertices; j++)
                {
                    if (P[i, j] != -1)
                    {    // Only print pairs of vertices that have paths
                        Console.Write("V" + i + " --> ");
                        path(i, j);
                        Console.WriteLine("V" + j);
                    }
                }
            }
        }
        // method path:
        // Prints the shortest path between any two vertices on the graph represented by the input data file
        //  
        // arguments:
        //  int q, int r - The indices of any two vertices on the graph, for the path matrix (P)
        //
        public static void path(int q, int r) {
            if (P[q, r] != -1)          // base case: -1 represents a pair of vertices without a path
            {
                path(q, P[q, r]);
                Console.Write("V" + P[q, r] + " --> ");
                path(P[q, r], r);
            }
        }

        static void Main(string[] args)
        {
            // args[0] is the first command line argument you supply.            
            StreamReader rdr;

            // assign filename when define or get error in catch block
            String filename = null;

            try
            {
                filename = args[0];
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("You must enter the filename on the command line",
                    e.ToString());
            }

            try
            {
                rdr = new StreamReader(filename);

                createGraph(rdr);

                printGraph();

                Floyd();

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File {0} was not found ", filename, e.ToString());
            }

            return;
        }
    }
}
