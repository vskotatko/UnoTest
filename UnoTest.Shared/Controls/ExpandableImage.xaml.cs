using SkiaSharp;
using SkiaSharp.Views.UWP;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using UnoTest.Nodes;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UnoTest.Shared.Controls
{
  public sealed partial class ExpandableImage : UserControl
  {
    private const int padding = 20; // left and right padding for text;
    private const int textSizeAdjustment = 2;

    public ObservableCollection<NodeData> DetailListItems = new ObservableCollection<NodeData>();

    // Source
    public static readonly DependencyProperty SourceProperty =
       DependencyProperty.Register(
          "Source",
          typeof(string),
          typeof(ExpandableImage),
          new PropertyMetadata(default(object)));

    public string Source
    {
      get { return (string)GetValue(SourceProperty); }
      set 
      { 
        SetValue(SourceProperty, value);
        LoadBmpSrc();
        SetElementWidth ();
        EICanvas.Invalidate();
      }
    }

    // Label
    public static readonly DependencyProperty LabelProperty =
       DependencyProperty.Register(
          "Label",
          typeof(string),
          typeof(ExpandableImage),
          new PropertyMetadata(default(object)));

    public string Label
    {
      get { return (string)GetValue(LabelProperty); }
      set 
      { 
        SetValue(LabelProperty, value);
        SetElementWidth ();
        EICanvas.Invalidate();
      }
    }

    // Other vars
    private float textCoreHeight = 0; // in actual device pixels
    private float desiredElementWidth = 0; // in actual device pixels
    private SKBitmap bmpSrc = null; // use this instead of Source

    //-----------------------------------------------------------------------------
    public ExpandableImage()
    {
      this.InitializeComponent();
    }

    //-----------------------------------------------------------------------------
    private void OnSizeChanged (Object sender, SizeChangedEventArgs e)
    {
      var newH = e.NewSize.Height;
      var oldH = this.ActualHeight; // in pixels
      if (newH != oldH)
      {
        Console.WriteLine ("ActualHeight: " + this.ActualHeight);
        SetElementWidth ();
      }
    }

    //-----------------------------------------------------------------------------
    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
      if (bmpSrc == null || Label == null)
        return;

      var canvas = e.Surface.Canvas;

      // Resize bitmap to match height of "this"
      Log.L("SI: canvas wh " + canvas.DeviceClipBounds.Width + ", " + canvas.DeviceClipBounds.Height);
      if (bmpSrc.Height != canvas.DeviceClipBounds.Height) // resize
      {
        float hDest = canvas.DeviceClipBounds.Height; // actual pixels
        float scale = hDest / (float)bmpSrc.Height;
        float wDest = bmpSrc.Width * scale;
        bmpSrc = bmpSrc.Resize(new SKSizeI((int)wDest, (int)hDest), SKFilterQuality.High);
      }

      // identify left, right halves and a 10px wide swath of the middle of the source bitmap
      SKRect rectSrcLeft = new SKRect (0, 0, bmpSrc.Width / 2, bmpSrc.Height);
      SKRect rectSrcRight = new SKRect (bmpSrc.Width / 2, 0, bmpSrc.Width, bmpSrc.Height);
      SKRect rectSrcMid = new SKRect (bmpSrc.Width / 2 - 5, 0, bmpSrc.Width / 2 + 5, bmpSrc.Height);

      // create a new bitmap containing a 10 pixel wide swatch from middle of bmpSrc
      SKBitmap bmpSrcMid = new SKBitmap (10, bmpSrc.Height);
      using (SKCanvas tempCanvas = new SKCanvas (bmpSrcMid))
      {
        SKRect rectDest = new SKRect (0, 0, rectSrcMid.Width, rectSrcRight.Height);
        tempCanvas.DrawBitmap (bmpSrc, rectSrcMid, rectDest);
      }

      using (SKPaint paint = new SKPaint ())
      {
//        paint.Color = new SKColor (0, 255, 255);
//        canvas.DrawRect (0, 0, canvas.DeviceClipBounds.Width, canvas.DeviceClipBounds.Height, paint);

        paint.IsAntialias = false;

        // determine dest rect for middle section
        float rightDest = canvas.DeviceClipBounds.Width;
        SKRect rectDestMid = new SKRect(rectSrcLeft.Width, 0, rightDest - rectSrcRight.Width, rectSrcRight.Height);

        // left part of tab
        canvas.DrawBitmap (bmpSrc, rectSrcLeft, rectSrcLeft, paint);
        Log.L ("SI: hSrc " + bmpSrc.Height);

        // right part of tab
        {
          SKRect rectDest = new SKRect (rectDestMid.Right, 0, rightDest, rectSrcRight.Height);
          canvas.DrawBitmap (bmpSrc, rectSrcRight, rectDest, paint);
        }

        // mid part of tab
        paint.Shader = SKShader.CreateBitmap (bmpSrcMid,
                                              SKShaderTileMode.Repeat,
                                              SKShaderTileMode.Repeat);
        canvas.DrawRect (rectDestMid, paint);
      }

      using (SKPaint paint = new SKPaint { Color = SKColors.Black })
      {
        paint.TextSize = (paint.TextSize + textSizeAdjustment) * getScreenScaleFactor ();
        float leftText = padding * getScreenScaleFactor ();
        float bottomText = canvas.DeviceClipBounds.Height / 2 + textCoreHeight / 2;
        canvas.DrawText(Label, new SKPoint(leftText, bottomText), paint);
      }
    }

    //-----------------------------------------------------------------------------
    private async void LoadBmpSrc()
    {
      if (Source == null)
        return;
#if true
      // The resource names will be prefixed with the "head" path. So, we cannot use a path
      // containing "Shared" to retrieve one. Instead we leave the head off and then search
      // through the resource names looking for everything but the head.
      string sourceWithNameSpace = null;
      Assembly assembly = GetType ().GetTypeInfo ().Assembly;
      string[] names = assembly.GetManifestResourceNames ();
      Log.L ("Resource Names");
      foreach (var name in names)
      {
        Log.L ("  " + name);
        if (name.EndsWith (Source))
        {
          sourceWithNameSpace = name;
          break;
        }
      }
      if (sourceWithNameSpace == null)
        return;
      using (var stream = assembly.GetManifestResourceStream (sourceWithNameSpace))
      {
        bmpSrc = SKBitmap.Decode (stream);
      }
#else
      // Uno does not package resources as "content".
      var uri = new System.Uri (Source);
      StorageFile file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync (uri);
      using (var stream = await file.OpenStreamForReadAsync ())
      {
        bmpSrc = SKBitmap.Decode (stream);
      }
#endif
    }

    //-----------------------------------------------------------------------------
    // Call this whenever either the Source or the Label changes
    private void SetElementWidth()
    {
      if (Label == null || bmpSrc == null)
        return;

      var scaleFactor = getScreenScaleFactor ();
      Log.L ("SEW: scaleFactor " + scaleFactor);

      // determine space needed for text -- units are actual device pixels
      using (SKPaint textPaint = new SKPaint ())
      {
        Log.L ("SEW: textSize " + textPaint.TextSize);
        textPaint.TextSize = (textPaint.TextSize + textSizeAdjustment); // * scaleFactor;
        var textBounds = new SkiaSharp.SKRect ();

        textPaint.MeasureText ("X", ref textBounds);
        textCoreHeight = textBounds.Height;
        Log.L ("SEW: core height " + textCoreHeight);

        textPaint.MeasureText (Label, ref textBounds);
        desiredElementWidth = (textBounds.Width + padding * 2); // * scaleFactor;
        Log.L ("SEW: source width " + textBounds.Width);
        Log.L ("SEW: desired width " + desiredElementWidth);
      }

      // make sure width is >= source bitmap width
      Log.L ("SEW: bmp wh " + bmpSrc.Width + ", " + bmpSrc.Height);
      var h = this.ActualHeight; // in XAML units (docs say pixels)
      Log.L ("SEW: actual h " + h);
      var scale = (float)h / (float)bmpSrc.Height;
      Log.L ("SEW: scale " + scale);
      var scaledBmpWidth = scale * bmpSrc.Width;
      Log.L ("SEW: sw " + scaledBmpWidth);

      if (scaledBmpWidth > desiredElementWidth)
        desiredElementWidth = scaledBmpWidth;
      Log.L ("SEW: w " + desiredElementWidth);

      // set element width
      Width = desiredElementWidth;
    }

    //-----------------------------------------------------------------------------
    private float getScreenScaleFactor ()
    {
      var dispInfo = DisplayInformation.GetForCurrentView ();
//      Log.L ("LogicalDpi: " + dispInfo.LogicalDpi);
//      Log.L ("RawDpiX: " + dispInfo.RawDpiX);
//      Log.L ("RawDpiY: " + dispInfo.RawDpiY);
//      Log.L ("RawPPV: " + dispInfo.RawPixelsPerViewPixel);
//      Log.L ("ResolutionScale: " + dispInfo.ResolutionScale);
//      Log.L ("ScreenHeightRaw: " + dispInfo.ScreenHeightInRawPixels);
//      Log.L ("ScreenWidthRaw: " + dispInfo.ScreenWidthInRawPixels);

      return (float)dispInfo.RawPixelsPerViewPixel;

      /*
      Peri laptop
      LogicalDpi: 96
      RawDpiX: 94
      RawDpiY: 94
      RawPPV: 1
      ResolutionScale: Scale100Percent
      ScreenHeightRaw: 1200
      ScreenWidthRaw: 1920

      LG G2
      LogicalDpi: 288
      RawDpiX: 422.03
      RawDpiY: 424.069
      RawPPV: 3
      ResolutionScale: Scale300Percent
      ScreenHeightRaw: 1920
      ScreenWidthRaw: 1080

      LG G6
      LogicalDpi: 384
      RawDpiX: 562.707
      RawDpiY: 562.707
      RawPPV: 4
      ResolutionScale: Scale400Percent
      ScreenHeightRaw: 2880
      ScreenWidthRaw: 1440

      TCL
      LogicalDpi: 288
      RawDpiX: 347.24
      RawDpiY: 396.24
      RawPPV: 3
      ResolutionScale: Scale300Percent
      ScreenHeightRaw: 2340
      ScreenWidthRaw: 1080
      */
    }
  }
}
