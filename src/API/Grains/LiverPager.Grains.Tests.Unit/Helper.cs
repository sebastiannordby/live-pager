using System.Diagnostics;

namespace LiverPager.Grains.Tests.Unit
{
    public static class Helper
    {
        public static async Task WaitUntilCollectionHasEntries<T>(
            this TimeSpan timeout,
            IEnumerable<T> enumerable)
        {
            var sw = Stopwatch.StartNew();
            while (enumerable.Count() == 0 && sw.Elapsed < timeout)
            {
                await Task.Delay(100); // Poll every 100 ms
            }
        }
    }
}
