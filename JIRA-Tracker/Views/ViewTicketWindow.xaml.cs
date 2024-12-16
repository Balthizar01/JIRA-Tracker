using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JIRA_Tracker.Models;
using JIRA_Tracker.Services;

namespace JIRA_Tracker
{
    /// <summary>
    /// Interaction logic for ViewTicketWindow.xaml
    /// </summary>
    public partial class ViewTicketWindow : Window
    {
        private readonly Stack<int> _navigationHistory = new Stack<int>(); // Holds ticket IDs for navigation
        private int _currentTicketId;

        public ViewTicketWindow(Ticket ticket, Stack<int> navigationHistory = null)
        {
            InitializeComponent();

            // Clone the navigation history for this view session
            if (navigationHistory != null)
            {
                _navigationHistory = new Stack<int>(new Stack<int>(navigationHistory));
            }

            LoadTicketDetails(ticket);
        }

        private void LoadTicketDetails(Ticket ticket)
        {
            _currentTicketId = ticket.Id;

            // Clear UI fields
            TitleBlock.Text = string.Empty;
            SubmitterBlock.Text = string.Empty;
            DescriptionBlock.Text = string.Empty;
            StatusBlock.Text = string.Empty;
            PriorityBlock.Text = string.Empty;
            CreatedDateBlock.Text = string.Empty;
            UpdatedDateBlock.Text = string.Empty;
            ParentTicketBlock.Text = string.Empty;
            LinkedTicketsListBox.Items.Clear();
            CommentsListBox.Items.Clear();

            // Populate UI fields with new ticket data
            TitleBlock.Text = ticket.Title;
            SubmitterBlock.Text = ticket.Submitter;
            DescriptionBlock.Text = ticket.Description;
            StatusBlock.Text = ticket.Status;
            PriorityBlock.Text = ticket.Priority;
            CreatedDateBlock.Text = ticket.CreatedDate.ToString("g");
            UpdatedDateBlock.Text = ticket.UpdatedDate?.ToString("g") ?? "N/A";

            // Set up parent ticket link
            if (ticket.ParentId.HasValue)
            {
                ParentTicketBlock.Text = $"ID: {ticket.ParentId.Value}";
                ParentTicketBlock.MouseLeftButtonDown -= ParentTicket_MouseLeftButtonDown; // Avoid duplicate handlers
                ParentTicketBlock.MouseLeftButtonDown += ParentTicket_MouseLeftButtonDown;
            }
            else
            {
                ParentTicketBlock.Text = "None";
            }

            // Set up linked tickets
            foreach (var linkedId in ticket.LinkedTicketIds)
            {
                var item = new ListBoxItem { Content = $"ID: {linkedId}" };
                item.MouseLeftButtonDown -= LinkedTicket_MouseLeftButtonDown; // Avoid duplicate handlers
                item.MouseLeftButtonDown += LinkedTicket_MouseLeftButtonDown;
                LinkedTicketsListBox.Items.Add(item);
            }

            // Populate comments
            foreach (var comment in ticket.Comments)
            {
                CommentsListBox.Items.Add($"{comment.Author}: {comment.Text} (Created: {comment.CreatedDate:g})");
            }
        }

        private void ParentTicket_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && ParentTicketBlock.Text.StartsWith("ID: "))
            {
                var parentId = int.Parse(ParentTicketBlock.Text.Replace("ID: ", ""));
                NavigateToTicket(parentId);
            }
        }

        private void LinkedTicket_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && sender is ListBoxItem item && item.Content.ToString().StartsWith("ID: "))
            {
                var linkedId = int.Parse(item.Content.ToString().Replace("ID: ", ""));
                NavigateToTicket(linkedId);
            }
        }

        private void NavigateToTicket(int ticketId)
        {
            // Push the current ticket ID onto the navigation history stack
            _navigationHistory.Push(_currentTicketId);

            // Load the new ticket
            var db = new DatabaseService();
            var ticket = db.GetTickets().FirstOrDefault(t => t.Id == ticketId);
            Console.WriteLine(ticketId);

            if (ticket != null)
            {
                var viewTicketWindow = new ViewTicketWindow(ticket, _navigationHistory);
                Close(); // Close the current window
                viewTicketWindow.Show();
            }
            else
            {
                MessageBox.Show($"Ticket with ID {ticketId} not found.");
            }

        }

        private void BackToPreviousTicket()
        {
            if (_navigationHistory.Count > 0)
            {
                var previousTicketId = _navigationHistory.Pop();
                Console.WriteLine($"Navigating back to Ticket ID: {previousTicketId}"); // Debugging

                // Fetch the previous ticket from the database
                var db = new DatabaseService();
                var ticket = db.GetTickets().FirstOrDefault(t => t.Id == previousTicketId);

                if (ticket != null)
                {
                    // Reload the ticket details
                    LoadTicketDetails(ticket);
                }
                else
                {
                    MessageBox.Show($"Ticket with ID {previousTicketId} not found.");
                }
            }
            else
            {
                Close(); // Close the window if there's nothing to go back to
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackToPreviousTicket();
        }


    }
}
