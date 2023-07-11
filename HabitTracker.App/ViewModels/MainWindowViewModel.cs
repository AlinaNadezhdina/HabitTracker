using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using HabitTracker.Data.Models;
using HabitTracker.Data;
using Microsoft.EntityFrameworkCore;


namespace HabitTracker.App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public HabitViewModel HabitViewModel { get; }
    
    public HabitCheckListViewModel HabitCheckListViewModel { get; set; }
    
    public MainWindowViewModel()
    {
        using var context = new HabitDbContext();
        var habit = context.Habits.FirstOrDefault(x => x.IsFinished);
        if (habit != null)
        {
			SetContentToFinishViewModel(context);
        }
        else
        {
            habit = context.Habits
                .Include(h => h.DaysList)
                .FirstOrDefault(x => !x.IsFinished);
            if (habit != null)
            {
                var habitCheckListViewModel = new HabitCheckListViewModel(habit.DaysList);
                habitCheckListViewModel.Habit = habit;
                Content = habitCheckListViewModel;
            }
            else
            {
                Content = HabitViewModel = new HabitViewModel();
            }
        }
    }

    
    private ViewModelBase _content;
    public ViewModelBase Content
    {
        get => _content;
        private set
        {
           var res  =  this.RaiseAndSetIfChanged( ref _content, value);
        } 
    }

    public void StartCommand()
    {
        // using (var db = new HabitDbContext()) 
		// {
        //     var countDays = this.HabitViewModel.TrackingDaysNumber;
        // 	var habit = new Habit(this.HabitViewModel.Title, this.HabitViewModel.Motivation, this.HabitViewModel.StartDate,
        //     this.HabitViewModel.TrackingDaysNumber);
        // 	habit.DaysList  = new List<HabitCheck>(countDays);
        
		// 	for (int i = 0; i < countDays; i++)
		// 	{
		// 		habit.DaysList.Add(new HabitCheck(habit.StartDate.AddDays((double)i), false));
		// 	}
		// 	db.Add(habit);
		// 	db.SaveChanges();
		// }
		var habit = this.HabitViewModel.SaveChanges();

		HabitCheckListViewModel  = new HabitCheckListViewModel(habit.DaysList );
		HabitCheckListViewModel.Habit = habit;
		Content = HabitCheckListViewModel;
        
    }
    public void FinishCommand()
    {
		using (var context = new HabitDbContext())
		{
			SetContentToFinishViewModel(context);
		}
 	}

	private void SetContentToFinishViewModel(HabitDbContext context)
	{ 
		var days = context.HabitChecks.Where(b => b.HabitId == 1).ToList();
		var motivation = context.Habits.FirstOrDefault(b => b.Id == 1).Motivation;
		var title = context.Habits.FirstOrDefault(b => b.Id == 1).Title;
		var congratulationsViewModel = new CongratulationsViewModel(days, motivation, title);
		Content = congratulationsViewModel;
	}
}