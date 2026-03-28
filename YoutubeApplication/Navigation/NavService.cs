using System.Reflection;
using System.Windows.Controls;

namespace YoutubeApplication.Navigation
{
    internal class NavService : INavService
    {
        private readonly Frame _frame;

        private readonly Dictionary<string, Page> _pageByKey = new Dictionary<string, Page>();

        private readonly Dictionary<string, TypeInfo> _typeInfoByName;

        public NavService(Frame frame)
        {
            _frame = frame;
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

            _frame.Navigate(page);
        }
    }
}
