using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Fenestrate.Win32
{
    /// <summary>
    /// The POINT structure defines the x- and y- coordinates of a point.
    /// </summary>
    [SuppressMessage("SA1307", "SA1307", Justification = "Names must match the unmanaged structures.")]
    internal struct PointFx
    {
        /// <summary>
        /// The x-coordinate of the point.
        /// </summary>
        public Fixed x;

        /// <summary>
        /// The y-coordinate of the point.
        /// </summary>
        public Fixed y;
    }
}
