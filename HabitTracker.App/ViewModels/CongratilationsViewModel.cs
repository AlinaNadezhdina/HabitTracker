using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using HabitTracker.Data;
using HabitTracker.Data.Models;

namespace HabitTracker.App.ViewModels;

public class CongratulationsViewModel : ViewModelBase
{
    private string _totalDays;

    public string TotalDays
    {
        get => _totalDays;
        set =>  this.RaiseAndSetIfChanged(ref _totalDays, value);
    }

	private string _title;

    public string Title
    {
        get => _title;
        set =>  this.RaiseAndSetIfChanged(ref _title, value);
    }

    public CongratulationsViewModel(IEnumerable<HabitCheck> items, string motivation, string title)
    {
        var itemsArray = items.ToArray();
		_title = title;
		TotalDays = $"Congratulations!\n{itemsArray.Count(x => x.IsChecked)}/{itemsArray.Count()} days checked\nFinally: {motivation}";

		using (var context = new HabitDbContext())    
		{
			context.Database.EnsureDeleted();    
		}
    }
}
