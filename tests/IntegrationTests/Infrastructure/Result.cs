namespace IntegrationTests.Infrastructure
{
    /// <summary>
    ///     Describes the state of a test result.
    /// </summary>
    public enum Result
    {
        /// <summary>
        ///     The state is unknown
        /// </summary>
        Unknown,

        /// <summary>
        ///     Successful test
        /// </summary>
        Success,

        /// <summary>
        ///     Failed test
        /// </summary>
        Failure
    }
}