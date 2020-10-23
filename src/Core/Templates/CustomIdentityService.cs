using System.Threading;

namespace SQLConfirm.Core.Templates
{
    /// <summary>
    /// A service to generate a unique identity for the test
    /// </summary>
    public static class CustomIdentityService
    {
        private static int _latestIdentity = 1000;

        /// <summary>
        /// Generates the next identity
        /// </summary>
        /// <returns>Returns the next identity value</returns>
        public static int GenerateNextIdentity()
        {
            return Interlocked.Increment(ref _latestIdentity);
        }
    }
}
