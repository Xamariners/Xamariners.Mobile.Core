using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamariners.Mobile.Core.Helpers;

namespace Xamariners.Mobile.Core.Controls.Collections
{
    public sealed class PagedObservableCollection<T> : ObservableCollection<IPagedObservableCollectionItem> where T : PagedObservableCollectionItem
    {
        private Func<Task<IList<IPagedObservableCollectionItem>>> _onLastItemLoaded;
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int CurrentIndex => (CurrentPage - 1) * PageSize;

        public Func<Task<IList<IPagedObservableCollectionItem>>> OnLastItemLoaded { get; private set; }

        public PagedObservableCollection(int pageSize, Func<Task<IList<IPagedObservableCollectionItem>>> onLastItemLoaded)
        {
            PageSize = pageSize;
            OnLastItemLoaded = onLastItemLoaded;
            CurrentPage = 1;
            TotalPages = 1;
        }

        public async Task Reset()
        {
            ThreadingHelpers.InvokeOnMainThread(Clear);
            CurrentPage = 1;
            TotalPages = 1;
        }

        public async Task Append(bool isOnUiThread = true)
        {
            var isFinished = (Count % PageSize) > 0;

            if (OnLastItemLoaded == null || isFinished)
                return;

            var results = await OnLastItemLoaded().ConfigureAwait(true); ;

            if (results == null)
                return;

            if (results.Count > PageSize)
                throw new Exception($"the pagesize is {PageSize} and {results.Count} attempted to be added");

            var lastIndex = Count + results.Count;

            for (int i = 0; i < results.Count; i++)
            {
                var item = results[i];

                item.Index = CurrentIndex + (i + 1);
                item.PageLastIndex = lastIndex;

                if (i == results.Count - 1)
                    item.OnLastItemLoaded = () => Append(true);

                if (isOnUiThread)
                {
                    // doesn't work for orders page
                    ThreadingHelpers.InvokeOnMainThread(() => Add(item));
                }
                else
                {
                    //// works for orders page
                    Add(item);
                }
            }

            TotalPages++;
            CurrentPage = TotalPages;
        }
    }
}