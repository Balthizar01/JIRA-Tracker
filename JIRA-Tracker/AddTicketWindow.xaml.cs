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
    /// Interaction logic for AddTicketWindow.xaml
    /// </summary>
    public partial class AddTicketWindow : Window
    {
        public Ticket Ticket { get; private set; } = new Ticket();

        public AddTicketWindow()
        {
            InitializeComponent();
        }

        private void AddComment_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewCommentTextBox.Text))
            {
                var comment = new Comment
                {
                    Author = "Kyal",
                    Text = NewCommentTextBox.Text,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                Ticket.Comments.Add(comment);

                // Update UI
                CommentsListBox.Items.Add(comment.Text);
                NewCommentTextBox.Clear();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Ticket.Title = TitleBox.Text;
            Ticket.Submitter = AuthorBox.Text;
            Ticket.Description = DescriptionBox.Text;
            Ticket.Status = (StatusBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            Ticket.Priority = (PriorityBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            Ticket.CreatedDate = DateTime.Now;

            DialogResult = true; // Indicates successful save
            Close();
        }
    }
}
