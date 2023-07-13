using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using HabitTracker.App.ViewModels;
using HabitTracker.Data;
using System.Linq;

namespace HabitTracker.App.Views;

public partial class HabitCheckListView : UserControl
{
    public HabitCheckListView()
    {
        InitializeComponent();
    }

	private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        var viewModel = (HabitCheckListViewModel)this.DataContext!;
        var checkBox = (CheckBox)sender;
        var content = checkBox.Content?.ToString();

        DateTimeOffset date = DateTimeOffset.MinValue;

        foreach (var habitCheck in viewModel.Days)
        {
            if (habitCheck.CurrentDate.ToString("dd/MM/yyyy").Substring(0, 10) == content.Substring(0, 10))
            {
                date = habitCheck.CurrentDate;
                break;
            }
        }
        using (var db = new HabitDbContext()) 
        {
            var habitCheck = db.HabitChecks?.FirstOrDefault(b => b.CurrentDate == date);
			if (habitCheck != null)
            {
                habitCheck.CurrentDate = date;
                if (checkBox.IsChecked != null)
                {
                    habitCheck.IsChecked = (bool)checkBox.IsChecked;
                }
            }
			db.SaveChanges();

            var fin = db.Habits?.FirstOrDefault();
            if (fin != null && checkBox.IsChecked == true && date == viewModel.EndDate)
            {
                fin.IsFinished = true;
                db.SaveChanges();
            }
        }

    }
}