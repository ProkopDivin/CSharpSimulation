/// <summary>
/// For animal species.
/// </summary>
public struct NutritionsMultiplicators
{
    public NutritionsMultiplicators()
    {
        Plants = -1;
        Meat = -1;
    }

    public NutritionsMultiplicators(float meat, float plants)
    {
        Meat = meat;
        Plants = plants;
    }
    public float Plants { get; set; }
    public float Meat { get; set; }
}

/// <summary>
/// For changing the maximum amount of food on the map.
/// At the end of the day, the food is replenished up to a certain number.
/// </summary>
public class FoodChanger
{
    public FoodChanger()
    {
        Interval = -1;
        Times = -1;
        Max = -1;
        Min = -1;
    }

    public FoodChanger(int interval, float times, int max, int min)
    {
        Interval = interval;
        Times = times;
        Max = max;
        Min = min;
    }

    public int Interval { get; set; }
    public float Times { get; set; }
    public int Max { get; set; }
    public int Min { get; set; }
}

/// <summary>
/// Class for loading parameters in the header section of the input.
/// </summary>
public class Header
{

    private int? days;
    private float? divider;
    private int? mapWidth;
    private int? mapHeight;
    private Calculator1? calc;

    private static string WasNotSetMessage(string parameter)
    {
        return $"{parameter} of a header wasn't set!";
    }

    public void Print()
    {
        Console.WriteLine("header parameters:");
        Console.WriteLine("days: " + Days);
        Console.WriteLine("map_width: " + MapWidth);
        Console.WriteLine("map_height: " + MapHeight);
        Console.WriteLine("calculator: " + calculator.ToString());
        Console.WriteLine();
    }
    public int Days
    {
        get { return days ?? throw new ArgumentException(WasNotSetMessage("days")); }
        set
        {
            if (value > 0)
            {
                days = value;
            }
            else
            {
                throw new ArgumentException("Error: days in header must be greater than or equal to 1");
            }
        }
    }

    public float Divider
    {
        get { return divider ?? throw new ArgumentException(WasNotSetMessage("divider")); }
        set
        {
            if (value > 0)
            {
                divider = value;
            }
            else
            {
                throw new ArgumentException("Error: nutrition_divider in header must be greater than or equal to 1");
            }
        }
    }

    public int MapWidth
    {
        get { return mapWidth ?? throw new ArgumentException(WasNotSetMessage("map width")); }
        set
        {
            if (value > 0 && value < 10001)
            {
                mapWidth = value;
            }
            else
            {
                throw new ArgumentException("Error: map_width in header must be in the interval [1-10000]");
            }
        }
    }

    public int MapHeight
    {
        get { return mapHeight ?? throw new ArgumentException(WasNotSetMessage("Map Height")); }
        set
        {
            if (value > 0 && value < 10001)
            {
                mapHeight = value;
            }
            else
            {
                throw new ArgumentException("Error: map_height in header must be in the interval [1-10000]");
            }
        }
    }
    public Calculator1 calculator
    {

        get { return calc ?? throw new ArgumentException(WasNotSetMessage("calculator")); }
        set { calc = value; }

    }
    public void SetCalculator(int x, float size, float sense, float dexterity)
    {
        if (x >= 0 && size > 0 && sense > 0 && dexterity > 0)
        {
            switch (x)
            {
                case 0:
                    calculator = new Calculator1(size, sense, dexterity);
                    break;
                case 1:
                    calculator = new PlusCalculator(size, sense, dexterity);
                    break;
                case 2:
                    calculator = new KineticCalculator(size, sense, dexterity);
                    break;
                case 3:
                    calculator = new Calculator2(size, sense, dexterity);
                    break;
                default:
                    throw new ArgumentException("Error: calculator must be an integer smaller integer, not enough calculators");
            }
        }
        else
        {
            throw new ArgumentException("Error: calculator must be a non-negative integer");
        }
    }
}

