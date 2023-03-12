using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ASK.fm
{
	internal static class Helper 
	{
		public static List<string> ReadFileLines(string path)
		{
			List<string> lines = new List<string>();

			try
			{
				using (StreamReader file = new StreamReader(path))
				{
					string line;
					while ((line = file.ReadLine()) != null)
					{
						if (line.Length == 0)
							continue;
						lines.Add(line);
					}
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("\n\nError: Can't open the file \n");
			}
			return lines;
		}
		
		public static void WriteFileLines(string path, List<string> lines, bool append = true)
		{
			try
			{
                using (StreamWriter writer = new StreamWriter(path, append))
                {
                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
			catch (FileNotFoundException)
			{
				Console.WriteLine("\n\nError: Can't open the file\n");
			}
			
		}

		public static int ReadInt(int low, int high)
		{
			Console.WriteLine($"\nEnter number in range {low} - {high}: ");
			int val = int.Parse(Console.ReadLine());

			if (low <= val && val <= high)
				return val;

			Console.WriteLine("Error: invalid number...Try again\n");
			return ReadInt(low, high);
		}

        public static int ShowReadMenu(List<string> choices)
        {
            Console.WriteLine("\nMenu: ");
            for (int ch = 0; ch < choices.Count(); ch++)
            {
                Console.WriteLine($"\t {ch + 1}: {choices[ch]}");
            }
            return ReadInt(1, choices.Count());
        }

    }
}
