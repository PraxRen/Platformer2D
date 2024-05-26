using UnityEngine;

public static class CharacterAnimatorData
{
    public static class Params
    {
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int IsJump = Animator.StringToHash("Jump");
        public static readonly int IsFreeFall = Animator.StringToHash("FreeFall");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Hit = Animator.StringToHash("Hit");
        public static readonly int Die = Animator.StringToHash("Die");
        public static readonly int IsDie = Animator.StringToHash("IsDie");
    }
}