/// <summary>
/// Class for loading parameters in the animal section of the input.
/// </summary>
public class AnimalSpecies
{
    private int id = -1; // If it is set i want to know which animal, if not than recognize it becaouse of -1  
    private string? name;
    private float? stopEat;
    private float? size;
    private float? sense;
    private float? dexterity;
    private float? reproduction;
    private float? mutation;
    private int? count;
    private NutritionsMultiplicators? food;
    private string WasNotSetMessage(string parameter)
    {
        return $"{parameter} of an animal with id{id} wasn't set!";
    }


    public void Print()
    {
        Console.WriteLine("Animal parameters:");
        Console.WriteLine("name: " + Name);
        Console.WriteLine("size: " + Size);
        Console.WriteLine("sense: " + Sense);
        Console.WriteLine("dexterity: " + Dexterity);
        Console.WriteLine("reproduction: " + Reproduction);
        Console.WriteLine("mutation: " + Mutation);
        Console.WriteLine("count: " + Count);
        Console.WriteLine("food multiplicators:");
        Console.WriteLine("    meat: " + Food.Meat);
        Console.WriteLine("    plant: " + Food.Plants);
        Console.WriteLine();
    }


    public int Id
    {
        get { return id; }
        set
        {
            if (value >= 0)
            {
                id = value;
            }
            else
            {
                throw new ArgumentException("id of animal must be positive or 0");
            };
        }
    }

    public string Name
    {
        get { return name ?? throw new ArgumentException(WasNotSetMessage("name")); }
        set
        {
            if (value.Length == 2)
            {
                name = value;
            }
            else
            {
                throw new ArgumentException("name of animal must have length 2");
            }
        }
    }

    public float StopEat
    {
        get { return stopEat ?? throw new ArgumentException("stopEat"); }
        set
        {
            if (value >= 1)
            {
                stopEat = value;
            }
            else
            {
                throw new ArgumentException("stop_eat of animal must be greater or equal to 1");
            }
        }
    }

    public float Size
    {
        get { return size ?? throw new ArgumentException(WasNotSetMessage("size")); }
        set
        {
            if (value > 0)
            {
                size = value;
            }
            else
            {
                throw new ArgumentException("size of animal must be positive");
            }
        }
    }

    public float Sense
    {
        get { return sense ?? throw new ArgumentException(WasNotSetMessage("sense")); }
        set
        {
            if (value >= 1)
            {
                sense = value;
            }
            else
            {
                throw new ArgumentException("sense of animal must be greater or equal to 1");
            }
        }
    }

    public float Dexterity
    {
        get { return dexterity ?? throw new ArgumentException(WasNotSetMessage("dexterity")); }
        set
        {
            if (value >= 0)
            {
                dexterity = value;
            }
            else
            {
                throw new ArgumentException("dexterity of animal must be greater or equal to 0");
            }
        }
    }

    public float Reproduction
    {
        get { return reproduction ?? throw new ArgumentException("reproduction"); }
        set
        {
            if (value >= 1)
            {
                reproduction = value;
            }
            else
            {
                throw new ArgumentException("reproduction of animal must be greater or equal to 1");
            }
        }
    }

    public float Mutation
    {
        get { return mutation ?? throw new ArgumentException("mutation"); }
        set
        {
            if (value >= 0 && value < 1)
            {
                mutation = value;
            }
            else
            {
                throw new ArgumentException("mutation of animal must be greater or equal to 0 and less than 1");
            }
        }
    }

    public int Count
    {
        get { return count ?? throw new ArgumentException("count"); }
        set
        {
            if (value >= 0)
            {
                count = value;
            }
            else
            {
                throw new ArgumentException("count of animal must be greater or equal to 0");
            }
        }
    }

    public NutritionsMultiplicators Food
    {
        get { return food ?? throw new ArgumentException(WasNotSetMessage("nutritions multiplicators")); }
        set
        {
            if (value.Plants >= 0 && value.Meat >= 0)
            {
                food = value;
            }
            else
            {
                throw new ArgumentException("nutritions multiplicators of animal must be greater or equal to 0");
            }
        }
    }
}

