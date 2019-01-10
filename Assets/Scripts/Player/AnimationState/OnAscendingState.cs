using System;
using UnityEngine;

namespace Player.AnimatorState{
    public class OnAscendingState : IAnimatorState
    {
        private float JumpHeight = 2.5f;
        private float AbsoluteHeight;
        private Player _player;
        public OnAscendingState(Player player)
        {
            _player = player;
            _player.GetRigidbody().useGravity = false;
            AbsoluteHeight = (Mathf.Round(_player.transform.position.y * 100) / 100) + JumpHeight;
            Debug.Log("AbsoluteHeight: " + AbsoluteHeight);
            _player.GetAnimator().SetTrigger("OnUp");
        }

        public override bool CanJump()
        {
            return false;
        }

        public override void UpdatePosition()
        {
            float yVal = Mathf.Round(_player.transform.position.y * 100) / 100;
            if(yVal + ERROR > AbsoluteHeight){
                _player.UpdateAnimatorState(new OnSuspendState(_player));
            }else{
                float delta = 1 * _player.jumpSpeed * Time.deltaTime;
                float YPosDelta = Mathf.Round((yVal + delta) * 100) / 100;
                if(Math.Abs(YPosDelta) > Math.Abs(AbsoluteHeight)){
                    float remainder = AbsoluteHeight - yVal;
                    _player.transform.Translate(0, remainder, 1 * _player.speed * Time.deltaTime);
                }else{
                    _player.transform.Translate(0, delta, 1 * _player.speed * Time.deltaTime);
                }
            }
        }
    }
}