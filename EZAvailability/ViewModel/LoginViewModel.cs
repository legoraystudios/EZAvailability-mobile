using EZAvailability.ViewModel.Base;
using System.Text;
using System.Text.Json;

namespace EZAvailability.ViewModel;

public class LoginViewModel : BaseViewModel
{

    public LoginViewModel()
    {

    }

    public async Task<int> Login(string email, string password)
    {

        string endpoint = ConnectionViewModel.ConnectionUrl + "/auth/login";

        using (HttpClient client = new HttpClient())
        {

            var requestData = new
            {
                email = email,
                password = password
            };

            using StringContent jsonContent = new(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");


            HttpResponseMessage response = await client.PostAsync(endpoint, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return 200;
            } else
            {
                return (int)response.StatusCode;
            }
        }
    }

}