/// <summary>
/// Class for loading the food section of the input.
/// </summary>
public class PlantSpecies
{

    private int id = -1;
    private int actualCount; // is not input file parametr
    private string? name;
    private int? count;
    private float? size;
    private float? nourishment;
    private FoodChanger? changer;

    private string WasNotSetMessage(string parameter)
    {
        return $"{parameter} wasn't set in plant species{id}";
    }

    public void Print()
    {
        Console.WriteLine("Plant parameters:");
        Console.WriteLine("name: " + Name);
        Console.WriteLine("count: " + Count);
        Console.WriteLine("size: " + Size);
        Console.WriteLine("nourishment: " + Nourishment);
        Console.WriteLine("food changer:");
        Console.WriteLine("    min: " + Changer.Min);
        Console.WriteLine("    max: " + Changer.Max);
        Console.WriteLine("    interval: " + Changer.Interval);
        Console.WriteLine("    times: " + Changer.Times);
        Console.WriteLine();
    }

    public void DecreaseActualCount() {actualCount--;}


    public int Id
    {
        get { return id; }
        set
        {
            if (value >= 0)
            {
                id = value;
            }
            else
            {
                throw new ArgumentException("Id of plants must be greater or equal to 0");
            }
        }
    }

    public int ActualCount
    {
        get { return actualCount; }
        set { actualCount = value; }
    }

    public string Name
    {
        get { return name ?? throw new ArgumentException(WasNotSetMessage("Name")); }

        set
        {
            if (value.Length == 2)
            {
                name = value;
            }
            else
            {
                throw new ArgumentException("Length of the name must be 2 characters");
            }
        }
    }

    public int Count
    {
        get { return count ?? throw new ArgumentException(WasNotSetMessage(" count")); }
        set
        {
            if (value >= 0)
            {
                count = value;
            }
            else
            {
                throw new ArgumentException("Count of plants must be greater or equal to 0");
            }
        }
    }

    public float Size
    {
        get { return size ?? throw new ArgumentException(WasNotSetMessage("Size")); }
        set
        {
            if (value >= 0)
            {
                size = value;
            }
            else
            {
                throw new ArgumentException("Size of plants must be non-negative");
            }
        }
    }

    public float Nourishment
    {
        get { return nourishment ?? throw new ArgumentException(WasNotSetMessage("Nourishment")); }
        set
        {
            if (value > 0)
            {
                nourishment = value;
            }
            else
            {
                throw new ArgumentException("Nourishment of plants must be greater than 0");
            }
        }
    }

    public FoodChanger Changer
    {
        get { return changer ?? throw new ArgumentException(WasNotSetMessage("changer")); }
        set
        {
            if (value.Interval > 0 && value.Min > 0 && value.Max >= value.Min && value.Times >= 0)
            {
                changer = value;
            }
            else
            {
                throw new ArgumentException("Plant changer doesn't support these parameters");
            }
        }
    }
}



/// <summary>
/// Struct to pass read parameters.
/// </summary>
public class Parameters
{
    public Parameters()
    {
        header = new Header();
        Species = new List<AnimalSpecies>();
        Plants = new List<PlantSpecies>();
    }

    public Header header;
    public List<AnimalSpecies> Species;
    public List<PlantSpecies> Plants;
}

public class Argument
{
    public string Name { get; set; }
    public List<string> Values { get; set; }

    public Argument(string name, List<string> values)
    {
        this.Name = name;
        this.Values = values;
    }

}

public class Line
{
    private List<string> values;
    private int lineNumber;
    public Line(List<string> values, int number)
    {
        this.values = values;
        this.lineNumber = number;
    }

    public int LineNumber
    {
        get { return lineNumber; }
        set { lineNumber = value; }
    }

    public List<string> Values
    {
        get { return values; }
        set { values = value; }
    }
}


