using MegaCrit.Sts2.Core.Models;
using Hellkite.HellkiteCode.DynamicVars;

namespace Hellkite.HellkiteCode.Structs;

public struct FireUp(int charge)
{
    public FireUp() : this(0)
    {
    }
    

    /// <summary>
    /// Construct FireUp struct with FireUp values in the DynamicVars of the provided CardModel
    /// </summary>
    /// <param name="cardModel"></param>
    public FireUp(CardModel cardModel) : this()
    {
        Charge = cardModel.DynamicVars.TryGetValue(FireUpVar.defaultName, out var var) ? var.IntValue : 0;
    }

    public static FireUp WithCharge(int cost)
    {
        return new FireUp(cost);
    }

    public int ByIndex(int index)
    {
        return index switch
        {
            0 => Charge,
            _ => 0
        };
    }

    public int Charge { get; set; } = charge;

    public void SetFireUp(int charge)
    {
        Charge = charge;
    }

    public int Total => Charge;

    public static FireUp operator +(FireUp a)
    {
        return a;
    }

    public static FireUp operator -(FireUp a)
    {
        return new FireUp(-a.Charge);
    }

    public static FireUp operator +(FireUp a, FireUp b)
    {
        return new FireUp(a.Charge + b.Charge);
    }

    public static FireUp operator -(FireUp a, FireUp b)
    {
        return new FireUp(a.Charge - b.Charge);
    }

    public static bool operator ==(FireUp a, FireUp b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(FireUp a, FireUp b)
    {
        return !(a == b);
    }

    public static FireUp operator /(FireUp a, int divisor)
    {
        return new FireUp(a.Charge / divisor);
    }

    public static FireUp operator *(FireUp a, int mult)
    {
        return new FireUp(a.Charge * mult);
    }

    public bool Equals(FireUp other)
    {
        return Charge == other.Charge;
    }

    public override bool Equals(object? obj)
    {
        return obj is FireUp other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Charge);
    }

    public override string ToString()
    {
        return $"charge {Charge}";
    }

    public FireUp ClampZero()
    {
        Charge = Math.Max(Charge, 0);

        return this;
    }
    
    public FireUp Clamp(int minimum, int maximum)
    {
        Charge = Math.Clamp(Charge, minimum, maximum);
        return this;
    }

    public void Max(FireUp other)
    {
        Charge = Math.Max(Charge, other.Charge);
    }

    public bool CanSpend(FireUp other)
    {
        return Charge >= other.Charge;
    }

    public int SubtractSequential(int sub)
    {
        var rem = Charge - sub;
        Charge = Math.Max(0, rem);
        sub = Math.Max(0, -rem);
        return sub;
    }
}