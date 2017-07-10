using System.Windows;
using System.Windows.Controls;

namespace Windowmancer.Extensions
{
  // ReSharper disable once InconsistentNaming
  public static class UIElementExtensions
  {
    public const int ZAXIS_BOTTOM = 0;
    public const int ZAXIS_MIDDLE = 1;
    public const int ZAXIS_TOP = 2;

    public static void MoveToBack(this UIElement element)
    {
      Panel.SetZIndex(element, ZAXIS_BOTTOM);
    }

    public static void SetMiddle(this UIElement element)
    {
      Panel.SetZIndex(element, ZAXIS_MIDDLE);
    }

    public static void MoveToFront(this UIElement element)
    {
      Panel.SetZIndex(element, ZAXIS_TOP);
    }
  }
}
