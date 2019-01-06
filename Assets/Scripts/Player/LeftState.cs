using System;
using System.Collections.Generic;

namespace Player
{
    public class LeftState : IState
    {
        private Player _player;

        public LeftState(Player player, Dictionary<POSITION,float> positions){
            _player = player;
            XPosition = positions;
        }
        public override void Left(){}

        public override void Right()
        {
            _player.UpdateExpectedPosition(XPosition[POSITION.CENTER]);
            _player.UpdateState(new MiddleState(_player, XPosition));
        }
    }
}