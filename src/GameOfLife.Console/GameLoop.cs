using GameOfLife.Core;

namespace GameOfLife.Console;

internal sealed class GameLoop
{
  private readonly Game _game;
  private readonly IDisplay _display;

  public GameLoop(Game game, IDisplay display)
  {
    _game = game;
    _display = display;
  }

  public async Task Run(TimeSpan delayPerCycle)
  {
    _display.Render();
    while (true)
    {
      bool isOver = !_game.Cycle();
      _display.Render();
      if (isOver)
        break;

      await Task.Delay(delayPerCycle).ConfigureAwait(false);
    }
  }
}
