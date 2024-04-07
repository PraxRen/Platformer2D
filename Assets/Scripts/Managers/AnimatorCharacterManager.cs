using UnityEngine;

public class AnimatorCharacterManager : MonoBehaviour
{
    public class AnimatorIdsParams
    {
        public readonly int Speed = Animator.StringToHash("Speed");
        public readonly int IsJump = Animator.StringToHash("Jump");
        public readonly int IsFreeFall = Animator.StringToHash("FreeFall");
        public readonly int Attack = Animator.StringToHash("Attack");
        public readonly int Hit = Animator.StringToHash("Hit");
        public readonly int Die = Animator.StringToHash("Die");
        public readonly int IsDie = Animator.StringToHash("IsDie");
    }

    public static AnimatorCharacterManager Instance { get; private set; }

    public AnimatorIdsParams Params { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Params = new AnimatorIdsParams();
    }
}
