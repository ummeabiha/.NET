using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Login
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void password_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        string email = emailText.Text;
        string password = passwordText.Text;

        if (!IsValidEmail(email))
        {
            MessageBox.Show("Please enter a valid email address.");
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Password field is required.");
            return;
        }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string directoryPath = Path.Combine(desktopPath, "userInfo");
            Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, $"{email}.txt");

            try
        {
            if (File.Exists(filePath))
            {
                MessageBox.Show("User already exists.");
                return;
            }

            using (StreamWriter signUp = new StreamWriter(filePath))
            {
                signUp.WriteLine($"Email: {email}");
                signUp.WriteLine($"Password: {password}");
            }

            MessageBox.Show("User Registered Successfully");
        }
        catch (IOException ex)
        {
            MessageBox.Show($"Error in Registering the User: {ex.Message}");
        }
    }


    private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
