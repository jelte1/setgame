using Microsoft.EntityFrameworkCore;

namespace backend.Entities;

public enum Shape { Oval, Diamond, Squiggle }

public enum Color { Red, Purple, Green }

public enum Filling { Solid, Striped, Empty }

public enum Amount { One, Two, Three }

public class Card
{
    public int Id { get; set; }
    
    public Shape Shape { get; set; }
    
    public Color Color { get; set; }
    
    public Filling Filling { get; set; }
    
    public Amount Amount { get; set; }
}