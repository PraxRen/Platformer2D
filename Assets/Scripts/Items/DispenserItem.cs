using UnityEngine;

public class DispenserItem : MonoBehaviour
{
    [SerializeField] private Item _item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Picker picker) == false)
            return;

        GiveItem(picker);
    }

    private void GiveItem(Picker picker)
    {
        picker.TakeItem(_item);
        Destroy(gameObject);
    }
}