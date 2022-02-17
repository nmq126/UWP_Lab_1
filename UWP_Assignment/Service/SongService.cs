using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UWP_Assignment.Entity;
using Windows.Storage;

namespace UWP_Assignment.Service
{
    public class SongService
    {
        public const string userTokenFileName = "dataUserLogin.txt";
        public static async Task<Credential> LoadToken()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.GetFileAsync(userTokenFileName);
                string token = await FileIO.ReadTextAsync(sampleFile);
                Credential credential = JsonConvert.DeserializeObject<Credential>(token);
                return credential;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<Song>> GetAllSongAsync()
        {
            List<Song> listSong = new List<Song>();
            Credential credential = await LoadToken();
            if (credential == null) 
            {
                return listSong;
            }
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.access_token);
                var result = await httpClient.GetAsync($"{ Config.Api.apiDomain }{Config.Api.songPath}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    listSong = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                else
                {
                    Debug.WriteLine("Error");
                }
            }
            listSong.Reverse();
            return listSong;
        }
        public static async Task<bool> CreateSongAsync(Song song)
        {
            Credential credential = await LoadToken();
            var jsonString = JsonConvert.SerializeObject(song);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.access_token);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, Config.Api.mediaType);
            var result = await httpClient.PostAsync($"{ Config.Api.apiDomain }{Config.Api.mySongPath}", content);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<List<Song>> GetMySongAsync()
        {
            List<Song> listSong = new List<Song>();
            Credential credential = await LoadToken();
            if (credential == null) // không tồn tại file token
            {
                return listSong;
            }
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.access_token);
                var result = await httpClient.GetAsync($"{ Config.Api.apiDomain }{Config.Api.mySongPath}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    listSong = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                else
                {
                    Debug.WriteLine("Error");
                }
            }
            listSong.Reverse();
            return listSong;
        }
    }
}
