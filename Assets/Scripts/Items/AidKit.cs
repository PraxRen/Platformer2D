using UnityEngine;

[CreateAssetMenu(fileName = "NewAidKit", menuName = "Item/AidKit", order = 0)]
public class AidKit : ActionItem
{
    [SerializeField] float _healthPoints;

    public float HealthPoints => _healthPoints;

    public override void Use(Player player)
    {
        player.Health.Heal(_healthPoints);
    }
}