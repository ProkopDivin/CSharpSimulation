/// <summary>
/// Represents an item on the map. actualy on the map is only its encestors
/// </summary>
public class Item { }

/// <summary>
/// Represents a food item on the map.
/// </summary>
public class Food : Item
{

    public PlantSpecies Plant; //reference to a kind of plant 
    public Food(PlantSpecies p)
    {
        Plant = p;
    }

}


public class Animal : Item
{

    private readonly static int[][] relativeMoves = new int[][]{
        new int[]{1, 0},
        new int[]{-1,0},
        new int[]{0,-1},
        new int[]{0,1}};

    private readonly static int[][] mutations = new int[][]
    {
        new int[] { 1, 1, 1 },
        new int[] { 1, 1, -1 },
        new int[] { 1, -1, 1 },
        new int[] { 1, -1, -1 },
        new int[] { -1, 1, 1 },
        new int[] { -1, 1, -1 },
        new int[] { -1, -1, 1 },
        new int[] { -1, -1, -1 }
    };
    public float eaten;

    public float Size { get; set; }
    public float Sense { get; set; }
    public float Dexterity { get; set; }
    public AnimalSpecies Species { get; set; }
    public float Eaten { get; set; }


    public Animal(float size, float sense, float dexterity, AnimalSpecies species)
    {
        Size = size;
        Sense = sense;
        Dexterity = dexterity;
        Species = species;
    }


    /// <summary>
    /// Makes a new animal with mutated traits based on the predecestor of animal.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <returns>The new mutated animal.</returns>
    public Animal MakeAnimal(Random random)
    {
        int roll = random.Next(8);
        float new_size = Size + mutations[roll][0] * Size * Species.Mutation;
        float new_sense = Sense + mutations[roll][1] * Sense * Species.Mutation;
        float new_dexterity = Dexterity + mutations[roll][2] * Dexterity * Species.Mutation;

        return new Animal(new_size, new_sense, new_dexterity, Species);
    }


    /// <summary>
    /// Updates the hunger state of the animal when eating.
    /// </summary>
    /// <param name="item">The possible food source.</param>
    /// <param name="max_food">The maximum amount of food the animal can eat.</param>
    public void Eat(Item? item, float maxFood)
    {
        if (item is not null)
        {
            if (item is Animal animal)
            {
                Eaten += animal.Size * Species.Food.Meat;
            }
            else
            {
                Eaten += ((Food)item).Plant.Nourishment * Species.Food.Plants;
                ((Food)item).Plant.DecreaseActualCount();
            }
            Eaten = Math.Min(maxFood, Eaten);
        }

    }

    /// <summary>
    /// Makes a step for the animal.
    /// </summary>
    /// <param name="map">The environment map.</param>
    /// <param name="coor">The current coordinates of the animal.</param>
    /// <param name="turn">The current turn of the animal.</param>
    /// <param name="random">The random number generator.</param>
    /// <returns>The new coordinates when animal will go.</returns>
    public Coordinates MakeStep(Map map, Coordinates coor, int turn, int rounds)
    {
        int step;
        int wholeDexterity = (int)Dexterity;
        if (turn == rounds - 1)
        {
            step = (wholeDexterity / rounds) + (wholeDexterity % rounds);
        }
        else
        {
            step = wholeDexterity / rounds;
        }
        Coordinates somethingToEat = SearchAround(map, coor, step);
        float hunger = map.calculator.Hunger(Size, Sense, Dexterity);

        if (somethingToEat.X == -1 || Eaten > Species.StopEat * Species.Reproduction * hunger)
        {
            return RandomEmptyBox(map, coor, step);
        }
        else
        {
            if (somethingToEat.Distance(coor) > step)
            {
                return GoCloser(map, coor, coor.Direction(somethingToEat), step);
            }
            else
            {
                return somethingToEat;
            }
        }
    }
    /// <summary>
    /// meke searching of surroundings of animal
    /// </summary>
    /// <param name="map"> enviroment</param>
    /// <param name="coor">coordinates of animal who is looking around</param>
    /// <param name="step">movemen of animal is in steps, this is number of current step</param>
    /// <returns> coordinates with most wanted food source</returns>
    private Coordinates SearchAround(Map map, Coordinates coor, int step)
    {
        Coordinates somethingToEat = new Coordinates();
        int isense = (int)Sense;
        float maxMeal = 0;
        bool reachable = true;
        foreach (Coordinates relativeMove in SearchingCoordinates.Get(1, isense))
        {
            Coordinates c = new Coordinates(relativeMove.X + coor.X, relativeMove.Y + coor.Y);
            if (c.X >= 0 && c.X < map.Width && c.Y >= 0 && c.Y < map.Height)
            {
                Item? item = map.map[c.X, c.Y];
                if (item != null)
                {
                    float meal = CalcMeal(item);
                    if (meal > maxMeal)
                    {
                        if (reachable)
                        {
                            maxMeal = meal;
                            somethingToEat.X = c.X;
                            somethingToEat.Y = c.Y;
                        }
                        else if (maxMeal == 0)
                        {
                            maxMeal = meal;
                            somethingToEat.X = c.X;
                            somethingToEat.Y = c.Y;
                        }
                    }
                    if (relativeMove.Length() > step)
                    {
                        if (reachable && maxMeal > 0)
                        {
                            return somethingToEat;
                        }
                        reachable = false;
                    }
                }
            }
        }
        return maxMeal > 0 ? somethingToEat : new Coordinates(-1, -1); //didn't find food source -> coordinate [-1, -1]
    }

