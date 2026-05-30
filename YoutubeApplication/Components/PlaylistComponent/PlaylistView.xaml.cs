using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YoutubeApplication.Components.PlaylistComponent
{
    /// <summary>
    /// PlaylistView.xaml 的互動邏輯
    /// </summary>
    public partial class PlaylistView : UserControl
    {
        private readonly PlaylistVm _vm;

        public PlaylistView()
        {
            InitializeComponent();
            _vm = new PlaylistVm();
            Root.DataContext = _vm;
        }

        public PlaylistItemVm PlaylistItem
        {
            get => (PlaylistItemVm)GetValue(PlaylistItemDp);
            set => SetValue(PlaylistItemDp, value);
        }

        public static readonly DependencyProperty PlaylistItemDp =
            DependencyProperty.Register(
                nameof(PlaylistItem),
                typeof(PlaylistItemVm),
                typeof(PlaylistView),
                new PropertyMetadata((d, e) => ((PlaylistView)d)._vm.PlaylistItem = (PlaylistItemVm)e.NewValue)
            );

        public ICommand OnClickCmd
        {
            get { return (ICommand)GetValue(OnClickCmdDp); }
            set { SetValue(OnClickCmdDp, value); }
        }

        public static readonly DependencyProperty OnClickCmdDp =
            DependencyProperty.Register(
                nameof(OnClickCmd),
                typeof(ICommand),
                typeof(PlaylistView),
                new PropertyMetadata((d, e) => ((PlaylistView)d)._vm.OnClickCmd = (ICommand)e.NewValue)
            );
    }
}
