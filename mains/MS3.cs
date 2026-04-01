using System;

class MilestoneThreeTests
{
    // currently Main so we can run it ez
    static void Main(string[] args)
    {
        String path = "src/mystery-functions/";

        // lol probably should make the names more cryptic
        // String[] mysteryFileNames =
        // {
        //     "average.txt",
        //     "plus5.txt",
        //     "isEven.txt",
        // };

        // did this to make it easier to play the game from command line
        Dictionary<int, string> mysteryFunctions = new Dictionary<int, string>()
        {
            { 1, "average" },
            { 2, "difference" },
            { 3, "isEven" },
            { 4, "mod" },
            { 5, "plus5" },
            { 6, "ppm" },
            { 7, "prodsum" }
        };

        // it must accept a file as a command line argument 
        if (args.Length < 1)
        {
            Console.WriteLine($"pls provide an integer 1 - {mysteryFunctions.Count} for a mystery function!!!:");
            return;
        }

        int mysteryNumber;
        if (!int.TryParse(args[0], out mysteryNumber) || !mysteryFunctions.ContainsKey(mysteryNumber))
        {
            Console.WriteLine($"integer between 1 and {mysteryFunctions.Count} only");
            return;
        }

        string filename = mysteryFunctions[mysteryNumber] + ".txt";
        string filepath = Path.Combine(path, filename);

        // open file and get the lines
        string[] lines = System.IO.File.ReadAllLines(filepath);

        Console.WriteLine($"{filename} has {lines.Length}");
        foreach (var line in lines)
        {
            Console.WriteLine($"\t{line}");
        }

        TuiInterface tui = new TuiInterface();
        tui.TerminalApp(lines);
    }
}