    // return reachebel coordinates of empty box or [-1,-1]
    private static Coordinates RandomEmptyBox(Map m, Coordinates coor, int step)
    {
        for (int i = step; i > 0; i--)
        {
            int roll = m.random.Next(i);
            for (int j = 0; j <= i; j++)
            {
                int x = (i - roll);
                int y = (roll);
                int randomIndex = m.random.Next(4);
                for (int k = randomIndex; k < 4 + randomIndex; k++)
                {
                    int index = k % 4;
                    Coordinates c = new Coordinates(x * relativeMoves[index][0] + coor.X, y * relativeMoves[index][1] + coor.Y);
                    if (m.IsEmpty(c))
                    {
                        return c;
                    }
                }
                roll = (roll + 1) % (i + 1);
            }
        }
        return coor;
    }

    private static Coordinates GoCloser(Map m, Coordinates coor, Coordinates direction, int step)
    {
        int mx = 1;
        int my = 1;
        if (direction.X < 0)
        {
            mx = -1;
        }
        if (direction.Y < 0)
        {
            my = -1;
        }
        for (int i = step; i > 0; i--)
        {
            for (int j = 0; j <= i; j++)
            {
                if (i - j <= Math.Abs(direction.X) && j <= Math.Abs(direction.Y))
                {
                    Coordinates c = new Coordinates((i - j) * mx + coor.X, j * my + coor.Y);
                    if (m.IsEmpty(c))
                    {
                        return c;
                    }
                }
            }
        }
        return new Coordinates(coor.X, coor.Y);
    }


    private float CalcMeal(Item item)
    {
        float meal;
        if (item is Food food)
        {
            if (food.Plant.Size > Size)
            {
                meal = -1;
            }
            else
            {
                meal = Species.Food.Plants * food.Plant.Nourishment;
            }
        }
        else
        {
            if (((Animal)item).Species.Id == Species.Id || ((Animal)item).Size > Size)
            {
                meal = -1;
            }
            else
            {
                meal = Species.Food.Meat * ((Animal)item).Size;
            }
        }
        return meal;
    }

}


/// <summary>
/// Provides methods for printing the map.
/// </summary>
public static class MapPrinter
{
    /// <summary>
    /// Prints the map to the specified file.
    /// </summary>
    /// <param name="map">The environment map.</param>
    /// <param name="day">The current day.</param>
    /// <param name="file">The output file stream.</param>
    public static void PrintMap(Item?[,] map, string day, StreamWriter file)
    {
        int cellWidth = 3;
        int rowWidth = cellWidth * map.GetLength(1) + 1;
        file.WriteLine("day: " + day);
        PrintRowSeparator(file, rowWidth);
        for (int i = 0; i < map.GetLength(0); i++)
        {
            PrintRowSeparator(file, rowWidth);
            for (int j = 0; j < map.GetLength(1); j++)
            {
                file.Write("|");
                Item? item = map[i, j];
                if (item is not null)
                {
                    if (item is Animal animal)
                    {
                        file.Write(animal.Species.Name);
                    }
                    else if (item is Food food)
                    {
                        file.Write(food.Plant.Name);
                    }
                }
                else
                {
                    file.Write("  ");
                }
            }
            file.WriteLine("|");
        }
        PrintRowSeparator(file, rowWidth);
    }

