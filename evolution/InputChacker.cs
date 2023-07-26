public abstract class Checker
{
    //check if there is right number of values, when seting property
    protected abstract void CheckCount(int expectedCount, Line parameters, string name);
}

/// <summary>
/// check parameters in header and convert them from string to their expected type.
/// </summary>
public class HeaderChecker : Checker
{
    //the folowing is protected just to have easy way to create new type of header
    protected static readonly List<string> head_param = new List<string> { "days", "map_hight", "map_width", "calculator", "nutritions_divider" };

    protected override void CheckCount(int expectedCount, Line parameters, string name)
    {
        if (parameters.Values.Count != expectedCount)
        {
            throw new ArgumentException($"*head \"{name}\" wrong number of arguments at line {parameters.LineNumber}");
        }
    }

    public void CheckHeader(Dictionary<string, Line> parameters, Header header)
    {
        if (parameters.Count > head_param.Count)
        {
            throw new InvalidOperationException("Too many parameters in header definition");
        }

        foreach (string parameterName in head_param)
        {
            if (!parameters.ContainsKey(parameterName))
            {
                throw new InvalidOperationException($"{parameterName} is not set in header declaration");
            }
            else
            {
                ProcesHeaderParams(header, parameters[parameterName], parameterName);
            }
        }
    }

    private void ProcesHeaderParams(Header header, Line parameter, string parameterName)
    {
        List<string> stringParam = parameter.Values;

        switch (parameterName)
        {
            case "days":
                CheckCount(1, parameter, parameterName);
                header.Days = int.Parse(stringParam[0]);
                break;
            case "map_hight":
                CheckCount(1, parameter, parameterName);
                header.MapHeight = int.Parse(stringParam[0]);
                break;
            case "map_width":
                CheckCount(1, parameter, parameterName);
                header.MapWidth = int.Parse(stringParam[0]);
                break;
            case "nutritions_divider":
                CheckCount(1, parameter, parameterName);
                header.Divider = float.Parse(stringParam[0]);
                break;
            case "calculator":
                CheckCount(4, parameter, parameterName);
                header.SetCalculator(int.Parse(stringParam[0]), float.Parse(stringParam[1]), float.Parse(stringParam[2]), float.Parse(stringParam[3]));
                break;
            default:
                throw new ArgumentException($"{parameterName} isn't a header parameter");
        }

    }
}

/// <summary>
/// check parameters for the first animal and convert them from string to their expected type.
/// this animal is used as a regular animal, which means that if in another animal something missing than value from this animal is used. 
/// </summary>
public class RegularSpeaciesChacker : Checker
{
    protected static readonly List<string> animal_param = new List<string> { "name", "size", "sense", "dexterity", "reproduction", "mutation", "count", "food", "stop_eat" };
    private AnimalSpecies regularAnimal = new AnimalSpecies();

    protected override void CheckCount(int expectedCount, Line parameters, string name)
    {
        if (parameters.Values.Count != expectedCount)
        {
            throw new ArgumentException($"*food \"{name}\" wrong number of arguments at line {parameters.LineNumber}");
        }
    }

    public void CheckAnimal(Dictionary<string, Line> parameters, AnimalSpecies animal)
    {
        foreach (string parameterName in animal_param)
        {
            if (!parameters.ContainsKey(parameterName))
            {
                throw new InvalidOperationException($"{parameterName} is not set in first species declaration");
            }
            else
            {
                ProcesSpeciesParams(animal, parameters[parameterName], parameterName);
            }
        }

        regularAnimal = animal;
    }

    public AnimalSpecies GetRSpecies()
    {
        return regularAnimal;
    }

    protected void ProcesSpeciesParams(AnimalSpecies animal, Line parameters, string parameterName)
    {
        List<string> stringParam = parameters.Values;

        switch (parameterName)
        {
            case "name":
                CheckCount(1, parameters, parameterName);
                animal.Name = stringParam[0];
                break;
            case "size":
                CheckCount(1, parameters, parameterName);
                animal.Size = float.Parse(stringParam[0]);
                break;
            case "sense":
                CheckCount(1, parameters, parameterName);
                animal.Sense = float.Parse(stringParam[0]);
                break;
            case "dexterity":
                CheckCount(1, parameters, parameterName);
                animal.Dexterity = float.Parse(stringParam[0]);
                break;
            case "reproduction":
                CheckCount(1, parameters, parameterName);
                animal.Reproduction = float.Parse(stringParam[0]);
                break;
            case "mutation":
                CheckCount(1, parameters, parameterName);
                animal.Mutation = float.Parse(stringParam[0]);
                break;
            case "count":
                CheckCount(1, parameters, parameterName);
                animal.Count = int.Parse(stringParam[0]);
                break;
            case "food":
                CheckCount(2, parameters, parameterName);
                float meet = float.Parse(stringParam[0]);
                float food = float.Parse(stringParam[1]);
                animal.Food = new NutritionsMultiplicators(meet, food);
                break;
            case "stop_eat":
                CheckCount(1, parameters, parameterName);
                animal.StopEat = float.Parse(stringParam[0]);
                break;
            default:
                throw new ArgumentException($"{parameterName} isn't a species parameter");
        }

    }
}

