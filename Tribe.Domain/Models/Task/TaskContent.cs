namespace Tribe.Domain.Models.Task;

public class TaskContent
{
    public IEnumerable<Section> Sections { get; set; }
    
    public class Section
    {
        public string Label { get; set; }
        public Input Input { get; set; }
    }

    public class Input 
    {
        public string Content { get; set; }
    }
}