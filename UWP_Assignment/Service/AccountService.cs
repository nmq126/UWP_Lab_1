using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UWP_Assignment.Entity;
using Windows.Storage;

namespace UWP_Assignment.Service
{
    public class AccountService
    {
        public const string userTokenFileName = "dataUserLogin.txt";

        public async Task<bool> RegisterAsync(Account account)
        {
            var jsonString = JsonConvert.SerializeObject(account);
            HttpClient httpClient = new HttpClient();
            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, Config.Api.mediaType);
            var result = await httpClient.PostAsync($"{ Config.Api.apiDomain }{Config.Api.accountPath}", contentToSend);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                Debug.WriteLine("Register fail " + result);
                return false;
            }
        }

        public async Task<Credential> LoginAsync(string email, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("email", email);
            parameters.Add("password", password);
            var encodedContent = new FormUrlEncodedContent(parameters);
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync($"{ Config.Api.apiDomain }{Config.Api.loginPath}", encodedContent);
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SaveToken(content);
                    Credential obj = JsonConvert.DeserializeObject<Credential>(content);
                    return obj;
                }
                else
                {
                    Debug.WriteLine("Error: " + content);
                    return null;
                }
            }
        }

        // Lưu token
        private async void SaveToken(string content)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(userTokenFileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, content);
        }


        private async Task<Credential> LoadToken()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile storageFile = await storageFolder.GetFileAsync(userTokenFileName);
                string token = await FileIO.ReadTextAsync(storageFile);
                Credential credential = JsonConvert.DeserializeObject<Credential>(token);
                return credential;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<Account> GetAccountInformation(string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await httpClient.GetAsync($"{ Config.Api.apiDomain }{Config.Api.accountPath}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Account account = JsonConvert.DeserializeObject<Account>(content);
                    App.accountUser = account;
                    return account;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<Account> GetLoggedInAccount()
        {
            Account account;
            Credential credential = await LoadToken();
            if (credential == null)
            {
                return null;
            }
            account = await GetAccountInformation(credential.access_token);
            return account;

        }
    }
}