using System.Text;

/// <summary>
/// Class for saving statistics
/// </summary>
public class Statistic
{
    public bool Recorded = false;
    private string d;
    private int records;
    private Dictionary<int, List<SpeciesStat>> speciesStats;
    private Dictionary<int, List<PlantsStat>> plantsStats;

    public Statistic(Map map, int days, string delimiter)
    {

        speciesStats = new Dictionary<int, List<SpeciesStat>>();
        plantsStats = new Dictionary<int, List<PlantsStat>>();
        d = delimiter;
        records = days + 1; // days + starting point
        foreach (PlantSpecies p in map.GetPlants())
        {
            List<PlantsStat> stats = new List<PlantsStat>(records);
            plantsStats.Add(p.Id, stats);
        }
        foreach (AnimalSpecies s in map.GetSpecies())
        {
            List<SpeciesStat> stats = new List<SpeciesStat>(records);
            speciesStats.Add(s.Id, stats);
        }
        Record(map);
    }

    public void Record(Map map)
    {
        BeginRecord(map);
        ProcessMap(map);
    }

    public void SaveStats(string fileName)
    {
        try
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                List<int> keysPlants = new List<int>(plantsStats.Keys);
                List<int> keysSpecies = new List<int>(speciesStats.Keys); 


                file.WriteLine(MakeHeadline(keysPlants, keysSpecies));
                for (int i = 0; i < records; ++i)
                {
                    file.Write($"{i}{d}");
                    foreach (int key in keysPlants)
                    {
                        file.Write(plantsStats[key][i].ToString(d));
                    }
                    foreach (int key in keysSpecies)
                    {
                        file.Write(speciesStats[key][i].ToString(d));
                    }
                    file.WriteLine();
                }
                Console.WriteLine("Results saved to: " + fileName);

            }
        }
        catch (IOException)
        {
            Console.WriteLine("Unable to open file: " + fileName);
        }
    }
    /// <summary>
    /// Make headline in csv file with statistics
    /// </summary>
    /// <param name="plants">kinds of plants</param>
    /// <param name="species">kinds of species</param>
    /// <returns></returns>
    private string MakeHeadline(List<int> plants, List<int> species)  
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("day,");
        foreach (int key in plants)
        {
            sb.Append(key + "_plant_count" + d);
        }
        foreach (int key in species)
        {
            sb.Append(key + "_species_size" + d);
            sb.Append(key + "_species_sense" + d);
            sb.Append(key + "_species_dexterity" + d);
            sb.Append(key + "_species_count" + d);
        }
        sb.AppendLine();
        return sb.ToString();
    }

    private void BeginRecord(Map map)
    {
        Recorded = true;
        foreach (PlantSpecies p in map.GetPlants())
        {
            plantsStats[p.Id].Add(new PlantsStat(0));
        }
        foreach (AnimalSpecies s in map.GetSpecies())
        {
            speciesStats[s.Id].Add(new SpeciesStat(0, 0, 0, 0));
        }
    }
    /// <summary>
    /// make record for one day 
    /// </summary>
    /// <param name="map">envioment</param>
    private void ProcessMap(Map map)
    {
        foreach (Coordinates coor in map.AnimalsCoordinates)
        {
            Item? item = map.GetMap()[coor.X, coor.Y];
            if (item != null && item is Animal)
            {
                Animal a = (Animal)item;
                speciesStats[a.Species.Id][speciesStats[a.Species.Id].Count - 1].NoteAnimal(a.Size, a.Sense, a.Dexterity, 1);
            }
        }
        foreach (PlantSpecies plant in map.GetPlants())
        {
            plantsStats[plant.Id][plantsStats[plant.Id].Count - 1].NotePlant(plant.Count);

        }
    }
}

public class SpeciesStat
{
    public float Size { get; private set; }
    public float Sense { get; private set; }
    public float Dexterity { get; private set; }
    public int Count { get; private set; }

    public SpeciesStat(float size, float sense, float dexterity, int count)
    {
        Size = size;
        Sense = sense;
        Dexterity = dexterity;
        Count = count;
    }

    public void NoteAnimal(float si, float se, float de, int co)
    {
        Size += si;
        Sense += se;
        Dexterity += de;
        Count += co;
    }

    public string ToString(string delimiter)
    {
        if (Count > 0)
        {
            return $"{Size / Count}{delimiter}{Sense / Count}{delimiter}{Dexterity / Count}{delimiter}{Count}{delimiter}";
        }
        else
        {
            return $"{delimiter}{delimiter}{delimiter}{Count}{delimiter}";
        }
    }
}

public class PlantsStat
{
    public int Count { get; private set; }

    public PlantsStat(int count)
    {
        Count = count;
    }

    public void NotePlant(int co)
    {
        Count = co;
    }

    public string ToString(string delimiter)
    {
        return $"{Count}{delimiter}";
    }
}
