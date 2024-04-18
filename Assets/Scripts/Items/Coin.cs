using UnityEngine;

[CreateAssetMenu(fileName = "NewCoin", menuName = "Item/Coin", order = 0)]
public class Coin : ActionItem
{
    public override void Use(PlayerController player)
    {
        ScoreManager.Instance.AddCoins(1);
    }
}