/// <summary>
/// check the plant parameters and convert them from string to their expected type.
/// </summary>
public class PlantChacker : Checker
{
    protected static readonly List<string> plantParam = new List<string> { "name", "size", "count", "nourishment", "changer" };

    protected override void CheckCount(int expectedCount, Line parameters, string name)
    {
        if (parameters.Values.Count != expectedCount)
        {
            throw new ArgumentException($"*species \"{name}\" wrong number of arguments at line {parameters.LineNumber}");
        }
    }

    public void CheckPlant(Dictionary<string, Line> parameters, PlantSpecies plant)
    {
        if (parameters.Count > plantParam.Count)
        {
            throw new InvalidOperationException("Too many parameters in plant definition");
        }

        foreach (string parameterName in plantParam)
        {
            if (!parameters.ContainsKey(parameterName))
            {
                throw new InvalidOperationException($"{parameterName} is not set in plant declaration");
            }
            else
            {
                ProcesPlantsParams(plant, parameters[parameterName], parameterName);
            }
        }
    }

    private void ProcesPlantsParams(PlantSpecies plant, Line parameter, string parameterName)
    {
        List<string> stringParam = parameter.Values;

        switch (parameterName)
        {
            case "size":
                CheckCount(1, parameter, parameterName);
                plant.Size = float.Parse(stringParam[0]);
                break;
            case "name":
                CheckCount(1, parameter, parameterName);
                plant.Name = stringParam[0];
                break;
            case "count":
                CheckCount(1, parameter, parameterName);
                plant.Count = int.Parse(stringParam[0]);
                break;
            case "nourishment":
                CheckCount(1, parameter, parameterName);
                plant.Nourishment = float.Parse(stringParam[0]);
                break;
            case "changer":
                CheckCount(4, parameter, parameterName);
                int interval = int.Parse(stringParam[0]);
                float times = float.Parse(stringParam[1]);
                int max = int.Parse(stringParam[2]);
                int min = int.Parse(stringParam[3]);
                plant.Changer = new FoodChanger(interval, times, max, min);
                break;
            default:
                throw new ArgumentException($"{parameterName} isn't a plant parameter");
        }
    }
}

/// <summary>
/// chack species parameters and fill the missing parameter 
/// </summary>
public class SpeciesChacker : RegularSpeaciesChacker
{
    public void CheckAnimal(Dictionary<string, Line> parameters, AnimalSpecies rSpecies, AnimalSpecies species)
    {
        if (parameters.Count > animal_param.Count)
        {
            throw new InvalidOperationException("Too many parameters in species definition");
        }

        foreach (string parameterName in animal_param)
        {
            if (!parameters.ContainsKey(parameterName))
            {
                SetAnimalParams(species, parameterName, rSpecies);
            }
            else
            {
                ProcesSpeciesParams(species, parameters[parameterName], parameterName);
            }
        }

        foreach (KeyValuePair<string, Line> pair in parameters)
        {
            if (!animal_param.Contains(pair.Key))
            {
                throw new InvalidOperationException($"{pair.Key} on line: {pair.Value.LineNumber} is not a parameter");
            }
        }
    }

    private void SetAnimalParams(AnimalSpecies animal, string parameterName, AnimalSpecies regularAnimal)
    {
        switch (parameterName)
        {
            case "size":
                animal.Size = regularAnimal.Size;
                break;
            case "sense":
                animal.Sense = regularAnimal.Sense;
                break;
            case "dexterity":
                animal.Dexterity = regularAnimal.Dexterity;
                break;
            case "reproduction":
                animal.Reproduction = regularAnimal.Reproduction;
                break;
            case "mutation":
                animal.Mutation = regularAnimal.Mutation;
                break;
            case "count":
                animal.Count = regularAnimal.Count;
                break;
            case "food":
                animal.Food = new NutritionsMultiplicators(regularAnimal.Food.Meat, regularAnimal.Food.Plants);
                break;
            case "name":
                animal.Name = regularAnimal.Name;
                break;
            case "stop_eat":
                animal.StopEat = regularAnimal.StopEat;
                break;
            default:

                throw new ArgumentException("Invalid parameter_name: " + parameterName);
        }
    }
}
