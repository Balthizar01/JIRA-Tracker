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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiteDB;
using JIRA_Tracker.Models;
using JIRA_Tracker.Services;

namespace JIRA_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DatabaseService _databaseService = new DatabaseService();
        private List<Ticket> _allTickets;
        private List<Ticket> _filteredTickets;

        public MainWindow()
        {
            InitializeComponent();
            LoadTickets();
        }

        private void LoadTickets()
        {

            _allTickets = _databaseService.GetTickets();

            // Apply default filter to exclude closed tickets
            FilterTickets(showClosed: false);

        }

        private void FilterTickets(bool showClosed)
        {
            if (showClosed)
            {
                // Display all tickets with the status of "Closed"
                _filteredTickets = _allTickets.Where(ticket => ticket.Status == "Closed").ToList(); // Only show closed tickets
            }
            else
            {
                _filteredTickets = _allTickets.Where(ticket => ticket.Status != "Closed").ToList(); // Exclude closed tickets
            }

            // Refresh the DataGrid
            TicketsGrid.ItemsSource = null;
            TicketsGrid.ItemsSource = _filteredTickets;
        }

        private void ShowClosedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FilterTickets(showClosed: true);
        }

        private void ShowClosedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            FilterTickets(showClosed: false);
        }

        // Add a new ticket
        private void AddTicket_Click(object sender, RoutedEventArgs e)
        {
            var addTicketWindow = new AddTicketWindow();
            if(addTicketWindow.ShowDialog() == true)
            {
                try
                {
                    _databaseService.AddTicket(addTicketWindow.Ticket);
                    LoadTickets();
                }
                catch( Exception ex)
                {
                    MessageBox.Show($"Error adding ticket:L {ex.Message}");
                }
            }
        }

        // Update an existing ticket
        private void EditTicket_Click(object sender, RoutedEventArgs e)
        {
            if (TicketsGrid.SelectedItem is Ticket selectedTicket)
            {
                var editTicketWindow = new EditTicketWindow(selectedTicket);
                if (editTicketWindow.ShowDialog() == true)
                {
                    try
                    {
                        _databaseService.UpdateTicket(editTicketWindow.Ticket);
                        LoadTickets();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating ticket: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a ticket to edit.");
            }
        }

        // Delete an existing ticket
        private void DeleteTicket_Click(object sender, RoutedEventArgs e)
        {
            if(TicketsGrid.SelectedItem is Ticket selectedTicket)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the ticket '{selectedTicket.Title}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if(result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _databaseService.DeleteTicket(selectedTicket.Id);
                        LoadTickets();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Error deleting ticket: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a ticket to delete.");
            }
        }

        private void TicketsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(TicketsGrid.SelectedItem is Ticket selectedTicket)
            {
                var viewTicketWindow = new ViewTicketWindow(selectedTicket);
                viewTicketWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a ticket to view.");
            }
        }
    }
}
