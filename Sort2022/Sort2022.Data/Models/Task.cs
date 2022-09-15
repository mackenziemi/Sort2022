using System;
using System.Collections.Generic;

namespace Sort2022.Data.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
