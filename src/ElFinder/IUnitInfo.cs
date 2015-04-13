using System;

namespace ElFinder
{
    /// <summary>
    /// Represents generic unit - file or directory
    /// </summary>
    public interface IUnitInfo
    {
        /// <summary>
        /// Get name of unit
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets parent root
        /// </summary>
        IRoot Root { get; }

        /// <summary>
        /// Gets unit mime type
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// Gets 
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Converts unit to data-trasfer object
        /// </summary>
        /// <returns>The data-transfer object</returns>
        UnitDTO ToDTO();
    }
}
