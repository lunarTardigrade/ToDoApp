using System.Collections.ObjectModel;

namespace ToDoApp;

public partial class MainPage : ContentPage
{
	public ObservableCollection<TodoItem> TodoItems { get; set; }
	//int count = 0;
	
	public MainPage()
	{
		InitializeComponent();
		var itemDataManager = DataManager<TodoItem>.Create();
		var allTodoItems = itemDataManager.Get_table(); 
		TodoItems = [.. allTodoItems];
	}

	//  private void OnCounterClicked(object sender, EventArgs e)
	//  {
	//  	count++;

	//  	if (count == 1)
	//  		CounterBtn.Text = $"Clicked {count} time";
	//  	else
	//  		CounterBtn.Text = $"Clicked {count} times";

	//  	SemanticScreenReader.Announce(CounterBtn.Text);
	//  }
}

