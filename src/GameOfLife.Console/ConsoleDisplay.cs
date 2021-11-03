using GameOfLife.Core;

namespace GameOfLife.Console;

internal sealed class ConsoleDisplay : IDisplay
{
  public Game Game { get; }

  public ConsoleDisplay(Game game)
  {
    Game = game;
  }

  public void Render()
  {
    System.Console.Clear();

    for (int y = 0; y < Game.Rows; y++)
    {
      string line = GetLineString(Game, y);
      System.Console.WriteLine(line);
    }
  }

  private static string GetLineString(Game game, int y) 
    => new(Enumerable.Range(0, game.Cols)
                     .Select(x => game.Cell(x, y))
                     .Select(GetCharForCell)
                     .ToArray());

  private static char GetCharForCell(Cell cell) 
    => cell switch
    {
      Cell.Alive => '#',
      Cell.Dead => ' ',
      _ => ' ',
    };
}
