using UnityEngine;

[CreateAssetMenu(fileName = "NewAidKit", menuName = "Item/AidKit", order = 0)]
public class AidKit : ActionItem
{
    [SerializeField] float _healthPoints;

    public float HealthPoints => _healthPoints;

    public override void Use(Player player)
    {
        if (player.TryGetComponent(out HealthReferenceHelper healthReferenceHelper) == false && healthReferenceHelper.Health == null)
        {
            throw new System.InvalidOperationException("Для взаимодействия с аптечкой необходимо иметь здоровье!");
        }
        
        healthReferenceHelper.Health.Heal(_healthPoints);
    }
}