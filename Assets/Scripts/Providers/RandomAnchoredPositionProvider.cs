using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using UnityEngine;

[Serializable]
public class RandomAnchoredPositionProvider : PositionProviderBase

{
    [SerializeField] List<Transform> _anchors = new List<Transform>();

    [SerializeField] float _minRandomOffsetHorizontal = 0f;
    [SerializeField] float _maxRandomOffsetHorizontal = 0f;
    [SerializeField] float _minRandomOffsetVertical = 0f;
    [SerializeField] float _maxRandomOffsetVertical = 0f;

    Vector3 randomPositionResult = Vector3.zero;

    public override Vector3 GetPosition()
    {
        if(_anchors == null || _anchors.Count == 0)
        {
            Assert.IsTrue(false, "Anchor list not set or empty in RandomAnchoredPositionProvider");
            return Vector3.zero;
        }

        Vector3 randomAnchorPosition =  _anchors[UnityEngine.Random.Range(0, _anchors.Count)].position;
        randomPositionResult.Set(randomAnchorPosition.x + UnityEngine.Random.Range(_minRandomOffsetHorizontal, _maxRandomOffsetHorizontal), randomAnchorPosition.y, randomAnchorPosition.z + UnityEngine.Random.Range(_minRandomOffsetVertical, _maxRandomOffsetVertical));
        return randomPositionResult;
    }
}
