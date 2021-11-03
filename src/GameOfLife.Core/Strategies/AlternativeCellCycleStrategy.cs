namespace GameOfLife.Core.Strategies;

public sealed class AlternativeCellCycleStrategy : ICellCycleStrategy
{
  public Cell Update(Cell cell, int aliveNeighbourCount) => aliveNeighbourCount % 2 == 0 ? Cell.Dead : Cell.Alive;
}
