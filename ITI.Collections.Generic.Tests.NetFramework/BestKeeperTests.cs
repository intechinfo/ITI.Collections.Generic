using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ITI.Collections.Generic.Tests.NetFramework
{
    [TestFixture]
    public class BestKeeperTests
    {
        [Test]
        public void add_some_candidates()
        {
            const int HeapSize = 16;
            Random random = new Random();
            int[] randomValues = Enumerable.Range( 0, 1000 ).Select( _ => random.Next() ).ToArray();
            BestKeeper<int> sut = new BestKeeper<int>( HeapSize, ( n1, n2 ) => n1 - n2 );

            for( int i = 0; i < randomValues.Length; i++ )
            {
                sut.Add( randomValues[ i ] );
                Assert.That( sut.Count, Is.EqualTo( Math.Min( i + 1, HeapSize ) ) );
            }

            IEnumerable<int> best = randomValues.OrderByDescending( x => x ).Take( HeapSize );
            Assert.That( sut, Is.EquivalentTo( best ) );
        }
    }
}
