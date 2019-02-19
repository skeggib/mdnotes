using System;

namespace MdNotes.Tests
{
    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User(string name)
        {
            Id = -1;
            Name = name;
        }
    }
}
