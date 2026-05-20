using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Utils;
using Xunit;

namespace Spice86.DataGrid.Tests;

/// <summary>
/// True logic test for the cycle-safe visual parent walk used by DataGrid_LostFocus.
///
/// DataGrid_LostFocus walks up the focused element's parent chain. If that chain contains
/// a cycle (which can happen with popups / detached visuals), a naive walk loops forever
/// and freezes the UI. The fix is <see cref="VisualParentWalker.EnumerateUniqueAncestors"/>,
/// which uses a HashSet to break out of cycles.
///
/// - RED  (without fix): EnumerateUniqueAncestors loops forever on a cyclic chain -> test times out -> FAIL
/// - GREEN (with fix):   EnumerateUniqueAncestors visits each node once then stops -> test PASSES
/// </summary>
public class VisualParentWalkerCycleTests
{
    private sealed class Node
    {
        public Node? Parent;
    }

    [Fact]
    public async Task EnumerateUniqueAncestors_OnCyclicChain_TerminatesAndVisitsEachNodeOnce()
    {
        // ==================== ARRANGE ====================
        // Build a cyclic parent chain: a -> b -> c -> a (cycle).
        Node a = new Node();
        Node b = new Node();
        Node c = new Node();
        a.Parent = b;
        b.Parent = c;
        c.Parent = a;

        List<Node> visited = new List<Node>();

        // ==================== ACT ====================
        // Run the walk on a worker thread with a hard timeout.
        // Without the HashSet guard, this loops forever and the task never completes.
        Task task = Task.Run(() =>
        {
            foreach (Node node in VisualParentWalker.EnumerateUniqueAncestors<Node>(a, n => n.Parent))
            {
                visited.Add(node);

                // Hard safety net so a buggy (cycle-blind) implementation cannot
                // run away with unbounded memory before the timeout fires.
                if (visited.Count > 1000)
                {
                    break;
                }
            }
        });

        await Task.Delay(TimeSpan.FromSeconds(2));
        bool completed = task.IsCompletedSuccessfully;

        // ==================== ASSERT ====================
        Assert.True(completed,
            "RED: VisualParentWalker.EnumerateUniqueAncestors did not terminate on a cyclic " +
            "parent chain within 2s. Without the HashSet<T> visited guard, the walk loops " +
            "forever and freezes the UI in DataGrid_LostFocus.");

        // Each node in the cycle must be visited exactly once, in order.
        Assert.Equal(new[] { a, b, c }, visited);
    }
}
