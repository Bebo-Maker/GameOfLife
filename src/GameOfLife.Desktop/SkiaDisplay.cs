using GameOfLife.Core;
using SkiaSharp;

namespace GameOfLife.Desktop;

internal class SkiaDisplay : IDisplay
{
  public SKCanvas? Canvas { get; set; }
  public SKSize Size { get; set; }

  public float CellBounds { get; set; } = 25f;
  public float Spacing { get; set; } = 0f;
  public SKPaint AliveCellPaint { get; set; } = new SKPaint
  {
    Color = SKColors.White
  };

  public void Render(Game game)
  {
    if (Canvas == null)
      return;

    Canvas.Clear(SKColors.Black);

    float baseOffsetX = (Size.Width - game.Cols * (CellBounds + Spacing) - Spacing) / 2;
    float baseOffsetY = (Size.Height - game.Rows * (CellBounds + Spacing) - Spacing) / 2;

    for (int x = 0; x < game.Cols; x++)
      for (int y = 0; y < game.Rows; y++)
      {
        if (game.Cell(x, y) == Cell.Dead)
          continue;

        var (offsetX, offsetY) = (x * (CellBounds + Spacing), y * (CellBounds + Spacing));
        Canvas.DrawRect(baseOffsetX + offsetX, baseOffsetY + offsetY, CellBounds, CellBounds, AliveCellPaint);
      }
  }
}
