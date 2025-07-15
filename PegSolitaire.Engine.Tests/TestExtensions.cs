using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Tests;

public static class TestExtensions
{
    public static void SetState(this List<GameNode> nodes, IList<int> indicesToOccupy)
    {
        for (var i = 0; i < nodes.Count; i++)
            nodes[i].Occupied = indicesToOccupy.Contains(i);
    }

    public static void CheckState(this List<GameNode> nodes, IList<int> indicesShouldBeOccupied,
        string failErrorPreamble)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var shouldBeOccupied = indicesShouldBeOccupied.Contains(i);
            Assert.AreEqual(shouldBeOccupied, nodes[i].Occupied,
                $"{failErrorPreamble}node {i} (X: {nodes[i].X}, Y: {nodes[i].Y}) {(shouldBeOccupied ? "should be occupied" : "shouldn't be occupied")}, but it {(shouldBeOccupied ? "wasn't" : "was")}");
        }
    }
}