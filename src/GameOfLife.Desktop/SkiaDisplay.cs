using GameOfLife.Core;
using SkiaSharp;

namespace GameOfLife.Desktop;

internal class SkiaDisplay : IDisplay
{
  private SKRect _grid;
  private Position? _hoveredCell;

  public SKCanvas? Canvas { get; set; }
  public SKSize Size { get; set; }

  public float CellBounds { get; set; } = 25f;
  public float Spacing { get; set; } = 0f;
  public SKPaint AliveCellPaint { get; set; } = new()
  {
    Color = SKColors.White
  };

  public SKPaint HoveredCellPaint { get; set; } = new()
  {
    Color = SKColors.Red
  };

  public SKPaint OutlinePaint { get; set; } = new SKPaint
  {
    Style = SKPaintStyle.Stroke,
    StrokeWidth = 25f,
    Color = SKColors.LightGray
  };

  public Game Game { get; }

  public SkiaDisplay(Game game)
  {
    Game = game;
  }

  public Position? HitTest(float x, float y)
  {
    if (!_grid.Contains(x, y))
      return null;

    int cellX = (int)((x - _grid.Left) / (CellBounds + Spacing));
    int cellY = (int)((y - _grid.Top) / (CellBounds + Spacing));

    return (cellX, cellY);
  }

  public void Render()
  {
    if (Canvas == null)
      return;

    Canvas.Clear(SKColors.Black);

    float width = Game.Cols * (CellBounds + Spacing) - Spacing;
    float height = Game.Rows * (CellBounds + Spacing) - Spacing;
    float baseOffsetX = (Size.Width - width) / 2;
    float baseOffsetY = (Size.Height - height) / 2;

    _grid = new SKRect
    {
      Left = baseOffsetX,
      Top = baseOffsetY,
      Right = baseOffsetX + width,
      Bottom = baseOffsetY + height,
    };

    Canvas.DrawRect(baseOffsetX - CellBounds, baseOffsetY - CellBounds, width + CellBounds * 2, height + CellBounds * 2, OutlinePaint);

    for (int x = 0; x < Game.Cols; x++)
      for (int y = 0; y < Game.Rows; y++)
      {
        bool isHovered = IsHoveredCell(x, y);

        if (Game.Cell(x, y) == Cell.Dead && !isHovered)
          continue;

        var paint = isHovered ? HoveredCellPaint : AliveCellPaint;

        var (offsetX, offsetY) = (x * (CellBounds + Spacing), y * (CellBounds + Spacing));
        Canvas.DrawRect(baseOffsetX + offsetX, baseOffsetY + offsetY, CellBounds, CellBounds, paint);
      }
  }

  private bool IsHoveredCell(int x, int y)
  {
    return _hoveredCell.HasValue && _hoveredCell.Value.X == x && _hoveredCell.Value.Y == y;
  }

  internal void UpdateHoveredCell((float X, float Y) p) => _hoveredCell = HitTest(p.X, p.Y);
  internal void ResetHoveredCell() => _hoveredCell = null;
}
