using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamariners.Mobile.Core.Helpers.MVVM;
using Xamariners.Mvvm.Core;

namespace Xamariners.Mobile.Core.Controls.Collections
{
    public class PagedObservableCollectionItem : BindableBase, IPagedObservableCollectionItem
    {
        public Func<Task> OnLastItemLoaded { get; set; }
        public int PageLastIndex { get; set; }

        private int _index;
        public int Index
        {
            get
            {
                Task.Run(async () => await CheckLastPageIndex()).ConfigureAwait(true); ;
                return _index;
            }

            set => _index = value;
        }

        private async Task<bool> CheckLastPageIndex()
        {
            // TODO: check correct page
            if (_index == PageLastIndex && OnLastItemLoaded != null)
            {
                await OnLastItemLoaded.Invoke().ConfigureAwait(true); ;
                OnLastItemLoaded = null;
                return true;
            }

            return false;
        }
    }

    public interface IPagedObservableCollectionItem
    {
       Func<Task> OnLastItemLoaded { get; set; }
       int Index { get; set; }
       int PageLastIndex { get; set; }
    }
}