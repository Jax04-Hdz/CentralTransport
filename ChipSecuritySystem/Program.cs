using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem
{
   internal class Program
    {
	private static readonly List<ColorChip> Chips = new List<ColorChip>();
        private static readonly Random Rand = new Random();



        private static void Main(string[] args)
        {
	    // Clear console and display options
            Console.Clear();
            Console.WriteLine("Chips Required to Unlock Master Panel\n");
            Console.WriteLine("1. Manually enter chips.");
            Console.WriteLine("2. Use example chips: [Blue, Yellow] [Red, Green] [Yellow, Red] [Orange, Purple].");
            
            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2))
            {
                Console.WriteLine("Invalid input. Please enter 1 or 2.");
            }
            
            switch (option)
            {
                case 1:
                    // Get user input for chips
                    InputChips();
                    break;
                case 2:
                    // Generate Sample Chips
                    Chips.Add(new ColorChip(Color.Blue, Color.Yellow));
                    Chips.Add(new ColorChip(Color.Red, Color.Green));
                    Chips.Add(new ColorChip(Color.Yellow, Color.Red));
                    Chips.Add(new ColorChip(Color.Orange, Color.Purple));
                    break;
            }

            Console.WriteLine("Your Chips");

            foreach (var chip in Chips)
            {
                Console.WriteLine("[" + chip + "]");
            }
            Console.WriteLine();

            var chipSet = new HashSet<ColorChip>();

            foreach (var chip in Chips.Where(chip => chip.StartColor == Color.Blue))
            {
                chipSet.Add(chip);
                if (FindChain(chip, chipSet))
                {
                    Console.WriteLine("Sequence\n----");

                    foreach (var c in chipSet)
                    {
                        Console.WriteLine("[" + c + "]");
                    }

                    Console.WriteLine("\n Panel unlocked!\n");
                    return;
                }

                chipSet.Remove(chip);
            }
	    Console.WriteLine(Constants.ErrorMessage + "!\n");
        }

       

        private static void InputChips()
        {
            Console.WriteLine("\nEnter colors for 4 chips. Choices:");

            var values = Enum.GetValues(typeof(Color));
            for (var j = 0; j < values.Length; j++)
            {
                Console.WriteLine($"{j + 1}. {values.GetValue(j)}");
            }

            for (var i = 0; i < 4; i++)
            {
                Console.WriteLine($"\nChip {i + 1}:");

                Color startColor = GetColorInput(nameof(startColor));
                Color endColor = GetColorInput(nameof(endColor));

                Chips.Add(new ColorChip(startColor, endColor));
            }
        }

        private static Color GetColorInput(string colorType)
        {
            Console.Write($"{colorType}: ");

            int input;
            while (!int.TryParse(Console.ReadLine(), out input)
                || input < 1 || input > Enum.GetValues(typeof(Color)).Length)
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
            return (Color)(input - 1);
        }

        private static bool FindChain(ColorChip lastChip, ISet<ColorChip> chipSet)
        {
            if (lastChip.EndColor == Color.Green)
            {
                return true;
            }

            foreach (var nextChip in Chips.Where(c => c.StartColor == lastChip.EndColor))
            {

                if (!chipSet.Add(nextChip)) continue;

                if (FindChain(nextChip, chipSet))
                {
                    return true;
                }

                chipSet.Remove(nextChip);
            }

            return false;
        }
    }
}
