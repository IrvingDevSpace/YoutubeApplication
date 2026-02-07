using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.VideoCardComponent
{
    /// <summary>
    /// VideoCardView.xaml 的互動邏輯
    /// </summary>
    public partial class VideoCardView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ICommand OpenVideoCommand { get; }

        public VideoCardView()
        {
            InitializeComponent();

            OpenVideoCommand = new AsyncRelayCommand(
               execute: () => ExecuteOpenVideoAsync(VideoCard.VideoUrl),
               canExecute: () => !string.IsNullOrEmpty(VideoCard.VideoUrl)
            );
        }

        public VideoCard VideoCard
        {
            get => (VideoCard)GetValue(VideoCardProperty);
            set => SetValue(VideoCardProperty, value);
        }
        public static readonly DependencyProperty VideoCardProperty =
            DependencyProperty.Register(nameof(VideoCard), typeof(VideoCard), typeof(VideoCardView),
                new PropertyMetadata(null));

        public ICommand OnOpenVideoCommand
        {
            get { return (ICommand)GetValue(OnOpenVideoCommandProperty); }
            set { SetValue(OnOpenVideoCommandProperty, value); }
        }

        public static readonly DependencyProperty OnOpenVideoCommandProperty =
            DependencyProperty.Register(nameof(OnOpenVideoCommand), typeof(ICommand), typeof(VideoCardView),
                 new PropertyMetadata(null));


        //private ICommand _openVideoCommand;
        //public ICommand OpenVideoCommand
        //{
        //    get => _openVideoCommand;
        //    set
        //    {
        //        _openVideoCommand = value;
        //        OnPropertyChanged();
        //    }
        //}

        private async Task ExecuteOpenVideoAsync(string url)
        {
            //Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            // ....
            await OnOpenVideoCommand.ExecuteAsync(url);
        }
    }
}
