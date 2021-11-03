namespace GameOfLife.Core.Strategies;

public sealed class HighLifeCellCycleStrategy : ICellCycleStrategy
{
  public Cell Update(Cell cell, int aliveNeighbourCount)
    => (cell, aliveNeighbourCount) switch
    {
      //Any live cell with two or three live neighbours survives.
      { cell: Cell.Alive, aliveNeighbourCount: 2 } => Cell.Alive,
      { cell: Cell.Alive, aliveNeighbourCount: 3 } => Cell.Alive,

      //Any dead cell with three live neighbours becomes a live cell.
      { cell: Cell.Dead, aliveNeighbourCount: 3 } => Cell.Alive,

      //Any dead cell with six live neighbours becomes a live cell.
      { cell: Cell.Dead, aliveNeighbourCount: 6 } => Cell.Alive,

      //All other live cells die in the next generation. Similarly, all other dead cells stay dead.
      _ => Cell.Dead,
    };
}
