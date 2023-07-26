public class InputReader
{
    private int lineNumber; //number of current line
    private char commentDeliminator;
    private static Char[] delimiters = {' ', '\t'};
    public InputReader()
    {
        lineNumber = 0;
        commentDeliminator = '#';
    }

    public Parameters GetParameters(string path)
    {
        using (var f = new StreamReader(path))
        {
            if (f.Peek() >= 0)
            {
                return ReadInput(f);
            }
            else
            {
                throw new FileNotFoundException($"{path} file not found");
            }
        }
    }
    /// <summary>
    /// load parameters from input file 
    /// </summary>
    /// <param name="str">input file</param>
    /// <returns> check and loaded parametes </returns>
    /// <exception cref="InvalidOperationException"></exception>
    private Parameters ReadInput(StreamReader str)
    {

        Parameters parameters = new Parameters();
        bool first_species = true;
        bool header_defined = false;
        HeaderChecker header_ch = new HeaderChecker();
        SpeciesChacker species_ch = new SpeciesChacker();
        RegularSpeaciesChacker r_species_ch = new RegularSpeaciesChacker();
        PlantChacker plants_ch = new PlantChacker();

        while (!str.EndOfStream)
        {
            Argument line = ReadLine(str);
            if (string.IsNullOrEmpty(line.Name))
                break;

            if (line.Name == "*head")
            {
                Dictionary<string, Line> section = new Dictionary<string, Line>();
                ReadSection(str, section);
                header_ch.CheckHeader(section, parameters.header);
                header_defined = true;
            }
            if (line.Name == "*plant")
            {
                Dictionary<string, Line> section = new Dictionary<string, Line>();
                ReadSection(str, section);

                parameters.Plants.Add(new PlantSpecies());
                PlantSpecies p = parameters.Plants[^1];
                plants_ch.CheckPlant(section, p);
                p.Id = parameters.Plants.Count;
            }
            if (line.Name == "*species")
            {
                if (first_species)
                {
                    Dictionary<string, Line> section = new Dictionary<string, Line>();
                    ReadSection(str, section);
                    parameters.Species.Add(new AnimalSpecies());
                    AnimalSpecies a = parameters.Species[^1];
                    r_species_ch.CheckAnimal(section, a);
                    a.Id = parameters.Species.Count;
                    first_species = false;
                }
                else
                {
                    Dictionary<string, Line> section = new Dictionary<string, Line>();
                    ReadSection(str, section);
                    parameters.Species.Add(new AnimalSpecies());
                    AnimalSpecies a = parameters.Species[^1];
                    species_ch.CheckAnimal(section, r_species_ch.GetRSpecies(), a);
                    a.Id = parameters.Species.Count;
                }
            }
        }

        if (!header_defined)
        {
            throw new InvalidOperationException("Header is not defined");
        }

        int items = 0;
        foreach (var plant in parameters.Plants)
        {
            items += plant.Count;
        }
        foreach (var s in parameters.Species)
        {
            items += s.Count;
        }
        if (parameters.header.MapHeight * parameters.header.MapWidth < items)
        {
            throw new InvalidOperationException("Too many items on the map");
        }

        return parameters;
    }

    private void ReadSection(StreamReader str, Dictionary<string, Line> parameters)
    {
        for (; ; )
        {
            Argument argument = ReadLine(str);
            if (string.IsNullOrEmpty(argument.Name) || argument.Name[0] == '-')
            {
                break;
            }
            else
            {
                parameters.Add(argument.Name, new Line(argument.Values, lineNumber));
            }
        }
    }

    private Argument ReadLine(StreamReader str)
    {
        List<string> parameters = new List<string>();
        string? rawLine;
        string firstWordInLine = string.Empty;

        while (firstWordInLine == string.Empty)
        {
            lineNumber++;
            rawLine = str.ReadLine();
            if (rawLine == null)
            {
                return new Argument(firstWordInLine, parameters);
            }
            rawLine = ThrowComment(rawLine);

            foreach (string word in rawLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
            {
                if (firstWordInLine == string.Empty)
                {
                    firstWordInLine = word;
                }
                else
                {
                    parameters.Add(word);
                }
            }

            if (!string.IsNullOrEmpty(firstWordInLine))
            {
                return new Argument(firstWordInLine, parameters);
            }
        }
        return new Argument(firstWordInLine, parameters);
    }

    private string ThrowComment(string line)
    {
        int index = line.IndexOf(commentDeliminator);
        if (index >= 0)
        {
            line = line.Substring(0, index);
        }
        return line;
    }
}

