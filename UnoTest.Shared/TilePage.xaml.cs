using System.Collections.ObjectModel;
using UnoTest.Nodes;
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
    public ObservableCollection<NodeData> DetailListItems = new ObservableCollection<NodeData>();
    public TilePage()
    {
        this.InitializeComponent();
    }

    //-----------------------------------------------------------------------------
    void OnBackClicked(object sender, RoutedEventArgs e)
    {
    }

    //-----------------------------------------------------------------------------
    void OnPageClicked(object sender, RoutedEventArgs e)
    {
    }

    //-----------------------------------------------------------------------------
    void OnItem1Clicked(object sender, RoutedEventArgs e)
    {

    }

    //-----------------------------------------------------------------------------
    void OnItem2Clicked(object sender, RoutedEventArgs e)
    {

    }

    //-----------------------------------------------------------------------------
    void OnItem3Clicked(object sender, RoutedEventArgs e)
    {

    }
  }
}
