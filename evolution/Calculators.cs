
/// <summary>
/// clas with function calculating hunger
/// </summary>
public class Calculator1
{
    /// <summary>
    /// average parameters, sizeForCalculatingHunger = sizeOfAnima / rSize 
    /// and so on
    /// </summary>
    protected float rSize;
    protected float rSense;
    protected float rDexterity;

    /// <summary>
    /// This calculator takes inspiration from formula for kynetic energy (1/2 * m^3 * v^2). 
    /// m is estimated by size
    /// v is estimated by dexterity
    /// the 1/2 was thrown out, wi can use or rSize or rDexterity for substitution
    /// sense is just added, to have some penalyzation faktor
    /// <summary>
    public Calculator1()
    {
        rSense = -1;
        rSize = -1;
        rDexterity = -1;
    }

    public Calculator1(float size, float sense, float dexterity)
    {
        rSize = size;
        rSense = sense;
        rDexterity = dexterity;
    }


    public virtual float Hunger(float size, float sense, float dexterity)
    {
        size = size / rSize;
        sense = sense / rSense;
        dexterity = dexterity / rDexterity;
        return (float)(Math.Pow(size, 3) * Math.Pow(dexterity, 2) + sense);
    }

    public override string ToString()
    {
        return $"id: {GetId()}, rsize: {rSize}, rsense: {rSense}, rdexterity: {rDexterity}";
    }

    protected virtual int GetId()
    {
        return 0;
    }
}

/// <summary>
/// Simulate situation where every animal property has the same panalization factor.
/// </summary>
public class PlusCalculator : Calculator1
{
    public PlusCalculator(float size, float sense, float dexterity)
        : base(size, sense, dexterity)
    {
    }

    public override float Hunger(float size, float sense, float dexterity)
    {
        size = size / rSize;
        sense = sense / rSense;
        dexterity = dexterity / rDexterity;
        return size + dexterity + sense;
    }
}

/// <summary>
/// Same as Calculator1 but sense is to the 2nd power
/// </summary>
public class KineticCalculator : Calculator1
{
    public KineticCalculator(float size, float sense, float dexterity)
        : base(size, sense, dexterity)
    {
    }

    public override float Hunger(float size, float sense, float dexterity)
    {
        size = size / rSize;
        sense = sense / rSense;
        dexterity = dexterity / rDexterity;
        return (float)(Math.Pow(size, 3) * Math.Pow(dexterity, 2) + Math.Pow(sense, 2));
    }
}
/// <summary>
/// Same as Calculator1 but size is to the 2nd power
/// </summary>
public class Calculator2 : Calculator1
{
    public Calculator2(float size, float sense, float dexterity)
        : base(size, sense, dexterity)
    {
    }

    public override float Hunger(float size, float sense, float dexterity)
    {
        size = size / rSize;
        sense = sense / rSense;
        dexterity = dexterity / rDexterity;
        return (float)(Math.Pow(size, 2) * Math.Pow(dexterity, 2) + sense);
    }
}