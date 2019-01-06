namespace Player.AnimatorState{
    public abstract class IAnimatorState
    {
        protected float ERROR = 0.2f;
        public int maxHeight = 4;
        public abstract void UpdatePosition();
        public abstract bool CanJump();
    }
}