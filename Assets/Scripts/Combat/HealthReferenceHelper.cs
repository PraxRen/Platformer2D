using UnityEngine;

public class HealthReferenceHelper : MonoBehaviour
{
    [SerializeField] private Health _health;

    public Health Health => _health;
}