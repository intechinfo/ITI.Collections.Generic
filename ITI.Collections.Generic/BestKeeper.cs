using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ITI.Collections.Generic
{
    public class BestKeeper<T> : IReadOnlyCollection<T>
    {
        readonly T[] _items;
        readonly IComparer<T> _comparer;
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

        public BestKeeper( int maxCount, Func<T, T, int> comparator = null )
        {
            if( maxCount <= 0 ) throw new ArgumentException( "The max count must be greater than 0.", nameof( maxCount ) );
            if( comparator == null ) _comparer = Comparer<T>.Default;
            else _comparer = new ComparerAdapter( comparator );
            _items = new T[ maxCount ];
        }

        public bool Add( T candidate )
        {
            if( IsFull )
            {
                if( _comparer.Compare( candidate, _items[ 0 ] ) < 0 ) return false;
                RemoveMin();
            }

            DoAdd( candidate );
            return true;
        }

        public int Count => _count;

        public IEnumerator<T> GetEnumerator() => _items.Take( _count ).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        bool IsFull => _count == _items.Length;

        void DoAdd( T item )
        {
            _items[ _count ] = item;
            int idx = _count;

            while( idx > 0 )
            {
                int fatherIdx = ( idx - 1 ) / 2;
                if( _comparer.Compare( item, _items[ fatherIdx ] ) > 0 ) break;
                Swap( idx, fatherIdx );
                idx = fatherIdx;
            }

            _count++;
        }

        void RemoveMin()
        {
            Swap( 0, _count - 1 );
            _count--;
            int idx = 0;
            T item = _items[ 0 ];

            while( true )
            {
                int leftIdx = idx * 2 + 1;
                int rightIdx = idx * 2 + 2;

                int smallestIdx;
                if( leftIdx < _count && _comparer.Compare( _items[ leftIdx ], item ) < 0 ) smallestIdx = leftIdx;
                else smallestIdx = idx;
                if( rightIdx < _count && _comparer.Compare( _items[ rightIdx ], _items[ smallestIdx ] ) < 0 ) smallestIdx = rightIdx;

                if( smallestIdx == idx ) return;

                Swap( smallestIdx, idx );
                idx = smallestIdx;
            }
        }

        void Swap( int idx1, int idx2 )
        {
            T item = _items[ idx1 ];
            _items[ idx1 ] = _items[ idx2 ];
            _items[ idx2 ] = item;
        }
    }
}
