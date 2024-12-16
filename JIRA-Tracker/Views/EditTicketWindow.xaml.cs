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

namespace JIRA_Tracker
{
    /// <summary>
    /// Interaction logic for EditTicketWindow.xaml
    /// </summary>
    public partial class EditTicketWindow : Window
    {
        public Ticket Ticket { get; private set; }

        public EditTicketWindow(Ticket ticket)
        {
            InitializeComponent();
            Ticket = ticket;
            LoadTicketDetails();
        }

        private void LoadTicketDetails()
        {
            TitleBox.Text = Ticket.Title;
            DescriptionBox.Text = Ticket.Description;

            foreach (ComboBoxItem item in StatusBox.Items)
                if ((string)item.Content == Ticket.Status)
                    StatusBox.SelectedItem = item;

            foreach (ComboBoxItem item in PriorityBox.Items)
                if ((string)item.Content == Ticket.Priority)
                    PriorityBox.SelectedItem = item;

            // Load comments
            foreach (var comment in Ticket.Comments)
                CommentsListBox.Items.Add(comment.Text);
        }

        private void AddComment_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(NewCommentTextBox.Text))
            {
                var comment = new Comment
                {
                    Author = "Kyal",
                    Text = NewCommentTextBox.Text,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                Ticket.Comments.Add(comment);

                CommentsListBox.Items.Add(comment.Text);
                NewCommentTextBox.Clear();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Ticket.Title = TitleBox.Text;
            Ticket.Description = DescriptionBox.Text;
            Ticket.Status = (StatusBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            Ticket.Priority = (PriorityBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            Ticket.UpdatedDate = DateTime.Now;

            // Parent ID
            if (int.TryParse(ParentIdBox.Text, out int parentId))
            {
                Ticket.ParentId = parentId;
            }

            // Linked Tickets
            if (!string.IsNullOrWhiteSpace(LinkedTicketsBox.Text))
            {
                Ticket.LinkedTicketIds = LinkedTicketsBox.Text
                    .Split(',')
                    .Select(id => int.TryParse(id.Trim(), out int result) ? result : 0)
                    .Where(id => id > 0)
                    .ToList();
            }

            DialogResult = true; // Indicates successful save
            Close();
        }
    }
}
