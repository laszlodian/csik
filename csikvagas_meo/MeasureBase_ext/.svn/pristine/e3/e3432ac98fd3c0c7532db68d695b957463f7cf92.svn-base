using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using e77.MeasureBase;
using e77.MeasureBase.Model;
using System.Collections.Specialized;

namespace e77.MeasureBase.Data
{
    public class HierarchyMultiValueDictionary<T>
        : MultiValueDictionary<string, T>
    {
        public List<T> GetAllChildren(string partialKey_in)
        {
            List<T> res = new List<T>();

            foreach (string key in Keys)
                if(key.StartsWith(partialKey_in))
                    res.AddRange(this[key]);
         
            return res;
        }
                
        /// <summary>
        ///  Returns the full List of strings with the name of the submeasurements
        ///  which is given as input parameter
        /// </summary>
        /// <param name="parent_in">The name of the measure</param>
        /// <returns>A List of strings with the measure sub-elements</returns>
        public List<string> GetAllChildrenId(string parent_in)
        {
            return HierarchyDatabaseHelper.GetAllChildId<List<T>>(_Items, parent_in);
        }

        /// <summary>
        /// Removes all items beginning with partialKey_in
        /// </summary>
        /// <param name="partialKey_in"></param>
        public void RemoveAllItems(string partialKey_in)
        {
            List<T> removedItems = new List<T>();

            foreach(string key in Keys)
                if (key.StartsWith(partialKey_in))
                {
                    removedItems.AddRange(this._Items[key]);
                    this._Items.Remove(key);
                }

            if (removedItems.Count > 0)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
        }

        /// <summary>
        /// removes items of key_in
        /// </summary>
        /// <param name="key_in"></param>
        public void RemoveItems(string key_in)
        {
            if (this._Items.ContainsKey(key_in))
            {
                List<T> removedItems = new List<T>();
                removedItems.AddRange(this._Items[key_in]);
                this._Items.Remove(key_in);

                if (removedItems.Count > 0)
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
            }
        }

        /// <summary>
        /// Removes item(s) of key_in
        /// </summary>
        /// <param name="key_in">CollectionKey of the item you want to remove</param>
        public void RemoveItem(string key_in,  T item_in)
        {
            List<T> removedItems = new List<T>();
            if (this._Items[key_in].Contains(item_in))
            {
                if (this._Items[key_in].Count == 1)
                {
                    //last item of collectionKey is removing -> Remove collectionKey
                    RemoveItems(key_in);
                    return;
                }
                else
                {
                    removedItems.Add(item_in);
                    this._Items[key_in].Remove(item_in);
                }
            }
    
            if (removedItems.Count > 0)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
        }

    }

    /// <summary>
    /// Contains a dictionary, where same key can contain more TValue.
    /// </summary>
    [DebuggerDisplay("Key count = {KeysCount} Values count = {ValuesCount}")]
    public class MultiValueDictionary<TKey, TValue> : INotifyCollectionChanged
    {
        public List<TKey> Keys
        {
            
            get { return new List<TKey>(_Items.Keys); } //todo_fgy use same method as Dictionary does
        }

        public List<TValue> Values
        {
            get 
            {
                List<TValue> res = new List<TValue>(512);
                foreach(List<TValue> leafList in _Items.Values)
                    res.AddRange(leafList);
                return res; 
            } //todo_fgy use same method as Dictionary does
        }

        /// <summary>
        /// Gets key of an item. 
        /// This function is not fast. (O = 1/2n)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>null if not found</returns>
        public TKey KeyOf(TValue value)
        {
            foreach (TKey key in _Items.Keys)
                if( _Items[key].Contains(value) )
                    return key;

            throw new NotFoundException(value.ToString());
        }

        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            Add(pair.Key, pair.Value);
        }

        public void Add(TKey key, TValue value)
        {
            if (!_Items.ContainsKey(key))
                _Items.Add(key, new List<TValue>());

            _Items[key].Add(value);

            _ValuesCount++;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value) );
        }

        public void AddRange(TKey key, IEnumerable<TValue> values_in)
        {
            if (!_Items.ContainsKey(key))
                _Items.Add(key, new List<TValue>());

            _Items[key].AddRange(values_in);

            _ValuesCount += values_in.Count();

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, values_in));//ew NotifyCollectionChangedEventArgs() );
        }

        
        public bool ContainsKey(TKey key_in)
        {
            return _Items.ContainsKey(key_in);
        }

        /// <summary>
        /// Gets all value associated with the specified key. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>>empty list if key not found</returns>
        public List<TValue> this[TKey key]
        {
            get
            {
                return GetItems(key);
            }
        }

        /// <summary>
        /// Gets all value associated with the specified key. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>empty list if key not found</returns>
        public List<TValue> GetItems(TKey key)
        {
            if (!_Items.ContainsKey(key))
            {
                return new List<TValue>();
            }
            return _Items[key];
        }

        /*
        public Dictionary<TKey, TValue>.KeyCollection Keys 
        {
            get { return (Dictionary<TKey, TValue>)_Items.Keys; }
        }*/

        public virtual void Clear()
        {
            _Items.Clear();
            _ValuesCount = 0;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) );
        }


        protected int _ValuesCount;

        protected int ValuesCount
        {
            get { return _ValuesCount; }
        }

        protected int KeysCount
        {
            get { return _Items.Keys.Count; }
        }

        public string AllTraceInfo
        {
            get
            {
                StringBuilder res = new StringBuilder();

                res.AppendFormat("{0}", GetType().Name);

                foreach (KeyValuePair<TKey, List<TValue>> pair in _Items)
                {
                    res.AppendFormat("\nKey: '{0}'\n", pair.Key.ToString());
                    foreach(TValue v in pair.Value)
                        res.AppendFormat("\t{0}", v);
                }
                res.AppendLine();

                return res.ToString();
            }
        }

        /// <summary>
        /// Contains same type MeasureSteps. 
        /// key: collection ID, where steps belongs to one measure (same measure at same position)
        /// </summary>
        protected SortedDictionary<TKey, List<TValue>> _Items =
            new SortedDictionary<TKey, List<TValue>>();


        #region INotifyCollectionChanged Members

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs arg_in)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, arg_in);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
    }
}
