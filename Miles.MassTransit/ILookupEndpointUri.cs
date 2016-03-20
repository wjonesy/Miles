using System;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Looks up the endpoint Uri based on the specified type.
    /// </summary>
    public interface ILookupEndpointUri
    {
        /// <summary>
        /// Looks up the endpoint Uri based on the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        Task<Uri> LookupAsync(Type type);
    }
}
