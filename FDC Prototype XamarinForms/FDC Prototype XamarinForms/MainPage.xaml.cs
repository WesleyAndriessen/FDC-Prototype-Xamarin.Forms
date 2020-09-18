using System;
using System.IO;
using Xamarin.Forms;

namespace FDCPrototypeXamarinForms
{
    public partial class MainPage : ContentPage
    {
        private readonly string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");

        public MainPage()
        {
            InitializeComponent();

            if (File.Exists(_fileName))
            {
                name.Text = File.ReadAllText(_fileName);
            }
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            File.WriteAllText(_fileName, name.Text);
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            File.WriteAllText(_fileName, name.Text);
        }

        private void OnSyncButtonClicked(object sender, EventArgs e)
        {
            File.WriteAllText(_fileName, name.Text);
        }

        private void OnGetFormsButtonClicked(object sender, EventArgs e)
        {
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
            name.Text = string.Empty;
        }
    }
}