    /// <summary>
    /// Prints a row separator to the specified file.
    /// </summary>
    /// <param name="file">The output file stream.</param>
    /// <param name="length">The length of the separator.</param>
    private static void PrintRowSeparator(System.IO.StreamWriter file, int length)
    {
        for (int i = 0; i < length; i++)
        {
            file.Write("-");
        }
        file.WriteLine();
    }
}

/// <summary>
/// Provides methods for loading the map and setting up the simulation.
/// </summary>
public class MapLoading
{
    protected int tryes = 5;
    public Random random = new Random(43);
    protected List<PlantSpecies> plants;
    protected List<AnimalSpecies> species;

    public int Width { get; set; }
    public int Height { get; set; }

    public Item?[,] map;

    public Calculator1 calculator;
    public MapLoading(Parameters parameters)
    {
        this.plants = parameters.Plants;
        this.species = parameters.Species;
        map = new Item[parameters.header.MapWidth, parameters.header.MapHeight];
        calculator = parameters.header.calculator;
    }


    protected void FillMap()
    {
        PutAnimals();
        PutFood();
    }


    protected void PutAnimals()
    {
        int index = 0;
        foreach (AnimalSpecies sp in species)
        {
            for (int i = 0; i < sp.Count; ++i)
            {
                Animal a = new Animal(sp.Size, sp.Sense, sp.Dexterity, sp);
                PlaceItem(a);
            }
            index++;
        }
    }

    /// <summary>
    /// Places the food on the map.
    /// </summary>
    protected void PutFood()
    {
        foreach (PlantSpecies f in plants)
        {
            for (int i = f.ActualCount; i < f.Count; ++i)
            {
                Food food = new Food(f);
                PlaceItem(food);
            }
            f.ActualCount = f.Count;
        }
    }

    /// <summary>
    /// Generates random coordinates.
    /// </summary>
    /// <returns>The generated coordinates.</returns>
    protected Coordinates RandomCoordinates()
    {
        int x = random.Next(Width);
        int y = random.Next(Height);
        for (int i = 0; i < tryes; i++) // frst couple random tryes
        {
            if (map[x, y] == null)
            {
                return new Coordinates(x, y);
            }
            x = random.Next(Width);
            y = random.Next(Height);
        }
        long startIndex = x * Width + y;
        long places = Width * Height;
        for (long i = startIndex; i < places + startIndex; i++)   //then go systematicaly
        {
            if (map[x, y] == null)
            {
                return new Coordinates(x, y);
            }
            long index = i % places;
            x = (int)(index / Height);
            y = (int)(index % Height);
        }
        if (map[x, y] == null)
        {
            return new Coordinates(x, y);
        }
        else
        {
            return new Coordinates(-1, -1);
        }
    }

    /// <summary>
    /// Places an item on the map.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="item">The item to place on the map.</param>
    /// <returns>The coordinates where the item is placed.</returns>
    protected Coordinates PlaceItem(Item item)
    {
        Coordinates coor = RandomCoordinates();
        if (coor.X > -1 && coor.Y > -1)
        {
            map[coor.X, coor.Y] = item;
        }
        return coor;
    }
}
/// <summary>
/// Represents the map and simulates one day of the simulation.
/// </summary>
public class Map : MapLoading
{
    public HashSet<Coordinates> AnimalsCoordinates = new HashSet<Coordinates>();
    protected HashSet<Coordinates> takenFields = new HashSet<Coordinates>();
    protected float divider;
    private static readonly int rounds = 3;
    public Map(Parameters parameters) : base(parameters)
    {
        Width = parameters.header.MapWidth;
        Height = parameters.header.MapHeight;
        plants = parameters.Plants;
        species = parameters.Species;
        divider = parameters.header.Divider;
        FillMap();
        FindAnimals();
    }

    /// <summary>
    /// Gets the environment map.
    /// </summary>
    /// <returns>The environment map.</returns>
    public Item?[,] GetMap()
    {
        return map;
    }

