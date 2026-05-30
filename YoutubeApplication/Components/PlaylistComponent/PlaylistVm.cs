using PropertyChanged;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.PlaylistComponent
{
    [AddINotifyPropertyChangedInterface]
    public class PlaylistVm
    {
        #region Dp
        public PlaylistItemVm PlaylistItem { get; set; }

        public ICommand OnClickCmd { get; set; }
        #endregion

        public ICommand ClickCmd { get; set; }

        public PlaylistVm()
        {
            ClickCmd = new AsyncRelayCommand(ClickAsync, () => !string.IsNullOrWhiteSpace(PlaylistItem?.PlaylistId));
        }

        private async Task ClickAsync()
        {
            await OnClickCmd.ExecuteAsync((PlaylistItem.PlaylistId, PlaylistItem.IsSelected));
        }
    }
}
