using System.Collections.Generic;
using UnityEngine;

namespace Player{
    public class RightState : IState
    {
        private Player _player;
        public RightState(Player player, Dictionary<POSITION,float> positions){
            _player = player;
            XPosition = positions;
        }

        public override void Left()
        {
            Debug.Log("Ok player is on right and will move to middle!!!");
            _player.UpdateExpectedPosition(XPosition[POSITION.CENTER]);
            _player.UpdateState(new MiddleState(_player, XPosition));
        }

        public override void Right(){}
    }
}