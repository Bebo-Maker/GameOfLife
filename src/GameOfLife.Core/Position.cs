namespace GameOfLife.Core;

public struct Position
{
  public int X { get; }
  public int Y { get; }

  public Position(int x, int y)
  {
    X = x;
    Y = y;
  }

  public (int X, int Y) Deconstruct() => (X, Y);

  public static implicit operator Position((int X, int Y) d) => new(d.X, d.Y);
}
