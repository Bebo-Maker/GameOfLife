namespace GameOfLife.Core
{
  public interface IDisplay
  {
    Game Game { get; }
    void Render();
  }
}
