using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SkiaSharp;

namespace GameOfLife.Desktop;

public class SkiaWindow : GameWindow
{
  private const SKColorType ColorType = SKColorType.Rgba8888;
  private const GRSurfaceOrigin SurfaceOrigin = GRSurfaceOrigin.BottomLeft;

  private GRContext? _grContext;
  private GRGlFramebufferInfo _glInfo;
  private GRBackendRenderTarget? _renderTarget;

  private SKSurface? _surface;
  private SKCanvas? _canvas;

  public event EventHandler<SkiaFrameEventArgs>? PaintSurface;

  public int Width => Size.X;
  public int Height => Size.Y;

  public SkiaWindow(bool isEventDriven) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
  {
    IsEventDriven = isEventDriven;
  }

  public SkiaWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
  {
    IsEventDriven = true;
  }

  protected override void OnRenderFrame(FrameEventArgs args)
  {
    // create the contexts if not done already
    if (_grContext == null)
    {
      MakeCurrent();
      var glInterface = GRGlInterface.Create();
      _grContext = GRContext.CreateGl(glInterface);
    }

    // get the new surface size
    var newSize = new SKSizeI(Width, Height);

    // manage the drawing surface
    if (_renderTarget == null || !_renderTarget.IsValid)
      CreateNewRenderTarget(newSize);

    if (_surface == null)
      CreateSurface();

    using (new SKAutoCanvasRestore(_canvas, true))
      PaintSurface?.Invoke(this, new SkiaFrameEventArgs(_canvas!, newSize, args.Time));

    // update the control
    _canvas?.Flush();
    Context.SwapBuffers();
  }

  protected override void OnResize(ResizeEventArgs e)
  {
    base.OnResize(e);
    _renderTarget = null;
  }

  private void CreateNewRenderTarget(SKSizeI newSize)
  {
    GL.GetInteger(GetPName.FramebufferBinding, out var framebuffer);
    //GL.GetInteger(GetPName.StencilBits, out var stencil);
    GL.GetInteger(GetPName.StencilTest, out var stencil);
    GL.GetInteger(GetPName.Samples, out var samples);

    var maxSamples = _grContext?.GetMaxSurfaceSampleCount(ColorType) ?? 1;
    if (samples > maxSamples)
      samples = maxSamples;

    _glInfo = new GRGlFramebufferInfo((uint)framebuffer, ColorType.ToGlSizedFormat());

    // destroy the old surface
    _surface?.Dispose();
    _surface = null;
    _canvas = null;

    // re-create the render target
    _renderTarget?.Dispose();
    _renderTarget = new GRBackendRenderTarget(newSize.Width, newSize.Height, samples, stencil, _glInfo);
  }

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);

    if (disposing)
    {
      _renderTarget?.Dispose();
      _surface?.Dispose();
      _grContext?.Dispose();
    }
  }

  private void CreateSurface()
  {
    _surface = SKSurface.Create(_grContext, _renderTarget, SurfaceOrigin, ColorType);
    _canvas = _surface.Canvas;
  }
}