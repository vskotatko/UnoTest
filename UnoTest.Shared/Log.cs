using System;
using System.Diagnostics;

namespace UnoTest.Shared
{
  public class Log
  {
    public static void L (string s)
    {
#if __ANDROID__ || __IOS__
      Console.WriteLine (s);
#else
      Debug.WriteLine (s);
#endif
    }
  }
}
