using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(OrientationSpace))]
public class RayVision : MonoBehaviour
{
    [SerializeField] private ContactFilter2D _filter;
    [SerializeField] private UnityEvent _foundObject;
    [SerializeField] private UnityEvent _lostObject;

    public RaycastHit2D Result { get => _results[0]; }

    public readonly RaycastHit2D[] _results = new RaycastHit2D[1];
    private OrientationSpace _orientationSpace;
    private Vector3 _directionRaycast;
    private bool _isFound;

    private void OnEnable()
    {
        _orientationSpace = GetComponent<OrientationSpace>();
        _orientationSpace.AfterChangeDirection += DirectionVisual_AfterChangeDirection;
        DirectionVisual_AfterChangeDirection();
    }
 
    private void OnDisable()
    {
        _orientationSpace.AfterChangeDirection -= DirectionVisual_AfterChangeDirection;
        _orientationSpace = null;
    }

    private void DirectionVisual_AfterChangeDirection()
    {
        if (_orientationSpace.Direction == Direction.Left)
            _directionRaycast = transform.right * -1;
        else
            _directionRaycast = transform.right;
    }

    private void FixedUpdate()
    {
        int hitCount = Physics2D.Raycast(transform.position, _directionRaycast, _filter, _results, 3);
        Debug.DrawRay(transform.position, _directionRaycast * 3, Color.red);

        if (_isFound == false && hitCount > 0)
        {
            _isFound = true;
            _foundObject?.Invoke();
        }
        else if (_isFound == true && hitCount == 0)
        {
            _isFound = false;
            _lostObject.Invoke();
        }
    }
}
