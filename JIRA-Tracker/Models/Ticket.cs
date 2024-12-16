using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIRA_Tracker.Models
{
    public class Ticket
    {
        public int Id { get; set; } // Auto-incremented by LiteDB
        public string Title { get; set; }
        public string Submitter { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // e.g., "Open", "In Progress", "Closed"
        public string Priority { get; set; } // e.g., "Low", "Medium", "High"
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public int? ParentId { get; set; } // For parent-child relationships
        public List<int> LinkedTicketIds { get; set; } = new List<int>(); // For linked tickets
    }
}
