namespace GameOfLife.Core;

public enum Cell
{
  Alive,
  Dead
}

public static class CellExtensions
{
  public static Cell Invert(this Cell cell)
  {
    return cell switch
    {
      Cell.Dead => Cell.Alive,
      _ => Cell.Dead
    };
  }
}