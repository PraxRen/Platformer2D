using UnityEngine;

[CreateAssetMenu(fileName = "NewCoin", menuName = "Item/Coin", order = 0)]
public class Coin : ActionItem
{
    public override void Use(Player player)
    {
        if (player.TryGetComponent(out �oin�ounter scoreManager) == false)
        {
            throw new System.InvalidOperationException("��� �������������� � �������� ���������� ����� ��������!");
        }

        scoreManager.AddCoins(1);
    }
}
