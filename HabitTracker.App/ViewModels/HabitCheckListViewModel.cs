using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using HabitTracker.Data.Models;

namespace HabitTracker.App.ViewModels;

public class HabitCheckListViewModel: ViewModelBase
{ 
    public Habit Habit { get; set; }
    public DateTimeOffset EndDate {get;}
    public ObservableCollection<HabitCheck> Days { get; }
    
    public HabitCheckListViewModel(IEnumerable<HabitCheck> days)
    {
        Days = new ObservableCollection<HabitCheck>(days);
        EndDate = Days[Days.Count - 1].CurrentDate;
    }
}
