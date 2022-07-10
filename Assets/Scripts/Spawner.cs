using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _countGameObject;
    [SerializeField] private float _delaySeconds;

    private void Start()
    {
        var createGameObjectJob = StartCoroutine(Create());
    }

    private IEnumerator Create()
    {
        var delay = new WaitForSeconds(_delaySeconds);

        for (int i = 0; i < _countGameObject; i++)
        {
            Instantiate(_prefab, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
            yield return delay;
        }
    }
}
