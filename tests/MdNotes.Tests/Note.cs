using System;

namespace MdNotes.Tests
{
    public class Note
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public long Owner { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Note(string title, string content)
        {
            Id = -1;
            Title = title;
            Content = content;
        }
    }
}