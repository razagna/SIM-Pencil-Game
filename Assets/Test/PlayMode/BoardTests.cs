using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardTests : MonoBehaviour
{
    GameObject testObject;
    Board board;
    int shape;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject();
        board = testObject.AddComponent<Board>();
        board.Init(8, shape = 6);
    }

    [TearDown] public void TearDown() => Destroy(testObject);

    [Test]
    public void ShouldInitializeBoard()
    {
        Assert.AreEqual(shape, board.vertices.Count);
        Assert.AreEqual(shape * (shape - 1) / 2, board.lineSegments.Count);
    }

    [Test]
    public void ShouldDrawBoard()
    {
        board.Draw(0.004f, 0.5f);
        Assert.AreEqual(shape, FindObjectsOfType<SpriteRenderer>().Length);
        Assert.AreEqual(shape * (shape - 1) / 2, FindObjectsOfType<LineRenderer>().Length);
    }

    [Test]
    public void ShouldFillBoard()
    {
        board.Draw(0.004f, 0.5f);
        board.FillBoard(new List<int>() { 1, 2, 9 }, Color.green);
        Assert.AreEqual(3, CountColor(Color.green));
    }

    [Test]
    public void ShouldResetBoard()
    {
        ShouldFillBoard();
        board.ResetBoard();
        Assert.AreEqual(0, CountColor(Color.green));
    }

    int CountColor(Color color)
    {
        int count = 0;
        foreach (LineSegment lineSegment in board.lineSegments)
            if (lineSegment.GetColor() == color) count += 1;
        return count;
    }

}
