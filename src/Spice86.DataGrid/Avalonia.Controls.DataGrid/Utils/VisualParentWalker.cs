#nullable enable

using System;
using System.Collections.Generic;

namespace Avalonia.Controls.Utils
{
    /// <summary>
    /// Walks a parent chain while guarding against cycles, so a cyclic visual-parent graph
    /// (which can happen with popups / detached visuals) cannot cause an infinite loop.
    /// </summary>
    internal static class VisualParentWalker
    {
        /// <summary>
        /// Enumerates <paramref name="start"/> and its ancestors as produced by <paramref name="getParent"/>,
        /// stopping when the chain ends (null) or when a node is encountered a second time (cycle).
        /// </summary>
        public static IEnumerable<T> EnumerateUniqueAncestors<T>(T? start, Func<T, T?> getParent)
            where T : class
        {
            if (start is null)
            {
                yield break;
            }

            HashSet<T> visited = new HashSet<T>(ReferenceEqualityComparer.Instance);
            T? current = start;
            while (current is not null)
            {
                if (!visited.Add(current))
                {
                    // Cycle detected - stop walking.
                    yield break;
                }

                yield return current;
                current = getParent(current);
            }
        }
    }
}
