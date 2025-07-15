using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;

namespace PegSolitaire.Engine.Tests.GameState;

[TestClass]
public class JumpTests
{
    [TestMethod]
    public void Jump_IsValidIsCorrect()
    {
        var fromNode = new GameNode(0, 0) { Occupied = true };
        var overNode = new GameNode(1, 1) { Occupied = true };
        var toNode = new GameNode(2, 2) { Occupied = true };
        overNode.AddAdjacent(fromNode);
        overNode.AddAdjacent(toNode);

        var jump = new Jump(fromNode, overNode, toNode);
        Assert.AreEqual(false, jump.IsValid, "A jump to an occupied node shouldn't be valid");

        toNode.Occupied = false;
        overNode.Occupied = false;
        Assert.AreEqual(false, jump.IsValid, "A jump over an unoccupied node shouldn't be valid");

        overNode.Occupied = true;

        fromNode.Occupied = false;
        toNode.Occupied = true;
        Assert.AreEqual(false, jump.IsValid, "A jump from an unoccupied node shouldn't be valid");

        fromNode.Occupied = true;
        toNode.Occupied = false;
        Assert.AreEqual(true, jump.IsValid,
            "A jump from an occupied node over an occupied node to an unoccupied node should be valid");
    }

    [TestMethod]
    public void Jump_CheckValidityIsCorrect()
    {
        var fromNode = new GameNode(0, 0) { Occupied = true };
        var overNode = new GameNode(1, 1) { Occupied = true };
        var toNode = new GameNode(2, 2) { Occupied = true };
        overNode.AddAdjacent(fromNode);
        overNode.AddAdjacent(toNode);

        var jump = new Jump(fromNode, overNode, toNode);
        Assert.ThrowsException<InvalidOperationException>(jump.CheckValidity,
            "A jump to an occupied node shouldn't be valid");

        toNode.Occupied = false;
        overNode.Occupied = false;
        Assert.ThrowsException<InvalidOperationException>(jump.CheckValidity,
            "A jump over an unoccupied node shouldn't be valid");

        overNode.Occupied = true;

        fromNode.Occupied = false;
        toNode.Occupied = true;
        Assert.ThrowsException<InvalidOperationException>(jump.CheckValidity,
            "A jump from an unoccupied node shouldn't be valid");

        fromNode.Occupied = true;
        toNode.Occupied = false;
        try
        {
            jump.CheckValidity();
        }
        catch (InvalidOperationException ex)
        {
            Assert.Fail(
                $"A jump from an occupied node over an occupied node to an unoccupied node should be valid; {ex}");
        }
    }
}