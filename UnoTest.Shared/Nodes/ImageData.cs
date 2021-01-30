using System;
using System.Collections.Generic;
using System.Text;

namespace UnoTest.Nodes
{
  public class ImageData : NodeData
  {
    public string FileName { get; set; }
    public string Label { get; set; }

    /*
    public ImageSource Path
    {
      get
      {
        ImageSource path = ImageSource.FromResource("UnoTest.Assets." + FileName);
        return path;
      }
    }
    */
  }
}
