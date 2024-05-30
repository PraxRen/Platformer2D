using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private Dictionary<string, Coroutine> _hash = new Dictionary<string, Coroutine>();

    private void OnDisable()
    {
        foreach (var coroutine in _hash)
            StopCoroutine(coroutine.Value);
    }

    public void Create(string id, Action action)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id));

        if (action == null)
            throw new ArgumentNullException(nameof(action));

        if (_hash.ContainsKey(id))
            return;

        _hash[id] = StartCoroutine(UpdateAction(action));
    }

    public void Destroy(string id)
    {
        if (_hash.ContainsKey(id) == false)
            return;

        StopCoroutine(_hash[id]);
        _hash.Remove(id);
    }

    private IEnumerator UpdateAction(Action action)
    {
        while (true)
        {
            action?.Invoke();
            yield return null;
        }
    }
}