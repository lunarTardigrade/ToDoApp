using System.Reflection.Metadata;
using Android.Speech.Tts;
using Microsoft.Extensions.Logging;

namespace ToDoApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		//ToDoListApp.AddUser("Tony Lopercio", 44);
		ToDoListApp.AddTask("Feed the dog","fill Pipin's food and water bowls");
		return builder.Build();
	}
}
public class ToDoListApp
{
	public static void AddUser(string fullName, int age)
	{
		var user = new User(fullName, age);
		var userUpdater = DataManager<User>.Create();
		userUpdater.Insert_item(user);
	}

	public static void DeleteUser(string identifier)
	{
		var userUpdater = DataManager<User>.Create();
		userUpdater.Delete_item(identifier);
	}

	public static void AddTask(
		string name, string? description = null, // requiered 
		DateTime? dueDate = null, string? priority = null, string? assignedTo = null, string? notes = null, int points = 1)
	{
		description ??= name;
		var task = new TodoItem(name, description, dueDate, priority, assignedTo, notes, points);
		var taskUpdater = DataManager<TodoItem>.Create();
		taskUpdater.Insert_item(task);
	}
			
	public static void DeleteTask(string identifier)
	{
		var taskUpdater = DataManager<TodoItem>.Create();
		taskUpdater.Delete_item(identifier);
	}

	public static void CompleteTask(string identifier)
	{
		var taskUpdater = DataManager<TodoItem>.Create();
		var taskToComplete = taskUpdater.Get_table().FirstOrDefault(todoItem => todoItem.Identifier == identifier);

		if (taskToComplete != null)
		{
			taskToComplete.MarkComplete();
			taskUpdater.Update_item(taskToComplete);
		}
	}

	public static void AssignTask(string taskID, string userID)
	{
		var taskUpdater = DataManager<TodoItem>.Create();
		var taskToAssign = taskUpdater.Get_table().FirstOrDefault(todoItem => todoItem.Identifier == taskID);
		
		if (taskToAssign != null)
		{
			taskToAssign.AssignTask(userID);
			taskUpdater.Update_item(taskToAssign);
		}
	}

	public static void AddTaskNote(string taskId, string note)
	{
		var taskUpdater = DataManager<TodoItem>.Create();
		var taskToNote = taskUpdater.Get_table().FirstOrDefault(todoItem => todoItem.Identifier == taskId);

		if (taskToNote != null)
		{
			taskToNote.AddNotes(note);
			taskUpdater.Update_item(taskToNote);
		}
	}

	public static void AddTaskTag(string taskId, string tag)
	{
		var taskUpdater = DataManager<TodoItem>.Create();
		var taskToTag = taskUpdater.Get_table().FirstOrDefault(todoItem => todoItem.Identifier == taskId);

		if (taskToTag != null)
		{
			taskToTag.AddTag(tag);
			taskUpdater.Update_item(taskToTag);
		}
	}

	public static void SetTaskPriority(string taskId, string priority)
	{
		var taskUpdater = DataManager<TodoItem>.Create();
		var taskToPrioritize = taskUpdater.Get_table().FirstOrDefault(todoItem => todoItem.Identifier == taskId);

		if (taskToPrioritize != null)
		{
			taskToPrioritize.ChangePriority(priority);
			taskUpdater.Update_item(taskToPrioritize);
		}
	}

	public static void SetTaskDueDate(string taskId, DateTime dueDate)
	{
		var taskUpdater = DataManager<TodoItem>.Create();
		var taskToSetDueDate = taskUpdater.Get_table().FirstOrDefault(todoItem => todoItem.Identifier == taskId);

		if (taskToSetDueDate != null)
		{
			taskToSetDueDate.SetDueDate(dueDate);
			taskUpdater.Update_item(taskToSetDueDate);
		}
	}

	public static void AddSubtask(string taskId, string name, string? description = null, // requiered 
		DateTime? dueDate = null, string? priority = null, string? assignedTo = null, string? notes = null, int points = 1)
	{
		description ??= name;
		var taskUpdater = DataManager<TodoItem>.Create();
		var parentTask = taskUpdater.Get_table().FirstOrDefault(todoItem => todoItem.Identifier == taskId);

		if (parentTask != null)
		{
			parentTask.AddSubtask(name, description,dueDate,priority,assignedTo,notes);
			taskUpdater.Update_item(parentTask);
		}
	}
}


//todo : error handling for cases existing task not found
//todo : add some reasonable comments
//todo : 