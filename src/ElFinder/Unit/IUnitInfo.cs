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
        /// Gets a value indicates is unit exists
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Get relative path to root
        /// </summary>
        string RelativePath { get; }


        bool IsHidden { get; }

        /// <summary>
        /// Converts unit to data-trasfer object
        /// </summary>
        /// <returns>The data-transfer object</returns>
        UnitDTO ToDTO();
    }
}
