using PropertyChanged;
using System.Windows;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.CommentComponent
{
    [AddINotifyPropertyChangedInterface]
    public class CommentThreadVm
    {
        #region Dp
        public CommentThreadItem CommentThread { get; set; }

        public ICommand OnSubCommentSubmitCmd { get; set; }

        public ICommand OnSaveCommentCmd { get; set; }

        public ICommand OnDelCommentCmd { get; set; }
        #endregion

        public bool IsExpanded { get; set; } = false;

        public ICommand ExpandCmd { get; }

        public ICommand CancelCmd { get; }

        public ICommand SubCommentSubmitCmd { get; }

        public Visibility EditViewVisibility { get; set; } = Visibility.Collapsed;

        public Visibility TextVisibility
            => EditViewVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

        public ICommand EditCommentCmd { get; }

        public ICommand EditCancelCommentCmd { get; }

        public ICommand SaveCommentCmd { get; }

        public ICommand DelCommentCmd { get; }

        public CommentThreadVm()
        {
            ExpandCmd = new RelayCommand(() => IsExpanded = true);
            CancelCmd = new RelayCommand(() => IsExpanded = false);
            SubCommentSubmitCmd = new AsyncRelayCommand<string>(SubCommentSubmitAsync, (replyText) => !string.IsNullOrWhiteSpace(replyText));
            EditCommentCmd = new RelayCommand(() => EditViewVisibility = Visibility.Visible);
            EditCancelCommentCmd = new RelayCommand(() => EditViewVisibility = Visibility.Collapsed);
            SaveCommentCmd = new AsyncRelayCommand<(string CommandId, string Text)>(SaveCommentAsync, (val) => !string.IsNullOrWhiteSpace(val.CommandId) && !string.IsNullOrWhiteSpace(val.Text));
            DelCommentCmd = new AsyncRelayCommand<string>(DelCommentAsync, (commandId) => !string.IsNullOrWhiteSpace(commandId));
        }

        private async Task SubCommentSubmitAsync(string replyText)
        {
            var parameter = (CommentThread, replyText);
            await OnSubCommentSubmitCmd.ExecuteAsync(parameter);
            IsExpanded = false;
        }

        private async Task SaveCommentAsync((string commandId, string text) parameter)
        {
            await OnSaveCommentCmd.ExecuteAsync(parameter);
        }

        private async Task DelCommentAsync(string commandId)
        {
            await OnDelCommentCmd.ExecuteAsync(commandId);
        }
    }
}
