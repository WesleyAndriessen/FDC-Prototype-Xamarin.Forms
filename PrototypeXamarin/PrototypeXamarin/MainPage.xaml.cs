using Newtonsoft.Json;
using PrototypeXamarin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PrototypeXamarin
{
    public partial class MainPage : ContentPage
    {
        private readonly string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "forms.txt");
        private readonly HttpClient _client;
        private readonly string _route = "http://649a351ac2c7.ngrok.io";

        public MainPage()
        {
            InitializeComponent();
            _client = new HttpClient();
        }

        private async void OnClearButtonClicked(object sender, EventArgs e)
        {
            await ClearFormsAsync().ConfigureAwait(false);
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(name.Text) && !string.IsNullOrEmpty(message.Text))
            {
                var form = new Form()
                {
                    Name = name.Text,
                    Message = message.Text
                };
                var json = JsonConvert.SerializeObject(form);
                File.WriteAllText(_fileName, json);
                name.Text = "";
                message.Text = "";
            }
        }

        private async void OnSyncButtonClicked(object sender, EventArgs e)
        {
            var formText = File.ReadAllText(_fileName);
            if (!string.IsNullOrEmpty(formText))
            {
                var formJson = JsonConvert.DeserializeObject<Form>(formText);
                await SyncFormsAsync(formJson).ConfigureAwait(false);
                File.WriteAllText(_fileName, "");
            }
        }

        private async void OnGetFormsButtonClicked(object sender, EventArgs e)
        {
            var forms = await GetFormsAsync().ConfigureAwait(false);
            Device.BeginInvokeOnMainThread(() => list.ItemsSource = forms);
        }

        private async Task<List<Form>> GetFormsAsync()
        {
            Uri uri = new Uri(_route + "/api/Sync/GetForms");
            HttpResponseMessage response = await _client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<Form>>(content);
            }
            return new List<Form>();
        }

        private async Task SyncFormsAsync(Form form)
        {
            Uri uri = new Uri(_route + "/api/Sync/SaveForm");
            string json = JsonConvert.SerializeObject(form);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PostAsync(uri, content).ConfigureAwait(false);
        }

        private async Task ClearFormsAsync()
        {
            Uri uri = new Uri(_route + "/api/Sync/ClearForms");
            await _client.PostAsync(uri, null).ConfigureAwait(false);
        }
    }
}
