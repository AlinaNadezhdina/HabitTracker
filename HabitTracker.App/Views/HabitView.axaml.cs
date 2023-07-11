using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HabitTracker.App.Views;

public partial class HabitView : UserControl
{
    public HabitView()
    {
        InitializeComponent();
    }
	private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}