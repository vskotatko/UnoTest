using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UnoTest.Shared.Controls
{
  public sealed partial class Titlebar : UserControl
  {
    public event RoutedEventHandler RoutedBackClick;
    public event RoutedEventHandler RoutedPageClick;
    public event RoutedEventHandler RoutedItem1Click;
    public event RoutedEventHandler RoutedItem2Click;
    public event RoutedEventHandler RoutedItem3Click;

    public string FolderLabel {
      get { return Folder.Label; }
      set { Folder.Label = value; }
    }

    //-----------------------------------------------------------------------------
    public Titlebar()
    {
      this.InitializeComponent();
      // Folder.Label = "undefined";
    }

    //-----------------------------------------------------------------------------
    void OnBackClicked(object sender, RoutedEventArgs e)
    {
      RoutedBackClick?.Invoke(sender, e);
    }

    //-----------------------------------------------------------------------------
    void OnPageClicked(object sender, RoutedEventArgs e)
    {
      RoutedPageClick?.Invoke (sender, e);
    }

    //-----------------------------------------------------------------------------
    void OnItem1Clicked(object sender, RoutedEventArgs e)
    {
      RoutedItem1Click?.Invoke(sender, e);
    }

    //-----------------------------------------------------------------------------
    void OnItem2Clicked(object sender, RoutedEventArgs e)
    {
      RoutedItem2Click?.Invoke(sender, e);
    }

    //-----------------------------------------------------------------------------
    void OnItem3Clicked(object sender, RoutedEventArgs e)
    {
      RoutedItem3Click?.Invoke(sender, e);
    }
  }
}
