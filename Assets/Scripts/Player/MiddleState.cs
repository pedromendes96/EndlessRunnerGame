using System.Collections.Generic;

namespace Player
{
    public class MiddleState : IState
    {
        private Player _player;
        public MiddleState(Player player, Dictionary<POSITION,float> positions){
            _player = player;
            XPosition = positions;
        }

        public MiddleState(Player player){
            _player = player;
        }
        public override void Left()
        {
            _player.UpdateExpectedPosition(XPosition[POSITION.LEFT]);
            _player.UpdateState(new LeftState(_player, XPosition));
        }

        public override void Right()
        {
            _player.UpdateExpectedPosition(XPosition[POSITION.RIGHT]);
            _player.UpdateState(new RightState(_player,XPosition));
        }
    }
}