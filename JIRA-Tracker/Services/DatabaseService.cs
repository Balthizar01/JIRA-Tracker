using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JIRA_Tracker.Models;

namespace JIRA_Tracker.Services
{
    public class DatabaseService
    {
        private const string DatabasePath = "tickets.db";


        public void AddTicket(Ticket ticket)
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var tickets = db.GetCollection<Ticket>("tickets");
                tickets.Insert(ticket);
            }
        }

        public List<Ticket> GetTickets()
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var tickets = db.GetCollection<Ticket>("tickets");
                return tickets.FindAll().ToList();
            }
        }

        public void UpdateTicket(Ticket ticket)
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var tickets = db.GetCollection<Ticket>("tickets");
                tickets.Update(ticket);
            }
        }

        public void DeleteTicket(int id)
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var tickets = db.GetCollection<Ticket>("tickets");
                tickets.Delete(id);
            }
        }
    }
}
