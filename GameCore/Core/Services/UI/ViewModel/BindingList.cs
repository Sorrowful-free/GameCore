using System;
using System.Collections;
using System.Collections.Generic;
using GameCore.Core.Extentions;

namespace GameCore.Core.Services.UI.ViewModel
{
    public class BindingList<TData> : IList<TData>
    {
        public event Action<int, TData> OnAdd;
        public event Action<int, TData> OnRemove;
        public event Action<int, TData> OnInsert;
        public event Action OnClear;
        
        private List<TData> _data = new List<TData>();

        public IEnumerator<TData> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TData item)
        {
            _data.Add(item);
            OnAdd.SafeInvoke(_data.IndexOf(item),item,ActionInvokationType.MainThread);
        }

        public void Clear()
        {
            _data.Clear();
            OnClear.SafeInvoke(ActionInvokationType.MainThread);
        }

        public bool Contains(TData item)
        {
            return _data.Contains(item);
        }

        public void CopyTo(TData[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public bool Remove(TData item)
        {
            var result = _data.Remove(item);
            OnRemove.SafeInvoke(_data.IndexOf(item), item, ActionInvokationType.MainThread);
            return result;
        }

        public int Count => _data.Count;
        public bool IsReadOnly { get { return false;} }
        public int IndexOf(TData item)
        {
            return _data.IndexOf(item);
        }

        public void Insert(int index, TData item)
        {
            _data.Insert(index,item);
            OnInsert.SafeInvoke(index,item);
        }

        public void RemoveAt(int index)
        {
            _data.RemoveAt(index);
            OnRemove.SafeInvoke(index, (index>=0 && index < _data.Count)?_data[index]:default(TData),ActionInvokationType.MainThread);
        }

        public TData this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }
    }
}
