using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private void Start()
    {
        Instantiate(_prefab, transform.position, Quaternion.identity);
    }
}
