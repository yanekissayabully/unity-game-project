namespace RewindHeist
{
    /// <summary>
    /// Input for exactly one physics step. A list of these is a recording of a run.
    /// </summary>
    public readonly struct InputFrame
    {
        public readonly float Move;
        public readonly bool JumpPressed;
        public readonly bool JumpHeld;

        public InputFrame(float move, bool jumpPressed, bool jumpHeld)
        {
            Move = move;
            JumpPressed = jumpPressed;
            JumpHeld = jumpHeld;
        }
    }
}
