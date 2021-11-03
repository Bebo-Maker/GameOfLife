using GameOfLife.Core.Strategies;

namespace GameOfLife.Core;

public class Game
{
  private readonly Position[] _neighbourCoords = new Position[] { (-1, -1), (-1, 0), (-1, 1), (0, 1), (0, -1), (1, 1), (1, 0), (1, -1) };

  private readonly Buffer2D<Cell> _cells;

  public int Rows => _cells.Rows;
  public int Cols => _cells.Cols;

  public ICellCycleStrategy CellCycleStrategy { get; set; } = new TraditionalCellCycleStrategy();

  public Game(int rows, int cols)
  {
    _cells = new Buffer2D<Cell>(cols, rows);
  }

  public Game(Cell[,] cells)
  {
    _cells = new Buffer2D<Cell>(cells);
  }

  public Cell Cell(int x, int y) => Cell((x, y));
  public Cell Cell(Position p) => _cells.GetValue(p);

  public bool Cycle()
  {
    _cells.Mutate(p => NextCellState(p));

    return !IsOver();
  }

  public void InvertCell(Position p) => SetCell(p, Cell(p).Invert());
  public void SetCell(Position p, Cell cell) => _cells.SetValue(p, cell);
  public void Mutate(Cell cellState) => Mutate(_ => cellState);
  public void Mutate(Func<Position, Cell> cellState) => _cells.Mutate(cellState);

  private bool IsOver()
  {
    for (int x = 0; x < Cols; x++)
      for (int y = 0; y < Rows; y++)
        if (Cell(x, y) == Core.Cell.Alive)
          return false;

    return true;
  }

  private Cell NextCellState(Position p)
  {
    int aliveNeighbourCount = GetAliveNeighbourCount(p);

    return CellCycleStrategy?.Update(Cell(p), aliveNeighbourCount) ?? Core.Cell.Dead;
  }

  private int GetAliveNeighbourCount(Position p) => NeighbourCount(p, Core.Cell.Alive);
  private int NeighbourCount(Position p, Cell cell) => GetNeighbours(p).Count(c => c == cell);

  private IEnumerable<Cell> GetNeighbours(Position p)
  {
    return _neighbourCoords
      .Select(f => new Position(p.X + f.X, p.Y + f.Y))
      .Where(CheckBounds)
      .Select(_cells.GetValue);
  }

  private bool CheckBounds(Position p) => p.X >= 0 && p.X < Cols && p.Y >= 0 && p.Y < Rows;
}
