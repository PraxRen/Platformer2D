using UnityEngine;

[RequireComponent(typeof(Player))]
public class Picker : MonoBehaviour
{
    private Player _playerController;

    private void Start()
    {
        _playerController = GetComponent<Player>();
    }

    public void TakeItem(Item item)
    {
        if (item is ActionItem actionItem)
        {
            actionItem.Use(_playerController);
        }
    }
}