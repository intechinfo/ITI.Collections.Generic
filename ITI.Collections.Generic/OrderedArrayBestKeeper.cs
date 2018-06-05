﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ITI.Collections.Generic
{
    public class OrderedArrayBestKeeper<T> : IReadOnlyCollection<T>
    {
        readonly T[] _items;
        IComparer<T> _comparer;
        int _count;

        class ComparerAdapter : IComparer<T>
        {
            readonly Func<T, T, int> _comparator;

            public ComparerAdapter( Func<T, T, int> comparator )
            {
                _comparator = comparator;
            }

            public int Compare( T x, T y )
            {
                return _comparator( x, y );
            }
        }

        public OrderedArrayBestKeeper( int maxCount, Func<T, T, int> comparator = null )
        {
            if( maxCount < 0 ) throw new ArgumentException();
            if( comparator == null ) _comparer = Comparer<T>.Default;
            else _comparer = new ComparerAdapter( comparator );
            _items = new T[ maxCount ];
        }

        public bool Add( T candidate )
        {
            int idx = Array.BinarySearch( _items, 0, _count, candidate, _comparer );
            if( idx < 0 ) idx = ~idx;

            if( idx >= _count )
            {
                if( _count == _items.Length ) return false;
                _items[ idx ] = candidate;
                _count++;
                return true;
            }
            int toCopy = _count - idx;
            if( _count == _items.Length ) --toCopy;
            Array.Copy( _items, idx, _items, idx + 1, toCopy );
            _items[ idx ] = candidate;
            if( _count < _items.Length ) _count++;
            return true;
        }

        public int Count => _count;

        public IEnumerator<T> GetEnumerator() => _items.Take( _count ).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
