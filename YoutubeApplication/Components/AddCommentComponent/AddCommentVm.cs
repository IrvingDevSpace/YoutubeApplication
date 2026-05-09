using PropertyChanged;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.AddCommentComponent
{
    [AddINotifyPropertyChangedInterface]
    public class AddCommentVm
    {
        #region Dp
        public string ChannelImageUrl { get; set; }

        public ICommand OnCancelCmd { get; set; }

        public ICommand OnSubmitCmd { get; set; }
        #endregion

        public string ReplyText { get; set; }

        public ICommand CancelCmd { get; }

        public ICommand SubmitCmd { get; }

        public AddCommentVm()
        {
            CancelCmd = new AsyncRelayCommand(CancelAsync);
            SubmitCmd = new AsyncRelayCommand(SubmitAsync, () => !string.IsNullOrWhiteSpace(ReplyText));
        }

        private async Task CancelAsync()
        {
            await OnCancelCmd.ExecuteAsync();
            ReplyText = "";
        }

        private async Task SubmitAsync()
        {
            await OnSubmitCmd.ExecuteAsync(ReplyText);
            ReplyText = "";
        }
    }
}
