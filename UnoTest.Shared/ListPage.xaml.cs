using UnoTest.Nodes;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnoTest.Shared;
using UnoTest.Shared.Controls;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
namespace UnoTest
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class ListPage : Page
  {
    public ObservableCollection<NodeData> DetailListItems = new ObservableCollection<NodeData>();
    int parentId = -1; // -1 == undefined

    //-----------------------------------------------------------------------------
    private class Item // members must be public for DeserializeObject.
    {
      public int id { get; set; }
      public String description { get; set; }
    }

    //-----------------------------------------------------------------------------
    public ListPage()
    {
      this.InitializeComponent();

//      this.Log("MP LoadList");
      LoadList();
/*
      DetailListItems.Add(new NoteData { Note = "As Donald Trump tried to claim victory before votes were tallied in critical battleground states, the Biden campaign was privately telling supporters not to panic, even as it prepared for pitched legal battles with the president." });
      DetailListItems.Add(new NoteData { Note = "In a Zoom call with donors Wednesday, the aides told the group that Joe Biden was on pace to reach 270 electoral votes in short order, beaming over victories in the Midwestern states that Donald Trump flipped four years ago." });
      //      DetailListItems.Add(new ImageData { FileName = "image_chair_pk.jpg" });
      //      DetailListItems.Add(new ImageData { FileName = "image_chanty.jpg" });
      DetailListItems.Add(new NoteData { Note = "Item 3" });
      DetailListItems.Add(new NoteData { Note = "Item 4" });
      DetailListItems.Add(new NoteData { Note = "Item 5" });
      DetailListItems.Add(new NoteData { Note = "Item 6" });
      DetailListItems.Add(new NoteData { Note = "Item 7" });
      DetailListItems.Add(new NoteData { Note = "Item 8" });
      DetailListItems.Add(new NoteData { Note = "Item 9" });
*/
    }

    //-----------------------------------------------------------------------------
    public async Task LoadList()
    {
      try
      {
//        Debug.WriteLine("LL 1");
        HttpClient httpClient = ((App)App.Current).httpClient;
//        Debug.WriteLine("LL 2");
        HttpResponseMessage response = null;

        // get folder
        Uri uri = new Uri("https://xamarin.perinote.com/root");
        //        Debug.WriteLine("LL 3");
#if false
        response = await httpClient.GetAsync(uri);
//        Debug.WriteLine("LL 4");
        if (!response.IsSuccessStatusCode)
        {
          // exit and send notification to UI thread
          return;
        }
        string results = await response.Content.ReadAsStringAsync();
#endif
        string results = await httpClient.GetStringAsync(uri);
        var folderData = JsonConvert.DeserializeObject<Item[]>(results);
        if (folderData.Length != 1)
        {
          // exit and send notification to UI thread
          return;
        }
        parentId = folderData[0].id;

        // get items
        uri = new Uri("https://xamarin.perinote.com/children?parent=" + parentId);
        response = await httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
          // exit and send notification to UI thread
          return;
        }
        results = await response.Content.ReadAsStringAsync();
        var children = JsonConvert.DeserializeObject<Item[]>(results);

        // display
//        Folder.Label = "something a bit longer";
//        Folder.Label = "tiny";
//        Folder.Label= "The globe";
        Titlebar.FolderLabel = folderData[0].description;

        foreach (var child in children)
          DetailListItems.Add(new NoteData { Note = child.description });
      }
      catch (Exception e)
      {
//        Debug.WriteLine("http exception: ", e.ToString());
      }
    }

    //-----------------------------------------------------------------------------
    void OnBackClicked (object sender, RoutedEventArgs e)
    {
    }

    //-----------------------------------------------------------------------------
    void OnPageClicked (object sender, RoutedEventArgs e)
    {
      Log.L("LP: page clicked");
      this.Frame.Navigate(typeof(TilePage));
    }

    //-----------------------------------------------------------------------------
    void OnItem1Clicked (object sender, RoutedEventArgs e)
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
