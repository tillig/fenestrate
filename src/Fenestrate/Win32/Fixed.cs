using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Fenestrate.Win32
{
    /// <summary>
    /// A fixed number structure for handling window point positions that may include floating point numbers.
    /// </summary>
    [SuppressMessage("SA1307", "SA1307", Justification = "Names must match the unmanaged structures.")]
    internal struct Fixed
    {
        /// <summary>
        /// The fractional part of the value.
        /// </summary>
        public short fract;

        /// <summary>
        /// The whole number part of the value.
        /// </summary>
        public short value;
    }
}
