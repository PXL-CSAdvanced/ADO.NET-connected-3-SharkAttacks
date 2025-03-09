using SharkAttackClassLibrary.DataAccess;
// import Entities.SharkData in 4. AttacksByCountry
// using SharkAttackClassLibrary.Entities;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

public class Program
{
    public static void Main(string[] args)
    {
        SwimSharkAnimation();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n===== Shark Attack Analyzer =====");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. View fatality rates");
            Console.WriteLine("2. View annual attack statistics");
            Console.WriteLine("3. Sort by most attacked activities");
            Console.WriteLine("4. Show attacks by country");
            Console.WriteLine("5. View most attacked body parts");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine() ?? "0");

            switch (choice)
            {
                case 1:
                    #region OverallStats
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    
                    Console.WriteLine($"Total recorded attacks: {SharkAttackData.GetTotalAttakcs()}");
                    Console.WriteLine($"Overall fatality rate: {SharkAttackData.GetFatalityRate():F2}%\n");
                    Console.ResetColor();
                    break;
                    #endregion
                case 2:
                    #region YearlySharkAttacks
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nYearly Shark Attacks");
                    Console.ResetColor();
                    Dictionary<int, int> attacksByYear = new ();
                    
                    attacksByYear = SharkAttackData.GetAttacksByYear();
                    
                    foreach (int year in attacksByYear.Keys)
                    {
                        int count = attacksByYear[year];
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"{year}: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(new string('█', count));
                        Console.ResetColor();
                    }
                    break;
                    #endregion
                case 3:
                    #region DangerousActivities
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nDangerous Activities");
                    Console.ResetColor();
                    Dictionary<string, int> attacksByActivities = new();
                    
                    attacksByActivities = SharkAttackData.GetAttacksByActivity();
                    
                    foreach (string activity in attacksByActivities.Keys)
                    {
                        int count = attacksByActivities[activity];
                        if(count >= 10)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"{activity.PadRight(20)}: ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($"{$"({count})".PadRight(5)} ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(new string('█', count / 10));
                            Console.ResetColor();
                        }
                    }
                    break;
                #endregion DangerousActivities
                case 4:
                    #region AttacksByCountry
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nAttacks By Country");
                    Console.ResetColor();

                    List<string> countries = SharkAttackData.GetAllCountries();
                    
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nAll attacked countries ever");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int i = 0; i < countries.Count; i++)
                    {
                        Console.WriteLine($"{i}: {countries[i]}");
                    }
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n#. Pick a number of a country to inspect its attacks");
                    Console.WriteLine("c. Cancel");
                    Console.Write("Enter your choice: ");
                    string input = Console.ReadLine() ?? "c";
                    if (input.ToLower().Equals("c")){
                        Console.WriteLine("Cancelled...");
                        break;
                    }
                    else if (!int.TryParse(input, out int countryNumber))
                    {
                        Console.WriteLine("Invalid choice");
                        break;
                    }
                    else
                    {
                        List<SharkAttack> sharkAttacksByCountry = SharkAttackData.GetAllAttacksByCountryName(countries[countryNumber]);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\nAll attackes in {countries[countryNumber]} ever");
                        Console.ResetColor();
                        foreach (SharkAttack sharkAttack in sharkAttacksByCountry)
                        {
                            Console.WriteLine(sharkAttack.ToString());
                        }
                    }
                    break;
                    #endregion
                case 5:
                    #region MostAttackedBodyParts
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nMost attacked body parts");
                    Console.ResetColor();

                    Console.WriteLine("Try writing this menu yourself.");
                    // TODO:    Schrijf nu zelf dit console menu. Let op, er is nog geen SharkData methode placeholder voorzien.
                    //          Je zal zelf de data moeten filteren. Ontdek de Injury kolom en bekijk hoe "messy" de data is.
                    break;
                    #endregion
                case 6:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nExiting... Stay safe in the water!    ><(((°>");
                    Console.ResetColor();
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice, try again.");
                    Console.ResetColor();
                    break;
            }
        }

    }

    static void SwimSharkAnimation()
    {
        string title = "===== Shark Attack Analyzer =====";
        string shark = "><(((°>";

        for (int i = 0; i < title.Length; i++)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(title.Substring(0, i) + " ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(shark);
            Console.ResetColor();
            Thread.Sleep(100);
            Console.Clear();
        }
    }
}
