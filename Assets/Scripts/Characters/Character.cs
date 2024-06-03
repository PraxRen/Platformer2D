using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Health Health;
    [SerializeField] protected Mover Mover;
    [SerializeField] protected Fighter Fighter;
    [SerializeField] protected GameObject UI;

    private static int s_idLast;

    public int Id { get; private set; }

    private void Awake()
    {
        Id = ++s_idLast;
    }

    private void OnEnable()
    {
        Health.Died += OnDied;
        HandleEnable();
    }

    private void OnDisable()
    {
        Health.Died -= OnDied;
        HandleDisable();
    }

    protected abstract void HandleEnable();
    protected abstract void HandleDisable();

    private void OnDied()
    {
        enabled = false;
        Mover.enabled = false;
        Fighter.enabled = false;
        Health.enabled = false;
        UI.SetActive(false);
    }
}