using System.ComponentModel.DataAnnotations;

namespace ClientServer.Protocol
{
    /// <summary>
    /// Represents a request in the Custom JSON Transfer Protocol (CJTP).
    /// </summary>
    public class CJTPRequest
    {
        /// <summary>
        /// Gets or sets the method used in the request.
        /// </summary>
        [Required(ErrorMessage = "Method is required.")]
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the path associated with the request.
        /// </summary>
        [Required(ErrorMessage = "Path is required.")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the date associated with the request.
        /// </summary>
        [Required(ErrorMessage = "Date is required.")]
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the body of the request.
        /// </summary>
        [Required(ErrorMessage = "Body is required.")]
        public string Body { get; set; }
    }
}