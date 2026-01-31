using YoutubeAPI;

namespace YoutubeApplication.Context
{
    class YoutubeContextProvider
    {
        private static YoutubeContext _context;

        public static YoutubeContext Context
            => _context ??= new YoutubeContext();
    }
}
