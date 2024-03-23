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
    private static string _filePath = FileSystem.CacheDirectory + "/EZAConnection.json";

    public static async Task<int> AddConnection(string apiUrl)
    {

        try
        {
            if (!apiUrl.StartsWith("http://") && !apiUrl.StartsWith("https://"))
            {
                apiUrl = "https://" + apiUrl;
            }

            apiUrl = apiUrl.TrimEnd('/');

                var _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(apiUrl);

                HttpResponseMessage response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {

                    Connection conn = new Connection(apiUrl);
                    var writtenData = JsonSerializer.Serialize(conn);
                    File.WriteAllText(_filePath, writtenData);

                    Application? current = Application.Current;
                    current.MainPage = new AppShell();
                    return 0;
                }
                else
                {
                    return -1;
                }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EZAvailability Errors] Error in AddConnection: {ex.Message}");
            return -2;
        }

    }

    public static void GetConnection()
    {
        try
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
            }
            else
            {
                ConnectionUrl = "Not Found";
            }
        } catch (Exception ex)
        {
            Console.WriteLine($"[EZAvailability Errors] Error in GetConnection: {ex.Message}");
        }

    }

}