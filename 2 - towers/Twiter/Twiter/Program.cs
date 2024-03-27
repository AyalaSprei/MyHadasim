using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            do
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Square");
                Console.WriteLine("2. Triangle");
                Console.WriteLine("3. Exit");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        HandleSquare();
                        break;
                    case 2:
                        HandleTriangle();
                        break;
                    case 3:
                        Console.WriteLine("Exiting the program...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }
            } while (choice != 3);
        }

        static void HandleSquare()
        {
            Console.Write("Enter the height: ");
            int height;
            if (!int.TryParse(Console.ReadLine(), out height) || height < 2)
            {
                Console.WriteLine("Invalid height. Height must be an integer greater than or equal to 2.");
                return;
            }

            Console.Write("Enter the width: ");
            int width;
            if (!int.TryParse(Console.ReadLine(), out width))
            {
                Console.WriteLine("Invalid width. Please enter a valid integer.");
                return;
            }

            if (Math.Abs(width - height) > 5)
            {
                Console.WriteLine("Area: " + (height * width));
            }
            else
            {
                Console.WriteLine("Scope: " + (2 * (height + width)));
            }
        }

        static void HandleTriangle()
        {
            Console.Write("Enter the height: ");
            int height;
            if (!int.TryParse(Console.ReadLine(), out height) || height < 2)
            {
                Console.WriteLine("Invalid height. Height must be an integer greater than or equal to 2.");
                return;
            }

            Console.Write("Enter the width: ");
            int width;
            if (!int.TryParse(Console.ReadLine(), out width))
            {
                Console.WriteLine("Invalid width. Please enter a valid integer.");
                return;
            }

            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Calculate scope");
            Console.WriteLine("2. Print triangle");
            int triangleChoice;
            if (!int.TryParse(Console.ReadLine(), out triangleChoice))
            {
                Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                return;
            }

            switch (triangleChoice)
            {
                case 1:
                    double hypotenuse = Math.Sqrt(height * height + width * width);
                    double scope = 2 * hypotenuse + width;
                    Console.WriteLine("scope: " + scope);
                    break;
                case 2:
                    PrintTriangle(width, height);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
            static void PrintTriangle(int width, int height)
            {
                if (!(width % 2 == 1 && width < height * 2)|| width == 3 && height>2)
                {
                    Console.WriteLine("can't print");
                }
                else
                {
                    int MinRowsInMiddle = ((width + 1) / 2) - 2;
                    int times = (height - 2) / MinRowsInMiddle;
                    int last = (height - 2) % MinRowsInMiddle;
                    Console.WriteLine(new string(' ', (width - 1) / 2) + new string('*', 1));
                    for (int i = 0; i < last; i++)
                    {
                        Console.WriteLine(new string(' ', (width - 3) / 2) + new string('*', 3));
                    }
                    for (int i = 3; i < width; i += 2)
                    {
                        for (int j = 0; j < times; j++)
                        {
                            Console.WriteLine(new string(' ', (width - i) / 2) + new string('*', i));
                        }

                    }
                    Console.WriteLine(new string('*', width));



                    

                }
            }
        }
    }
}