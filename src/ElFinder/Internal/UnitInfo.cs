using System;
using System.Diagnostics.Contracts;

namespace ElFinder
{
    internal abstract class UnitInfo : IUnitInfo
    {
        public abstract string Name { get; }

        public IRoot Root
        {
            get { return m_root; }
        }

        public abstract UnitDTO ToDTO();

        protected UnitInfo(IRoot root)
        {
            Contract.Requires(root != null);

            m_root = root;
        }

        protected static readonly DateTime UnixOrigin = new DateTime(1970, 1, 1, 0, 0, 0);
        private readonly IRoot m_root;
    }
}
