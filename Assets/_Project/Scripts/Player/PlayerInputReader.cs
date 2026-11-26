using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// Reads the keyboard and turns it into one InputFrame per physics step.
    /// A jump press is remembered until the next physics step so it is never lost.
    /// </summary>
    public class PlayerInputReader : MonoBehaviour
    {
        private bool _jumpPressedPending;

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                _jumpPressedPending = true;
            }
        }

        public InputFrame ReadFrame()
        {
            float move = Input.GetAxisRaw("Horizontal");
            bool jumpPressed = _jumpPressedPending;
            _jumpPressedPending = false;
            bool jumpHeld = Input.GetButton("Jump");
            return new InputFrame(move, jumpPressed, jumpHeld);
        }
    }
}
