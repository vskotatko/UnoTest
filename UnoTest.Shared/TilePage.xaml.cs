using System.Collections.ObjectModel;
using UnoTest.Nodes;
using UnoTest.Shared.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UnoTest.Shared
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class TilePage : Page
  {
    public ObservableCollection<NodeData> DetailGridItems = new ObservableCollection<NodeData>();
    public TilePage()
    {
      this.InitializeComponent();

      DetailGridItems.Add(new ImageData { FileName = "ms-appx:///Assets/pictures/beach.jpg",
                                          Label = "Beach" });
      DetailGridItems.Add(new ImageData { FileName = "ms-appx:///Assets/pictures/covid_wedding.jpg",
                                          Label = "Wedding" });
      DetailGridItems.Add(new ImageData { FileName = "ms-appx:///Assets/pictures/image_chair_pk.jpg",
                                          Label = "Chair Peak" });
      DetailGridItems.Add(new ImageData { FileName = "ms-appx:///Assets/pictures/image_chanty.jpg",
                                          Label = "Chanterelle" });
      DetailGridItems.Add(new ImageData { FileName = "ms-appx:///Assets/pictures/sidekick.png",
                                          Label = "Side Kick" });
      DetailGridItems.Add(new ImageData { FileName = "ms-appx:///Assets/pictures/smoke.jpg",
                                          Label = "Smoky Sky" });
      Titlebar.FolderLabel = "something a bit longer";

    }

    //-----------------------------------------------------------------------------
    private void OnSizeChanged (object sender, SizeChangedEventArgs e)
    {
      GridView gridView = (GridView)sender;
      ItemsWrapGrid panel = ((ItemsWrapGrid)gridView.ItemsPanelRoot);

      const int minColWidth = 250;
      var columns = System.Math.Ceiling(ActualWidth / minColWidth);
      double newWidth = e.NewSize.Width / columns;
      panel.ItemWidth = newWidth;
      panel.ItemHeight = newWidth;
    }

    //-----------------------------------------------------------------------------
    void OnPageClicked(object sender, RoutedEventArgs e)
    {
      Log.L("TP: page clicked");
      this.Frame.Navigate(typeof(ListPage));
    }
  }
}
