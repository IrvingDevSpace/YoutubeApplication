using YoutubeAPI;
using YoutubeApplication.Common;
using YoutubeApplication.Presenters.Base;
using YoutubeApplication.Presenters.Interfaces;

namespace YoutubeApplication.Presenters
{
    public class LoginPresenter : BasePresenter, ILoginPresenter
    {
        private readonly YoutubeContext _context;

        public LoginPresenter(YoutubeContext context)
        {
            _context = context;
        }

        public async Task<Result> LoginAsync()
        {
            return await ExecuteAsync(_context.Auth.LoginAsync);
        }
    }
}
