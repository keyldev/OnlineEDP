using Newtonsoft.Json;
using OnlineEDP.Core;
using OnlineEDP.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineEDP.MVVM.ViewModel
{
    internal class LoginViewModel : ObservableObject
    {
        private string _userLoginText = "keyl";

        public string UserLoginText
        {
            get { return _userLoginText; }
            set { _userLoginText = value; NotifyPropertyChanged(); }
        }
        private string _userPasswordText = "word123";

        public string UserPasswordText
        {
            get { return _userPasswordText; }
            set { _userPasswordText = value; NotifyPropertyChanged(); }
        }

        public RelayCommand LoginCommand { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(async o =>
            {
                var user = await LoginUser();

                if (user != null)
                {
                    MainWindowViewModel.Instance.CurrentView = new MainViewModel(user);
                }
                else
                {
                    MessageBox.Show("Перепроверьте правильность ввода");
                }
            });
        }
        private string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
        private async Task<UserModel> LoginUser()
        {
            try
            {
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(new TempUser()
                {
                    uLogin = UserLoginText,
                    uPassword = ComputeHash(UserPasswordText, new MD5CryptoServiceProvider())
                });
                var data = new StringContent(json, UTF8Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync($"https://localhost:7138/api/user/login/", data);
                string message = await result.Content.ReadAsStringAsync();
                // MessageBox.Show(message);
                var user = JsonConvert.DeserializeObject<UserModel>(message);
                UserModel.User = user;
                return user;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }
    }
    class TempUser
    {
        public string uLogin { get; set; }
        public string uPassword { get; set; }
    }
}
