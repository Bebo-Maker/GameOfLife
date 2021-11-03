namespace GameOfLife.Desktop;

public class Disposable : IDisposable
{
  protected bool IsDisposed { get; private set; }

  ~Disposable()
  {
    Dispose(false);
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  private void Dispose(bool disposing)
  {
    if (IsDisposed)
      return;

    if (disposing)
      DisposeManagedResources();

    DisposeUnmanagedResources();
  }

  protected virtual void DisposeManagedResources() { }
  protected virtual void DisposeUnmanagedResources() { }
}
