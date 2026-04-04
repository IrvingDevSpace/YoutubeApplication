using System.Reflection;
using System.Windows.Controls;

namespace YoutubeApplication.Navigation
{
    internal class NavService : INavService
    {
        public Frame Frame { get; set; }

        private readonly Dictionary<string, Page> _pageByKey = new Dictionary<string, Page>();

        private readonly Dictionary<string, TypeInfo> _typeInfoByName;

        private Page _currentPage;

        private Page _prevPage;

        public NavService()
        {
            _typeInfoByName = Assembly.GetExecutingAssembly().DefinedTypes
                .Where(x => x.BaseType == typeof(Page))
                .ToDictionary(x => x.Name);
        }

        public void Navigate(string key, params object[] args)
        {
            if (!_typeInfoByName.TryGetValue(key, out var typeInfo))
                return;

            if (!_pageByKey.TryGetValue(key, out var page))
            {
                var instance = Activator.CreateInstance(typeInfo);
                if (instance is not Page p)
                    return;

                page = p;
                _pageByKey.Add(key, page);
            }

            if (page.DataContext is INavAware aware)
                aware.ApplyDataParams(args);

            if (_currentPage != null)
                _prevPage = _currentPage;

            _currentPage = page;
            Frame.Navigate(page);
        }
    }
}
