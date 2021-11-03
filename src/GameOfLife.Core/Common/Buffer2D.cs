namespace GameOfLife.Core;

internal sealed class Buffer2D<T>
{
  private T[,] _buffer;

  public int Rows { get; }
  public int Cols { get; }

  public Buffer2D(int cols, int rows) : this(new T[cols, rows]) { }
  public Buffer2D(T[,] buffer)
  {
    Cols = buffer.GetLength(0);
    Rows = buffer.GetLength(1);

    _buffer = buffer;
  }

  public T GetValue(Position p) => _buffer[p.X, p.Y];
  public void SetValue(Position p, T value) => _buffer[p.X, p.Y] = value;

  public void Mutate(Func<Position, T> func)
  {
    var tempBuffer = new T[Cols, Rows];

    for (int c = 0; c < Cols; c++)
      for (int r = 0; r < Rows; r++)
        tempBuffer[c, r] = func((c, r));

    _buffer = tempBuffer;
  }
}
