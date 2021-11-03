using var game = new GameOfLife.Desktop.SkiaGameOfLife();
game.Run();
//await GameOfLife.Console.ConsoleGameOfLife.RunAsync(25, 25, TimeSpan.FromMilliseconds(100));