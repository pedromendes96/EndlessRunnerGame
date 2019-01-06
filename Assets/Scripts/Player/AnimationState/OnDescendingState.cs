using System.Timers;
using UnityEngine;

namespace Player.AnimatorState{
    public class OnDescendingState : IAnimatorState
    {
        private Player _player;
        private System.Timers.Timer aTimer;
        private bool isGameOver = false;
        
        public OnDescendingState(Player player)
        {
            _player = player;
            _player.GetAnimator().SetTrigger("OnDescending");
            SetTimer();
        }

        public override bool CanJump()
        {
            return false;
        }
        
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            isGameOver = true;
        }


        public override void UpdatePosition()
        {
            if (isGameOver)
                _player.GameOverAction();
            // Debug.Log("ISGROUNDED: " + _player.IsGrounded);
            if(!_player.IsGrounded()){
                float delta = -1 * _player.fallingSpeed * Time.deltaTime;
                _player.transform.Translate(0, delta, 1 * _player.speed * Time.deltaTime);
            }else{
                aTimer.Stop();
                aTimer.Dispose();
                _player.GetComponent<Rigidbody>().useGravity = true;
                _player.UpdateAnimatorState(new OnFloorState(_player));
            }
        }
    }
}