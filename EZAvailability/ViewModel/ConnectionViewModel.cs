using EZAvailability.Model;
using EZAvailability.ViewModel.Base;
using System;
using System.ComponentModel;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace EZAvailability.ViewModel;

public class ConnectionViewModel : BaseViewModel
{
    public static string ConnectionUrl { get; set; }
    private BaseViewModel baseViewModel = new BaseViewModel();

    //private static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private static string _filePath = FileSystem.AppDataDirectory + "/EZAConnection.json";

    public static async Task<int> AddConnection(string apiUrl)
    {

        try
        {
            if (!apiUrl.StartsWith("http://") && !apiUrl.StartsWith("https://"))
            {
                apiUrl = "https://" + apiUrl;
            }

            apiUrl = apiUrl.TrimEnd('/');

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    Connection conn = new Connection(apiUrl);
                    var writtenData = JsonSerializer.Serialize(conn);
                    File.WriteAllText(_filePath, writtenData);

                    Application.Current.MainPage = new AppShell();
                    return 0;
                }
                else
                {
                    Application.Current.MainPage = new AppShell();
                    return -1;
                }
            }
        } catch (Exception ex)
        {
            return -2;
        }

    }

    public static void GetConnection()
    {
        if (File.Exists(_filePath) && new FileInfo(_filePath).Length > 0)
        {
            string jsonDocument = File.ReadAllText(_filePath);

            JsonDocument jsonParse = JsonDocument.Parse(jsonDocument);
            JsonElement root = jsonParse.RootElement;

            if (root.TryGetProperty("ApiUrl", out JsonElement value))
            {
                ConnectionUrl = value.GetString();
            }
        } else
        {
            ConnectionUrl = "Not Found";
        }
    }

}