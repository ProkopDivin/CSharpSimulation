using System.Drawing;

public struct Coordinates : IEquatable<Coordinates>  
{
    public int X;
    public int Y;

    public Coordinates(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
    public int Distance()
    {
        return Math.Abs(X) + Math.Abs(Y);
    }
    public int Distance(Coordinates p)
    {
        return Math.Abs(X - p.X) + Math.Abs(Y - p.Y);
    }
    public double Length()
    {
        return Math.Sqrt(X * X + Y * Y);
    }
    public Coordinates Direction(Coordinates c)
    {
        return new Coordinates(c.X - X, c.Y - Y);
    }
    public bool Equals(Coordinates other)
    {
        return X == other.X && Y == other.Y;
    }
    public override int GetHashCode()
    {
        return (X + Y) * (X + Y + 1) / 2 + X;
    }
}


/// <summary>
/// generate coordinates of searcheble fields for animal relative to his pozition
/// </summary>
public static class SearchingCoordinates
{
    public static IEnumerable<Coordinates> Get(int min, int max) //serchin from closes to futherles
    {
        for (int lenght = min; lenght < max; lenght++)
        {
            for (int x = 0; x <= lenght; x++)
            {
                int y = lenght - x;
                yield return new Coordinates(x, y);  // first qadrant
                if (x != 0 && y != 0)    // to not return the same coordinates on x an y exes 
                {
                    yield return new Coordinates(x, -y);   //second quadrant
                    yield return new Coordinates(-x, y);  //fourth quadrant 
                }
                yield return new Coordinates(-x, -y);     //third quadrant

            }
        }
    }
}


