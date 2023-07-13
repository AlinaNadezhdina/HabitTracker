using HabitTracker.Data;
using HabitTracker.Data.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;

namespace HabitTracker.App.ViewModels;

public class HabitViewModel : ViewModelBase
{
	private readonly ObservableAsPropertyHelper<bool> _startBtnEnable;
	public bool StartButtonEnable => _startBtnEnable.Value;

	private string _title;
	public string Title
	{
		get => _title;
		set => this.RaiseAndSetIfChanged(ref _title, value);
	}

	private string _motivation;
	public string Motivation
	{
		get => _motivation;
		set => this.RaiseAndSetIfChanged(ref _motivation, value);
	}

	private DateTimeOffset _startDate = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 
		new TimeSpan(1, 0, 0));
	public DateTimeOffset StartDate 
	{
		get => _startDate.DateTime;
		set => this.RaiseAndSetIfChanged(ref _startDate, value.DateTime);
	}
	
    private int _trackingDaysNumber;
    public int TrackingDaysNumber
    {
        get => _trackingDaysNumber;
        set => this.RaiseAndSetIfChanged( ref _trackingDaysNumber, value);  
    }
    
    public HabitViewModel()
    {
	    this.WhenAnyValue(
			    x => x.Title,
			    x => x.Motivation,
			    x => x.TrackingDaysNumber,
			    (title, motiv, days) => !string.IsNullOrWhiteSpace(title) &&
			                            !string.IsNullOrWhiteSpace(motiv) && days > 0)
		    .ToProperty(this, x => x.StartButtonEnable, out _startBtnEnable);
			
    }

	public Habit SaveChanges()
	{ 
		 using (var db = new HabitDbContext()) 
		{
            var countDays = this.TrackingDaysNumber;
        	var habit = new Habit(this.Title, this.Motivation, this.StartDate,this.TrackingDaysNumber);
        	habit.DaysList  = new List<HabitCheck>(countDays);
        
			for (int i = 0; i < countDays; i++)
			{
				habit.DaysList.Add(new HabitCheck(habit.StartDate.AddDays((double)i), false));
			}			
			db.Add(habit);
			db.SaveChanges();
			return habit;
        }
	}
}
