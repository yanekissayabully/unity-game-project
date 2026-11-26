using System.Collections.Generic;
using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// Replays a recorded list of InputFrame through its own PlayerMotor.
    /// When the recording ends the clone keeps standing still.
    /// </summary>
    [RequireComponent(typeof(PlayerMotor))]
    public class CloneReplayer : MonoBehaviour
    {
        private PlayerMotor _motor;
        private IReadOnlyList<InputFrame> _frames;
        private int _index;
        private bool _running;

        private void Awake()
        {
            _motor = GetComponent<PlayerMotor>();
        }

        public void Init(IReadOnlyList<InputFrame> frames)
        {
            _frames = frames;
        }

        public void Restart(Vector2 spawnPosition)
        {
            _motor.ResetTo(spawnPosition);
            _index = 0;
            _running = true;
        }

        private void FixedUpdate()
        {
            if (!_running || _frames == null)
            {
                return;
            }

            InputFrame frame = default;
            if (_index < _frames.Count)
            {
                frame = _frames[_index];
                _index++;
            }

            _motor.Simulate(frame);
        }
    }
}
