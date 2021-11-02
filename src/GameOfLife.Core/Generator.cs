namespace GameOfLife.Core
{
  public static class Generator
  {
    private static readonly Random Random = new();
    
    public static Cell[,] RandomCells(int cols, int rows)
    {
      var cells = new Cell[cols, rows];
      for (int x = 0; x < cols; x++)
        for (int y = 0; y < rows; y++)
          cells[x, y] = RandomCell();

      return cells;
    }
    
    public static Cell RandomCell() => Random.Next(0, 3) == 0 ? Cell.Alive : Cell.Dead;
  }
}
