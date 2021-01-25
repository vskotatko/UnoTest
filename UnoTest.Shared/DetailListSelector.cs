using System;
using UnoTest.Nodes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnoTest
{
  class DetailListSelector : DataTemplateSelector
  {
    public DataTemplate NoteItemTemplate { get; set; }
    public DataTemplate ImageItemTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore (object item)
    {
      if (item is NoteData)
        return NoteItemTemplate;
      else // item is ImageData
        return ImageItemTemplate;
    }
  }

  /*
  class ImageResourceExtension : IMarkupExtension
  {
    public string SourceFileName { get; set; }

    public object ProvideValue (IServiceProvider serviceProvider)
    {
      if (SourceFileName == null)
        return null;

      var imageSource = ImageSource.FromResource (SourceFileName);
      return imageSource;
    }
  }
  */
}
