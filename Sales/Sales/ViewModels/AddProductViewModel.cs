using System;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class AddProductViewModel : BaseViewModel
    {
        #region Attributes

        private ApiServices apiService;

        private bool isRunning;

        private bool isEnabled;

        private ImageSource imageSource;

        private MediaFile file;

        #endregion

        #region Properties

        public string Description { get; set; }

        public string Price { get; set; }

        public string Remarks { get; set; }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }

        #endregion

        #region Constructors

        public AddProductViewModel()
        {

            this.apiService = new ApiServices();
            this.IsEnabled = true;
            this.ImageSource = "noproduct";
        }

        #endregion

        #region Methods

        #endregion

        #region Commands

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        private async void Save()
        {
            if (String.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.errorDescription,
                    Languages.Accept);
                return;
            }

            if (String.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.errorPrice,
                    Languages.Accept);
                return;
            }

            var price = decimal.Parse(this.Price);
            //decimal.TryParse(this.Price, price);
            if (price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.errorPrice,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnection = await this.apiService.CheckConnection();
            if (!checkConnection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, checkConnection.Message,
                    Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesManager.ReadFully(this.file.GetStream());
            }

            var product = new Product
            {
                Description = this.Description,
                Price = price,
                Remarks = this.Remarks,
                ImageArray = imageArray,
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Post(url, prefix, controller, product, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var newProduct = (Product)response.Result;
            var productsViewModel = ProductsViewModel.GetIntance();
            productsViewModel.MyProducts.Add(newProduct);
            productsViewModel.RefreshList();

            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();
            //await Application.Current.MainPage.Navigation.PopAsync();

        }

        public ICommand ChangeImageCommand
        {
            get { return new RelayCommand(ChanceImage); }
        }

        private async void ChanceImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.msgImageSource,
                Languages.Cancel,
                null,
                Languages.msgFromGallery,
                Languages.msgNewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.msgNewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    });
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        private async void TakePhoto()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }
            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var option = await Application.Current.MainPage.DisplayActionSheet("Photo Action with", null, null, "Take Photo", "Choose from Gallary");
                if (option == "Take Photo")
                {
                    this.file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        });
                    //var file = await CrossMedia.Current.TakePhotoAsync(
                    //    new StoreCameraMediaOptions
                    //    {
                    //        SaveToAlbum = true,
                    //        Directory = "Sample",
                    //        DefaultCamera = CameraDevice.Front,
                    //        PhotoSize = PhotoSize.Medium,
                    //        CompressionQuality = 92
                    //    });

                    //riderPhotoName = "RiderPhoto.jpg";
                    //riderphotoPreview.IsVisible = true;
                    //riderphotoPreview.Source = ImageSource.FromStream(() =>
                    //{
                    //    riderPhotoStream = file.GetStream();
                    //    return riderPhotoStream;
                    //});
                    //riderPhotoStream = file.GetStream();
                    //file.Dispose();
                }
                //else
                //{
                //    var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { });
                //    riderPhotoName = "RiderPhoto.jpg";
                //    riderphotoPreview.IsVisible = true;
                //    riderphotoPreview.Source = ImageSource.FromStream(() =>
                //    {
                //        riderPhotoStream = file.GetStream();
                //        return riderPhotoStream;
                //    });
                //    riderPhotoStream = file.GetStream();
                //    file.Dispose();
                //}
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                CrossPermissions.Current.OpenAppSettings();
            }

        }

        #endregion
    }
}
