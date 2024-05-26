using UnityEngine;
using System;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] Transform[] _waypoints;

    public void SetNextIndex(ref int currentIndex)
    {
        currentIndex++;
        currentIndex = currentIndex >= _waypoints.Length ? 0 : currentIndex;
    }

    public Vector3 GetWaypointPosition(int index)
    {
        if (index >= _waypoints.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _waypoints[index].transform.position;
    }
}