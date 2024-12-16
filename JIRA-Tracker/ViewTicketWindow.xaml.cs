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

namespace JIRA_Tracker
{
    /// <summary>
    /// Interaction logic for ViewTicketWindow.xaml
    /// </summary>
    public partial class ViewTicketWindow : Window
    {
        public ViewTicketWindow(Ticket ticket)
        {
            InitializeComponent();
            LoadTicketDetails(ticket);
        }

        private void LoadTicketDetails(Ticket ticket)
        {
            TitleBlock.Text = ticket.Title;
            AuthorBlock.Text = ticket.Submitter;
            DescriptionBlock.Text = ticket.Description;
            StatusBlock.Text = ticket.Status;
            PriorityBlock.Text = ticket.Priority;
            CreatedDateBlock.Text = ticket.CreatedDate.ToString("g");
            UpdatedDateBlock.Text = ticket.UpdatedDate?.ToString("g") ?? "N/A";

            // Load Comments
            foreach (var comment in ticket.Comments)
            {
                CommentsListBox.Items.Add($"{comment.Author}: {comment.Text} (Created: {comment.CreatedDate:g})");
            }
        }
    }
}
