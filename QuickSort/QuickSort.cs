using System;

namespace QuickSort
{

	class Program
	{
		static long[] unsorted_array;			// An array of random numbers to be sorted

		static long swap_count;					// Tracks the number of exchanges Partition() makes
		static long[] average_swap_counts;		// Stores the average number of exchanges for each length of array
												
		static Random rnd;


		private static void quicksort(long low, long high)
		{
			long pivotpoint;

			if (high > low)
			{

				partition(low, high, out pivotpoint);
				quicksort(low, pivotpoint - 1);
				quicksort(pivotpoint + 1, high);

			}
		}

		private static void partition(long low, long high, out long pivotpoint)
		{

			long i;
			long j = low;
			long pivotitem = unsorted_array[low];
			long temp;

			for (i = low + 1; i <= high; i++)
			{
				if (unsorted_array[i] < pivotitem)
				{
					j++;

					temp = unsorted_array[i];
					unsorted_array[i] = unsorted_array[j];
					unsorted_array[j] = temp;
					swap_count++;
				}
			}
			pivotpoint = j;

			temp = unsorted_array[low];
			unsorted_array[low] = unsorted_array[j];
			unsorted_array[j] = temp;
			swap_count++;
		}

		static void Main()
		{
			rnd = new Random();

			int[] lim = { 10, 50, 100, 500, 1000, 5000, 10000, 25000, 50000, 75000, 100000, 200000, 300000, 400000, 500000 };
			average_swap_counts = new long[lim.Length];

			for (int i = 0; i < lim.Length; i++)					// For each array length in lim[], creates an array and
			{														// adds a value to average_swap_counts.

				unsorted_array = new long[lim[i]];

				for (int j = 0; j < 50; j++)						// Runs QuickSort 50 times for each array length in lim[]
				{

					for (long k = 0; k < lim[i]; k++)				// Fills the array with random numbers for QuickSort to sort
					{
						unsorted_array[k] = rnd.Next(lim[i]);
					}

					quicksort(0, unsorted_array.Length - 1);

				}

				average_swap_counts[i] = swap_count / 50;
				swap_count = 0;

			}
			// Makes a table of information on the efficiency of QuickSort
			Console.WriteLine($"{"Array size (n)", -22}{"Average exchanges", -22}{"Average exchanges \\ n", -22}");

			for (int i = 0; i < lim.Length; i++)
			{
				Console.Write($"{lim[i],-22}");
				Console.Write($"{average_swap_counts[i],-22}");
				Console.WriteLine($"{(double) average_swap_counts[i] / (double) lim[i], -22}");
			}
		}
	}
}
