namespace LiverPager.Grains.Tests.Integration
{
    public class IntegrationTestsFixture : IAsyncLifetime
    {
        public const string CollectionName = nameof(IntegrationTestsFixture);

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
