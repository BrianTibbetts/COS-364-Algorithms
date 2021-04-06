using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Prim
{
    class GraphPrim
    {
        public const int INFINITY = 30000;       // Infinity is represented by an arbitrarily large number
        public static int[,] W;                  // The adjacency matrix, also called W
        public static int numVertices;           // Number of vertices

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

                if (directionality == "D") {
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

        // method Prim:
        // Performs Prim's algorithm to find the minimum spanning tree of an undirected graph.
        // Prints each edge of the tree.
        // 
        // arguments:
        //  startVertex - The vertex on the graph where the algorithm starts
        public static void Prim(int startVertex) {
            int i, j;            // loop counters, i for Prim iterations and j for vertex indices
            int vnear = 0;       // represents the vertex closest to Y, the set of vertices available on any given iteration of Prim
            int min;             // the lowest distance between vertices for the current Prim iteration
            int totalWeight = 0; // the total weight of the minimum spanning tree   

            // array nearest - The vertices that make up the edges of the MST:
            //  Index: The vertex being checked. Counted from 1 to n, where n is the number of vertices
            //  Element: The vertex closest to the index's vertex
            int[] nearest = new int[numVertices - 1];   

            // array distance - the weights of the edges made up by the vertex pairs in array nearest
            int[] distance = new int[numVertices - 1];

            // For all vertices, initialize v 0 to be the nearest vertex in Y and initialize
            // the distance from Y to be the weight on the edge to v 0.
            for (i = 0; i < numVertices - 1; i++)
            {
                nearest[i] = 0;
                distance[i] = W[0,i + 1];
            }

            // Perform Prim's algorithm until Y is equal to V, the set of all vertices on the graph
            for (i = 0; i < numVertices - 1; i++)
            {
                min = INFINITY;
                for (j = 0; j < numVertices - 1; j++)
                {
                    if (0 <= distance[j] && distance[j] < min)
                    {    // Skips over any distances set to -1, which indicates the vertex is in set Y
                        min = distance[j];
                        vnear = j;
                    }
                }

                totalWeight += distance[vnear];

                // Adds a vertex to Y once it's determined to be the closest vertex in V - Y to Y.
                distance[vnear] = -1; 

                // Update the distance and nearest arrays for the next Prim iteration
                for (j = 0; j < numVertices - 1; j++)
                {
                    if (W[j + 1, vnear + 1] < distance[j])
                    {
                        distance[j] = W[j + 1, vnear + 1];
                        nearest[j] = vnear + 1;
                    }
                }
            }


            // Print a table of information on the edges of the minimum spanning tree

            Console.WriteLine("\nMinimum Spanning Tree edges: ");
            Console.Write($"{"Edge",-10}");
            Console.WriteLine($"{"Weight",-10}");
            for (i = 0; i < numVertices - 1; i++) {

                Console.Write($"{"(V" + nearest[i] + ", V" + (i + 1) + ")", -10}");
                Console.WriteLine($"{W[nearest[i], (i + 1)],-10}");
            }

            Console.WriteLine("\nTotal weight: " + totalWeight);

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

                Prim(0);  // 0 is the start vertex

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File {0} was not found ", filename, e.ToString());
            }

            return;
        }
    }
}
