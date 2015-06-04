using System;
using System.Diagnostics.Contracts;

namespace ElFinder
{    
    /// <summary>
    /// Represents generic unit - file or directory
    /// </summary>
    [ContractClass(typeof(ConstractForIUnitInfo))]
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

        /// <summary>
        /// Gets a value indicates is unit hidden
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        /// Converts unit to data-trasfer object
        /// </summary>
        /// <returns>The data-transfer object</returns>
        UnitDTO ToDTO();
    }

    [ContractClassFor(typeof(IUnitInfo))]
    internal abstract class ConstractForIUnitInfo : IUnitInfo
    {
        string IUnitInfo.Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        IRoot IUnitInfo.Root
        {
            get
            {
                Contract.Ensures(Contract.Result<IRoot>() != null);
                return null;
            }
        }

        string IUnitInfo.MimeType
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return null;
            }
        }

        bool IUnitInfo.Exists
        {
            get { return true; }
        }

        string IUnitInfo.RelativePath
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        bool IUnitInfo.IsHidden
        {
            get { return true; }
        }

        UnitDTO IUnitInfo.ToDTO()
        {
            Contract.Ensures(Contract.Result<UnitDTO>() != null);
            return null;
        }
    }
}