    /// <summary>
    /// Simulates one day of the simulation.
    /// </summary>
    /// <param name="date">The current date.</param>
    public void Day(int date)
    {
        MoveAnimals();
        var newGeneration = new HashSet<Coordinates>();
        foreach (Coordinates c in AnimalsCoordinates)
        {
            // AnimalsCoordinates prvky at animal coordinates shouldn´t be null,
            // if it is so, than wrong coordinates were added --> want  exception
            Animal a = (Animal)map[c.X, c.Y]; 

            float hunger = calculator.Hunger(a.Size, a.Sense, a.Dexterity);
            a.Eaten -= hunger;

            if (a.Eaten < 0)
            {
                map[c.X, c.Y] = null;  //animal die becaouse of hunger 
            }
            else
            {
                a.Eaten += hunger;  // animal will reproduce maybe 
                if (a.Eaten > a.Species.Reproduction * hunger)
                {
                    Animal newAnimal = a.MakeAnimal(random);
                    Coordinates newCoord = PlaceItem(newAnimal);
                    if (newCoord.X > -1)
                    {
                        newGeneration.Add(newCoord);
                        a.Eaten -= hunger * a.Species.Reproduction;
                    }
                    else
                    {
                        a.Eaten -= hunger;  // not enought space for animal cant reproduce
                    }
                }
                newGeneration.Add(c);
                a.Eaten /= divider;
            }
        }
        AnimalsCoordinates = newGeneration;
        ChangeFoodCount(date);
        PutFood();
    }


    /// <summary>
    /// Gets the calculator used for calculating hunger.
    /// </summary>
    /// <returns>The calculator used for calculating hunger.</returns>
    public Calculator1 GetCalculator()
    {
        return calculator;
    }



    /// <summary>
    /// Checks if the specified coordinates are empty on the map.
    /// </summary>
    /// <param name="coor">The coordinates to check.</param>
    /// <returns>True if the coordinates are empty, false otherwise.</returns>
    public bool IsEmpty(Coordinates c)
    {
        if (Width > c.X && c.X > -1 && Height > c.Y && c.Y > -1)
        {
            if (map[c.X, c.Y] == null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Gets the animal species in the map.
    /// </summary>
    /// <returns>The list of animal species.</returns>
    public List<AnimalSpecies> GetSpecies()
    {
        return species;
    }

    /// <summary>
    /// Gets the plant species in the map.
    /// </summary>
    /// <returns>The list of plant species.</returns>
    public List<PlantSpecies> GetPlants()
    {
        return plants;
    }

    /// <summary>
    /// Gets the coordinates of the animals on the map.
    /// </summary>
    /// <returns>The set of animal coordinates.</returns>
    public HashSet<Coordinates> GetAnimalCoor()
    {
        return AnimalsCoordinates;
    }
    /// <summary>
    /// this will find animal on the map and add their coordinations to animal coordinates
    /// </summary>
    protected void FindAnimals()
    {
        AnimalsCoordinates = new HashSet<Coordinates>();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Item? item = map[i, j];
                if (item != null && item.GetType() == typeof(Animal))
                {
                    AnimalsCoordinates.Add(new Coordinates(i, j));
                }
            }
        }
    }


    protected void MoveAnimals()
    {
        for (int i = 0; i < rounds; i++)
        {
            var actualCoordinates = new HashSet<Coordinates>();
            foreach (Coordinates from in AnimalsCoordinates)
            {
                Item? fromItem = map[from.X, from.Y];
                if (!takenFields.Contains(from) && fromItem != null)
                {
                    Animal a = (Animal)fromItem;
                    map[from.X, from.Y] = null;    //remove old animal from the map
                    Coordinates to = a.MakeStep(this, from, i, rounds);
                    if (from.X != to.X || from.Y != to.Y)
                    {
                        float maxFood = a.Species.StopEat * a.Species.Reproduction * calculator.Hunger(a.Size, a.Sense, a.Dexterity);
                        a.Eat(map[to.X, to.Y], maxFood);
                    }
                    takenFields.Add(to);
                    map[to.X, to.Y] = a;


                    actualCoordinates.Add(to);
                }
            }
            AnimalsCoordinates = actualCoordinates;  //actualization of coordinates of animals on the map 
            takenFields.Clear();
        }
    }

    public void ChangeFoodCount(int date)
    {
        foreach (PlantSpecies f in plants)
        {
            FoodChanger changer = f.Changer;
            if (date % changer.Interval == 0)
            {
                int x = (int)(changer.Times * f.Count);
                x = Math.Min(changer.Max, x);
                x = Math.Max(changer.Min, x);
                f.Count = x;
            }
        }
    }

}





