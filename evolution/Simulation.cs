

class Simulation
{
    public bool Ready = false;
    private int days;
    private Map map;
    private Statistic stats;
    private string directory = "";
    private string output = "";
    private static int roundToPrintMap = 1;

    /// <summary>
    /// Loads the input parameters and configures the simulation.
    /// </summary>
    /// <param name="inputfile">Name of the file with input.</param>
    /// <param name="outputfile">Name of the file where the record should be stored.</param>

    public Simulation(string inputfile, string directory, string output)
    {
        this.directory = directory;
        this.output = output;
        InputReader reader = new InputReader();
        Parameters parameters = reader.GetParameters(inputfile);
        PrintParams(parameters);
        days = parameters.header.Days;
        map = new Map(parameters);
        Ready = true;
        stats = new Statistic(map, days, ",");
    }




    /// <summary>
    /// Starts the simulation.
    /// </summary>
    public void Start()
    {
        System.IO.Directory.CreateDirectory(directory);
        using (StreamWriter file = new StreamWriter(directory + "\\" + output + "_log.txt"))
        {
            if (file != null)
            {
                MapPrinter.PrintMap(map.GetMap(), "0", file); //"0" because it is the 0th day

                for (int i = 1; i <= days; ++i)
                {
                    map.Day(i);
                    stats.Record(map);
                    if (i % roundToPrintMap == 0) MapPrinter.PrintMap(map.GetMap(), i.ToString(), file);
                    file.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Cannot save log, the file: " + output + " in directory:" + directory + "File cannot be opened.");
            }
        }
    }

    public void Save()
    {
        if (stats.Recorded)
        {
            stats.SaveStats(directory + "/" + output + ".csv");
        }
        else
        {
            Console.WriteLine("No data for saving.");
        }
    }

    private static void PrintParams(Parameters parameters)
    {
        parameters.header.Print();
        foreach (AnimalSpecies animal in parameters.Species)
        {
            animal.Print();
        }
        foreach (PlantSpecies plant in parameters.Plants)
        {
            plant.Print();
        }
    }
}