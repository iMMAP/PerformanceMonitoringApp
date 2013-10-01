using System;
using System.Collections.Generic;

    /// <summary>
    /// Generic Collection can hold any object
    /// Author: Asad Aziz
    /// Date: 07/04/2008
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class GCollection<T> : IList<T>
    {
        //=====================================================================================
        protected object head;
        protected object current;

        //=====================================================================================
        List<T> list = new List<T>();

        #region IList<T> Members

        //=====================================================================================
        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }
        //=====================================================================================
        public void Insert(int index, T item)
        {
            list.Insert(index, item);
        }
        //=====================================================================================
        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }
        //=====================================================================================
        public T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }
        //=====================================================================================
        #endregion

        #region ICollection<T> Members

        //=====================================================================================
        public void Add(T item)
        {
            list.Add(item);
        }
        //=====================================================================================
        public void Clear()
        {
            list.Clear();
        }
        //=====================================================================================
        public bool Contains(T item)
        {
            return list.Contains(item);
        }
        //=====================================================================================
        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }
        //=====================================================================================
        public int Count
        {
            get { return list.Count; }
        }
        //=====================================================================================
        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        //=====================================================================================
        public bool Remove(T item)
        {
            return list.Remove(item);
        }
        //=====================================================================================
        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for (int x = 0; x < list.Count; x++)
            {
                yield return list[x];
            }
        }
        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
    }


