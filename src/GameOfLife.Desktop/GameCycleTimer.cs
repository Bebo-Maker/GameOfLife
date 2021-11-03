using GameOfLife.Core;

namespace GameOfLife.Desktop;

public sealed class GameCycleTimer : Disposable
{
  private readonly Game _game;
  private readonly System.Timers.Timer _timer;

  public bool IsPaused { get; private set; }

  public GameCycleTimer(Game game, TimeSpan interval)
  {
    _game = game;

    _timer = new System.Timers.Timer(interval.TotalMilliseconds);
    _timer.Elapsed += Timer_Elapsed;
  }

  public void Start()
  {
    IsPaused = false;
    _timer.Start();
  }

  public void Stop()
  {
    IsPaused = true;
    _timer.Stop();
  }

  public void Toggle()
  {
    if (IsPaused)
      Start();
    else
      Stop();
  }

  protected override void DisposeUnmanagedResources() => _timer.Dispose();
  protected override void DisposeManagedResources()
  {
    _timer.Stop();
    _timer.Elapsed -= Timer_Elapsed;
  }

  private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e) => _game.Cycle();
}
