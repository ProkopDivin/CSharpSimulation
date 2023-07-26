using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        /**/
        if (args.Length != 2)
        {
            Console.WriteLine("ERROR: argument missing, 2 arguments needed");
            return;
        }
        /**/
        {
            /**      //for testingpurpouses
            string pathToHomeDir= "C:\\Users\\proko\\Documents\\2022-2023predmety\\c#\\evolution\\results\\";
            string name = "big";
            string pathToInput = pathToHomeDir + name +".txt";    
            string outputDirectory = pathToHomeDir;
            /**/
        }
        string[] deliminators = { "\\", "/" };
        string pathToInput = args[0];
        string outputDirectory = args[1];
        string[] itemsInPath = (args[0]).Split(deliminators, StringSplitOptions.RemoveEmptyEntries);
        string name = itemsInPath[itemsInPath.Length - 1];
        name = name.Substring(0, name.Length - 4);
        try
        {
        
            Simulation simulation = new Simulation(pathToInput, outputDirectory, name);
            if (simulation.Ready)
            {
                simulation.Start();
                simulation.Save();
            }

        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

