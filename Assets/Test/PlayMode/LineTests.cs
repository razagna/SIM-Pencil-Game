using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;


public class LineTests : MonoBehaviour
{
    GameObject testObject;
    LineSegment lineSegment;
    Vector3 startPoint, endPoint;

    [SetUp]
    public void SetUp()
    {
        testObject = new GameObject();

        startPoint = new Vector3(0, 0, 0);
        endPoint = new Vector3(1, 0.5f, 0);
        //lineSegment = new LineSegment(startPoint, endPoint);

        lineSegment = testObject.AddComponent<LineSegment>();
        lineSegment.Initialize(startPoint, endPoint);
    }

    [TearDown]
    public void TearDown() => Destroy(testObject);

    [Test]
    public void ShouldDrawLineSegment()
    {
        lineSegment.Draw(0.5f, false);
        LineRenderer lineRenderer = testObject.GetComponent<LineRenderer>();
        Assert.AreEqual(startPoint, lineRenderer.GetPosition(0));
        Assert.AreEqual(endPoint, lineRenderer.GetPosition(1));
    }

    [UnityTest]
    public IEnumerator ShouldInitializeLine()
    {
        //lineSegment.Draw(0.5f);
        //Input.simulateMouseWithTouches
        //yield return new WaitForSeconds(10);

        //Assert.AreEqual(Color.green, lineSegment.GetColor());

        yield return null;
    }


}
