using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform[] _spawnPoints;

    private void Start()
    {
        foreach (Transform point in _spawnPoints) 
        {
            Instantiate(_prefab, point.position, Quaternion.identity);
        }
    }
}
