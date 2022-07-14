using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class OrientationSpace : MonoBehaviour
{
    [SerializeField] private Direction _direction;
    [SerializeField] private UnityEvent _afterChangeDirection;

    public event UnityAction AfterChangeDirection
    {
        add => _afterChangeDirection.AddListener(value);
        remove => _afterChangeDirection?.RemoveListener(value);
    }

    public Direction Direction { get => _direction; }

    public void ChangeDirection(Direction newDirection)
    {
        if (_direction == newDirection)
            return;

        _direction = newDirection;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        _afterChangeDirection?.Invoke();
    }
}

public enum Direction
{
    Right,
    Left
}
