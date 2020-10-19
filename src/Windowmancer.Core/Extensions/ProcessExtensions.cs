﻿///
/// https://stackoverflow.com/a/48316821
/// 

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;

namespace Windowmancer.Core.Extensions
{
  public static class ProcessExtensions
  {
    [DllImport("Kernel32.dll")]
    private static extern uint QueryFullProcessImageName([In] IntPtr hProcess, [In] uint dwFlags, [Out] StringBuilder lpExeName, [In, Out] ref uint lpdwSize);

    public static string GetMainModuleFileName(this Process process, int buffer = 1024)
    {
      var fileNameBuilder = new StringBuilder(buffer);
      uint bufferLength = (uint)fileNameBuilder.Capacity + 1;
      return QueryFullProcessImageName(process.Handle, 0, fileNameBuilder, ref bufferLength) != 0 ?
          fileNameBuilder.ToString() :
          null;
    }

    public static Icon GetIcon(this Process process)
    {
      try
      {
        string mainModuleFileName = process.GetMainModuleFileName();
        return Icon.ExtractAssociatedIcon(mainModuleFileName);
      }
      catch
      {
        // Probably no access
        return null;
      }
    }
  }
}
