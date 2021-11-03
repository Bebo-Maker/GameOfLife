using GameOfLife.Core;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;

namespace GameOfLife.Desktop;

public class SkiaGameOfLife : Disposable
{
  private readonly Game _game;
  private readonly SkiaDisplay _display;
  private readonly SkiaWindow _window;

  private readonly GameCycleTimer _gameCycle;

  private bool _leftMouseDown;
  private bool _rightMouseDown;

  private readonly SKPaint _toolbarPaint = new()
  {
    TextSize = 24,
    Color = SKColors.White,
  };

  public SkiaGameOfLife()
  {
    _game = new Game(CellGenerator.RandomCells(90, 50));
    _display = new SkiaDisplay(_game)
    {
      Spacing = 5,
    };

    _window = new SkiaWindow(false);

    _gameCycle = new GameCycleTimer(_game, TimeSpan.FromMilliseconds(100));
  }

  public void Run()
  {
    _gameCycle.Start();

    _window.PaintSurface += Window_PaintSurface;
    _window.MouseDown += Window_MouseDown;
    _window.MouseMove += Window_MouseMove;
    _window.MouseUp += Window_MouseUp;
    _window.KeyDown += Window_KeyDown;

    _window.Run();
  }

  private void SetCellStateAtPos((float X, float Y) args)
  {
    if (_leftMouseDown)
      UpdateCell(Cell.Alive, (args.X, args.Y));
    else if (_rightMouseDown)
      UpdateCell(Cell.Dead, (args.X, args.Y));

    _display.UpdateHoveredCell((args.X, args.Y));
  }

  private void UpdateCell(Cell cellState, (float X, float Y) p)
  {
    if (_display.HitTest(p.X, p.Y) is Position pos)
    {
      if (!_gameCycle.IsPaused)
        _gameCycle.Stop();

      _game.SetCell(pos, cellState);
    }
  }

  private void Window_MouseDown(MouseButtonEventArgs args)
  {
    if (args.Button == MouseButton.Left)
      _leftMouseDown = true;
    else if (args.Button == MouseButton.Right)
      _rightMouseDown = true;

    if (_leftMouseDown || _rightMouseDown)
      SetCellStateAtPos((_window.MousePosition.X, _window.MousePosition.Y));
  }

  private void Window_MouseMove(MouseMoveEventArgs args)
  {
    SetCellStateAtPos((args.X, args.Y));
  }

  private void Window_MouseUp(MouseButtonEventArgs args)
  {
    if (args.Button == MouseButton.Left)
      _leftMouseDown = false;
    else if (args.Button == MouseButton.Right)
      _rightMouseDown = false;

    _display.ResetHoveredCell();
  }

  private void Window_KeyDown(KeyboardKeyEventArgs args)
  {
    ApplyIf(Keys.Space, _gameCycle.Toggle);
    CheckAndMutate(Keys.A, _ => Cell.Alive);
    CheckAndMutate(Keys.D, _ => Cell.Dead);
    CheckAndMutate(Keys.R, _ => CellGenerator.RandomCell());

    void ApplyIf(Keys k, Action act)
    {
      if (args.Key == k)
        act();
    }

    void CheckAndMutate(Keys r, Func<Position, Cell> func)
    {
      ApplyIf(r, () =>
      {
        _gameCycle.Stop();
        _game.Mutate(func);
      });
    }
  }

  private void Window_PaintSurface(object? sender, SkiaFrameEventArgs e)
  {
    _display.Size = e.Size;
    _display.Canvas = e.Canvas;
    _display.Render();

    if (_gameCycle.IsPaused)
    {
      _display.Canvas.DrawRect(e.Size.Width - 20, 20, 10, 30, _toolbarPaint);
      _display.Canvas.DrawRect(e.Size.Width - 35, 20, 10, 30, _toolbarPaint);
    }
  }


  protected override void DisposeUnmanagedResources()
  {
    _window.Dispose();
    _gameCycle.Dispose();
  }

  protected override void DisposeManagedResources()
  {
    _window.PaintSurface -= Window_PaintSurface;
    _window.MouseDown -= Window_MouseDown;
    _window.MouseMove -= Window_MouseMove;
    _window.MouseUp -= Window_MouseUp;
    _window.KeyDown -= Window_KeyDown;
  }
}
