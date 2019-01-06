using System;
using UnityEngine;

namespace Player.AnimatorState{
    public class OnFloorState : IAnimatorState
    {
        private Player _player;
        public OnFloorState(Player player){
            _player = player;
            _player.GetAnimator().SetTrigger("OnFloor");
        }

        public override bool CanJump()
        {
            return true;
        }

        public override void UpdatePosition()
        {
            var result = Physics.Raycast(_player.transform.position, Vector3.down, 10f);
            Debug.Log("Result:" + result);
            if (!result)
            {
                _player.UpdateAnimatorState(new OnDescendingState(_player));
            }
                
            float xVal = Mathf.Round(_player.transform.position.x * 100) / 100;
            if(xVal == _player.expectedPosition){
                _player.transform.Translate(0, 0, 1 * _player.speed * Time.deltaTime);
            }else{
                int direction = xVal > _player.expectedPosition ? -1 : 1;
                float delta = direction * _player.moveSpeed * Time.deltaTime;
                float XPosDelta = Mathf.Round((xVal + delta) * 100) / 100;

                if(direction == 1){
                    if(XPosDelta > _player.expectedPosition){
                        _player.transform.Translate(xVal - XPosDelta, 0, 1 * _player.speed * Time.deltaTime);
                    }else{
                        _player.transform.Translate(delta, 0, 1 * _player.speed * Time.deltaTime);
                    }
                }else{
                    if(XPosDelta < _player.expectedPosition){
                        _player.transform.Translate(xVal + XPosDelta, 0, 1 * _player.speed * Time.deltaTime);
                    }else{
                        _player.transform.Translate(delta, 0, 1 * _player.speed * Time.deltaTime);
                    }
                }
            }
        }
    }
}