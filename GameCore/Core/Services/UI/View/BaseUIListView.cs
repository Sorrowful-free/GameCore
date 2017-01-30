using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Core.Base.Async;
using GameCore.Core.Services.UI.ViewModel;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Core.Services.UI.View
{
    public enum UIListViewDirectionType
    {
        Horizontal,
        Vertical,
    }
    public abstract class BaseUIListView<TData,TRenderer> : BaseUIBehaviour
        where TRenderer : BaseUIItemRenderer<TData>
    {
        [SerializeField]
        private TRenderer _renderer;

        [SerializeField]
        private RectTransform _contentRoot;
        
        private BindingList<TData> _list;
        private List<TRenderer> _renderers = new List<TRenderer>();
        public async Task Initialize(BindingList<TData> list)
        {
            _list = list;
            _list.OnAdd += OnAddedItem;
            _list.OnRemove += OnRemovedItem;
            _list.OnInsert += OnInsertedItem;
            _list.OnClear += OnClearList;
            foreach (var data in _list)
            {
                var renderer = await AddRenderer(data);
                _renderers.Add(renderer);
            }
            
        }

        private async void OnAddedItem(int i, TData data)
        {
            var renderer = await AddRenderer(data);
            renderer.RectTransfrom.SetSiblingIndex(i);
        }

        private async void OnRemovedItem(int i, TData data)
        {
            await RemoveRenderer(i);
        }

        private async void OnInsertedItem(int i, TData data)
        {
            var renderer = await InsertRenderer(i,data);
            renderer.RectTransfrom.SetSiblingIndex(i);
        }

        private async void OnClearList()
        {
            while (_renderers.Count>0)
            {
                await RemoveRenderer(0);
            }
            _renderers.Clear();
        }
        
        private async Task<TRenderer> AddRenderer(TData data)
        {
            var go = await UnityAsync.Instantiate(_renderer.gameObject);
            var renderer = go.GetComponent<TRenderer>();
            renderer.RectTransfrom.SetParent(_contentRoot,false);
            _renderers.Add(renderer);
            return renderer;
        }

        private async Task<TRenderer> InsertRenderer(int i, TData data)
        {
            var go = await UnityAsync.Instantiate(_renderer.gameObject);
            var renderer = go.GetComponent<TRenderer>();
            renderer.RectTransfrom.SetParent(_contentRoot, false);
            _renderers.Insert(i,renderer);
            return renderer;
        }

        private async Task RemoveRenderer(int i)
        {
            var renderer = _renderers[i];
            await UnityAsync.Destroy(renderer.gameObject);
            _renderers.RemoveAt(i);
        }
    }
}
