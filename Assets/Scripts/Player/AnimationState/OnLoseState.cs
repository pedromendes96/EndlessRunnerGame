using UnityEngine;

namespace Player.AnimatorState
{
    public class OnLoseState : IAnimatorState
    {
        private Player _player;
        
        public OnLoseState(Player player)
        {
            _player = player;
            _player.GetRigidbody().useGravity = false;
            _player.GetAnimator().SetTrigger("OnGameOver");
        }
        
        public override bool CanJump()
        {
            return false;
        }

        public override void UpdatePosition()
        {
            
        }
    }
}