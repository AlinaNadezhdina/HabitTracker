using Microsoft.EntityFrameworkCore;
using HabitTracker.Data.Models;

namespace HabitTracker.Data;

public class HabitDbContext : DbContext
{
	public DbSet<Habit>? Habits {get; set;}
    public DbSet<HabitCheck>? HabitChecks {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
		optionsBuilder.UseSqlite("Filename=habits.db");
    }
}
