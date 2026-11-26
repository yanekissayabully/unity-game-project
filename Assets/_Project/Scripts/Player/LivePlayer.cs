using System.Collections.Generic;
using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// The character the human controls. Every physics step it reads the keyboard,
    /// stores the input in a recording and moves the motor.
    /// </summary>
    [RequireComponent(typeof(PlayerMotor), typeof(PlayerInputReader))]
    public class LivePlayer : MonoBehaviour
    {
        private readonly List<InputFrame> _recording = new List<InputFrame>();

        private PlayerMotor _motor;
        private PlayerInputReader _reader;
        private bool _active;

        public IReadOnlyList<InputFrame> Recording => _recording;
        public int FrameCount => _recording.Count;

        private void Awake()
        {
            _motor = GetComponent<PlayerMotor>();
            _reader = GetComponent<PlayerInputReader>();
        }

        public void BeginLoop(Vector2 spawnPosition)
        {
            _recording.Clear();
            _motor.ResetTo(spawnPosition);
            _active = true;
        }

        private void FixedUpdate()
        {
            if (!_active)
            {
                return;
            }

            InputFrame frame = _reader.ReadFrame();
            _recording.Add(frame);
            _motor.Simulate(frame);
        }
    }
}
