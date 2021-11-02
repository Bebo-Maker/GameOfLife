using GameOfLife.Core;

namespace GameOfLife.Console;

public static class ConsoleGameOfLife
{
  public static async Task Run()
  {
    var game = new Game(Generator.RandomCells(25, 25));
    var display = new ConsoleDisplay();

    var loop = new GameLoop(game, display);

    await loop.Run(TimeSpan.FromMilliseconds(100));

    System.Console.WriteLine("Game over.");
  }
}
