using YoutubeApplication.Common;

namespace YoutubeApplication.Presenters.Base
{
    public abstract class BasePresenter
    {
        protected async Task<Result> ExecuteAsync(Func<Task> action)
        {
            try
            {
                await action();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var data = await action();
                return Result<T>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure(ex.Message);
            }
        }
    }
}
