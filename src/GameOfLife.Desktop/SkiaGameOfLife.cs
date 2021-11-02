using GameOfLife.Core;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameOfLife.Desktop;

public class SkiaGameOfLife
{
  private readonly SkiaDisplay _display;
  private readonly Game _game;

  private bool _isPaused = false;

  public SkiaGameOfLife()
  {
    _display = new SkiaDisplay();
    _game = new Game(Generator.RandomCells(90, 50));
  }

  public void Run()
  {
    SetupCycleTimer();

    using var window = new SkiaWindow(false);
    
    window.PaintSurface += Window_PaintSurface;
    window.KeyDown += Window_KeyDown;
    window.Run();
  }

  private void SetupCycleTimer()
  {
    var timer = new System.Timers.Timer(20);
    timer.Elapsed += Timer_Elapsed;
    timer.Start();
  }

  private void Window_KeyDown(KeyboardKeyEventArgs args)
  {
    if (args.Key == Keys.Space)
      _isPaused = !_isPaused;
  }

  private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
  {
    if(!_isPaused)
      _game.Cycle();
  }

  private void Window_PaintSurface(object? sender, SkiaWindow.RenderInfo e)
  {
    _display.Size = e.Size;
    _display.Canvas = e.Canvas;
    _display.Render(_game);
  }
}
