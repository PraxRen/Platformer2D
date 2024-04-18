using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Picker : MonoBehaviour
{
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void TakeItem(Item item)
    {
        if (item is ActionItem actionItem)
        {
            actionItem.Use(_playerController);
        }
    }
}