namespace GameOfLife.Core;

public class Game
{
  private readonly Position[] _neighbourCoords = new Position[] { (-1, -1), (-1, 0), (-1, 1), (0, 1), (0, -1), (1, 1), (1, -1), (1, 0) };

  private readonly Cell[,] _cells;

  public int Rows { get; }
  public int Cols { get; }

  public Game(int rows, int cols)
  {
    Rows = rows;
    Cols = cols;

    _cells = new Cell[rows, cols];
  }

  public Game(Cell[,] cells)
  {
    Cols = cells.GetLength(0);
    Rows = cells.GetLength(1);
    _cells = cells;
  }

  public Cell Cell(int x, int y) => _cells[x, y];
  public Cell Cell(Position p) => Cell(p.X, p.Y);

  public bool Cycle()
  {
    MutateCells(p => NextCellState(p));

    return !IsOver();
  }

  private bool IsOver()
  {
    for (int x = 0; x < Cols; x++)
      for (int y = 0; y < Rows; y++)
        if (Cell(x, y) == Core.Cell.Alive)
          return false;

    return true;
  }

  private void MutateCells(Func<Position, Cell> func)
  {
    for (int x = 0; x < Cols; x++)
      for (int y = 0; y < Rows; y++)
        _cells[x, y] = func((x, y));
  }

  private Cell NextCellState(Position p)
  {
    int aliveNeighbourCount = GetAliveNeighbourCount(p);
    return Cell(p).Update(aliveNeighbourCount);
  }

  private int GetAliveNeighbourCount(Position p) => CountNeighbourWithState(p, Core.Cell.Alive);
  private int GetDeadNeighbourCount(Position p) => CountNeighbourWithState(p, Core.Cell.Dead);
  private int CountNeighbourWithState(Position p, Cell cell) => GetNeighbours(p).Count(c => c == cell);

  private IEnumerable<Cell> GetNeighbours(Position p)
  {
    return _neighbourCoords
      .Select(f => new Position(p.X + f.X, p.Y + f.Y))
      .Where(p => CheckBounds(p))
      .Select(f => _cells[f.X, f.Y]);
  }

  private bool CheckBounds(Position p) => p.X >= 0 && p.X < Cols && p.Y >= 0 && p.Y < Rows;
}
