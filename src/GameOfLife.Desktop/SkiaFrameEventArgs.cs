using SkiaSharp;

namespace GameOfLife.Desktop;

public readonly struct SkiaFrameEventArgs
{
  public readonly SKCanvas Canvas;
  public readonly SKSize Size;
  public readonly double Time;

  public SkiaFrameEventArgs(SKCanvas canvas, SKSize size, double time)
  {
    Canvas = canvas;
    Size = size;
    Time = time;
  }
}
