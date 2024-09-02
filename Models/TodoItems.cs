using IntelliJ.Lang.Annotations;
using Newtonsoft.Json;
using SQLite;

namespace ToDoApp
{
    public class TodoItem : IIdentifiable
    {
        [PrimaryKey] public string Identifier { get; private set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get;set; }
        public string Priority { get; set; } 
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        [Ignore] public List<string> Tags { get; set; } = [];
        [Ignore] public List<TodoItem> Subtasks { get; set; } = [];
        public string? AssignedTo { get; set; }
        public string Status { get; set; } 
        public string? Notes { get; set; }
        public int Points { get; set; }
        [Column("Tags")] public string TagsSerialized
        {
            get => JsonConvert.SerializeObject(Tags);
            set => Tags = JsonConvert.DeserializeObject<List<string>>(value) ?? [];
        }
        [Column("Subtasks")] public string SubtasksSerialized
        {
            get => JsonConvert.SerializeObject(Subtasks);
            set => Subtasks = JsonConvert.DeserializeObject<List<TodoItem>>(value) ?? [];
        }

        public TodoItem()
        {
            Identifier = IdHelper.GetNewID();
            Name = "none";
            Description = "none";
            Priority = "Normal";
            CreationDate = DateTime.Now;
            Tags = [];
            Subtasks = [];
            Status = "New";
            Points = 1;
        }

        public TodoItem(
            string name, string description, // requiered 
            DateTime? dueDate = null, string? priority = null, string? assignedTo = null,
            string? notes = null, int points = 1, string? identifier = null)
        {
            Identifier = identifier ?? IdHelper.GetNewID();
            Name = name;
            Description = description;
            DueDate = dueDate;
            Priority = priority ?? "Normal";
            CreationDate = DateTime.Now;
            Tags = [];
            Subtasks = [];
            AssignedTo = assignedTo;
            Status = assignedTo != null ? "Assigned" : "New";
            Notes = notes;
            Points = points;
        }

        public void AssignTask(string assignee)
        {
            AssignedTo = assignee;
            Status = "Assigned";
        }

        public void SetDueDate(DateTime dueDate)
        {
            DueDate = dueDate;
        }

        public void ChangePriority(string priority)
        {
            Priority = priority;
        }

        public void AddTag(string tag)
        {
            if (!Tags.Contains(tag))
            {
                Tags.Add(tag);
            }
        }

        public void AddSubtask(
            string name, string description, // requiered 
            DateTime? dueDate = null, string? priority = null, string? assignedTo = null, string? notes = null)
        {
            var subtask = new TodoItem(name, description, dueDate, priority, assignedTo, notes);
            Subtasks.Add(subtask);
        }

        public void AddNotes(string notes)
        {
            if (string.IsNullOrWhiteSpace(Notes))
            {
                Notes = notes;
            }
            else
            {
                Notes += "\n" + notes;  
            }
        }

        public void AssignPoints(int points)
        {
            Points = points;
        }

        public void MarkComplete()
        {
            Status = "Complete";
            CompletionDate = DateTime.Now;
        }
    }
}