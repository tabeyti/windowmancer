﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Windowmancer.Core.Extensions;
using Windowmancer.Core.Models;
using Windowmancer.Core.Services;

namespace Windowmancer.UI
{
  /// <summary>
  /// Interaction logic for WindowConfigEditor.xaml
  /// </summary>
  public partial class WindowConfigEditor
  {
    public Action OnClose;

    public Action<WindowConfig> OnSave;

    // Binding objects.
    public bool DisplayHelperPreview { get; set; }
    public bool WindowStylingPreview { get; set; }
    public WindowConfig WindowConfig { get; set; }
    public MonitorLayoutInfo OriginalLayoutInfo { get; set; }
    public uint OriginalOpacity { get; set; }
    public ProfileManager ProfileManager { get; set; }

    private Process _process;

#region Constructors
    
    public WindowConfigEditor(
      WindowConfig windowConfig,
      Action<WindowConfig> onSave)
    {
      this.OnSave = onSave;
      this.WindowConfig = (WindowConfig)windowConfig?.Clone();
      PreInitialization();
      InitializeComponent();
      Initialize();
    }
    
#endregion

    /// <summary>
    /// Initializes data bound objects and other dependencies of xaml prior
    /// to component initialization.
    /// </summary>
    private void PreInitialization()
    {
      this.ProfileManager = App.ServiceResolver.Resolve<ProfileManager>();
      this.WindowConfig = this.WindowConfig ?? new WindowConfig();
      _process = MonitorWindowManager.GetProcess(this.WindowConfig);
    }

    private void Initialize()
    {
      // Set up layout manager.
      if (this.WindowConfig.LayoutType == Core.Models.WindowConfigLayoutType.Monitor)
      {
        var layoutEditor = new MonitorLayoutEditor(this.WindowConfig.MonitorLayoutInfo);
        layoutEditor.OnSave += c =>
        {
          this.WindowConfig.MonitorLayoutInfo = c;
        };
        layoutEditor.OnClose += () => { this.FlipView.SelectedIndex = 0; };
        this.FlipView.Items.Add(layoutEditor);
      }
    }

    /// <summary>
    /// Saves all input data into the WindowConfig instance.
    /// </summary>
    private void SaveConfig()
    {
      var matchType = this.MatchByProcesNameRadioButton.IsChecked.Value ?
        WindowMatchCriteriaType.ProcessName : WindowMatchCriteriaType.WindowTitle;

      this.WindowConfig.Name = this.NameTextBox.Text;
      this.WindowConfig.MatchCriteria.MatchType = matchType;
      this.WindowConfig.MatchCriteria.MatchString = this.RegexMatchStringTextBox.Text;
    }

    private void Close()
    {
      // Uncheck previews, restoring old window values.
      this.WindowStylingPreviewCheckBox.IsChecked = false;

      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("WindowConfigEditor - Could locate active window to unbind the KeyDown listener.");
      }
      window.KeyDown -= WindowConfigEditor_HandleKeyPress;
      OnClose?.Invoke();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      SaveConfig();
      OnSave?.Invoke(this.WindowConfig);
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void WindowConfigEditor_OnLoaded(object sender, RoutedEventArgs e)
    {
      var window = Window.GetWindow(this);
      if (window == null)
      {
        throw new Exception("WindowConfigEditor - Could locate active window to bind the KeyDown listener.");
      }
      window.KeyDown += WindowConfigEditor_HandleKeyPress;
    }

    private void WindowConfigEditor_HandleKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key != Key.Escape)
      {
        return;
      }
      
      // If we are in the layout editor, then transition back to 
      // the general settings.
      if (this.FlipView.SelectedIndex != 0)
      {
        this.FlipView.SelectedIndex = 0;
        return;
      }

      Close();
    }
    
    private void WindowOpacitySlider_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (e.Delta > 0)
      {
        WindowOpacitySlider.Value = WindowOpacitySlider.Value + 1;
      }
      else
      {
        WindowOpacitySlider.Value = WindowOpacitySlider.Value - 1;
      }
    }

    private void WindowOpacityValueTextBox_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (this.WindowStylingPreview && null != _process)
      {
        MonitorWindowManager.SetWindowOpacityPercentage(_process, (uint)WindowOpacitySlider.Value);
      }
    }

    private void WindowStylingPreviewCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
      if (this.WindowStylingPreview)
      {
        (_process != null).RunIfTrue(() => this.OriginalOpacity = MonitorWindowManager.GetWindowOpacityPercentage(_process));
        MonitorWindowManager.SetWindowOpacityPercentage(_process, (uint)WindowOpacitySlider.Value);
      }
      else
      {
        (_process != null).RunIfTrue(() => MonitorWindowManager.SetWindowOpacityPercentage(_process, this.OriginalOpacity));
      }
    }
    private void LabelTextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
      (sender as LabelTextBox)?.BaseTextBox.SelectAll();
    }

    private void LayoutEditorButton_OnClick(object sender, RoutedEventArgs e)
    {
    }
  }

  public enum WindowConfigLayoutType
  {
    MonitorLayout,
    HostContainerLayout
  }
}

