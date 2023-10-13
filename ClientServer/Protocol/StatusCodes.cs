namespace ClientServer.Protocol
{
    /// <summary>
    /// Provides constants representing status codes for CJTP (Custom JSON Transfer Protocol) responses.
    /// </summary>
    public static class StatusCodes
    {
        /// <summary>
        /// Represents a successful response code (1).
        /// </summary>
        public const int Ok = 1;

        /// <summary>
        /// Represents a response code indicating a resource was created (2).
        /// </summary>
        public const int Created = 2;

        /// <summary>
        /// Represents a response code indicating a resource was updated (3).
        /// </summary>
        public const int Updated = 3;

        /// <summary>
        /// Represents a response code indicating a missing method (4).
        /// </summary>
        public const int MissingMethod = 4;

        /// <summary>
        /// Represents a response code indicating a missing path (5).
        /// </summary>
        public const int MissingPath = 5;

        /// <summary>
        /// Represents a response code indicating a missing date (6).
        /// </summary>
        public const int MissingDate = 6;

        /// <summary>
        /// Represents a response code indicating an illegal method (7).
        /// </summary>
        public const int IllegalMethod = 7;
    }
}