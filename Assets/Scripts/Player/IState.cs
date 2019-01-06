using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class IState
    {
        public enum POSITION
        {
            LEFT,
            CENTER,
            RIGHT
        }

        private const float AbsoluteDifferenceDistance = 2.5f;

        public Dictionary<POSITION, float> XPosition = new Dictionary<POSITION, float>(){
            {
                POSITION.LEFT, -2.5f
            },
            {
                POSITION.CENTER, 0
            },
            {
                POSITION.RIGHT, 2.5f
            }
        };
        public abstract void Right();
        public abstract void Left();
    }
}
