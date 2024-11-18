using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiverPager.Grains.Tests.Unit
{
    [CollectionDefinition(Name)]
    public sealed class ClusterFixtureCollection : ICollectionFixture<ClusterFixture>
    {
        public const string Name = nameof(ClusterFixtureCollection);
    }
}
