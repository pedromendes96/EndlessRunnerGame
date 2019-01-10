using System;
using UnityEngine;

namespace Player.AnimatorState{
    public class OnSuspendState : IAnimatorState
    {
        private float JumpWidth;
        private float finalWidth;
        private Player _player;

        public OnSuspendState(Player player){
            _player = player;
            JumpWidth = 2;
            finalWidth = (Mathf.Round(_player.transform.position.z * 100) / 100) + JumpWidth;
            _player.GetAnimator().SetTrigger("OnFloat");
        }
        public override bool CanJump()
        {
            return false;
        }

        public override void UpdatePosition()
        {
            float zVal = Mathf.Round(_player.transform.position.z * 100) / 100;
            if(zVal + ERROR > finalWidth){
                _player.UpdateAnimatorState(new OnDescendingState(_player));
            }else{
                float deltaZ = 1 * _player.airSpeed * Time.deltaTime;
                float ZPosDelta = Mathf.Round((zVal + deltaZ) * 100) / 100;
                if(Math.Abs(ZPosDelta) > Math.Abs(finalWidth)){
                    _player.transform.Translate(0, 0, finalWidth - zVal);
                }else{
                    _player.transform.Translate(0, 0, deltaZ);
                }
            }
        }
    }
}