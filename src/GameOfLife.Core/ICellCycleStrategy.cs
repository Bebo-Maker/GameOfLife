namespace GameOfLife.Core;

public interface ICellCycleStrategy
{
  Cell Update(Cell cell, int aliveNeighbourCount);
}
