using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// Picks the sprite for idle, walking and jumping. Works for the player and the clones.
    /// </summary>
    [RequireComponent(typeof(PlayerMotor), typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] private Sprite _idle;
        [SerializeField] private Sprite _walk1;
        [SerializeField] private Sprite _walk2;
        [SerializeField] private Sprite _jump;
        [SerializeField] private float _frameTime = 0.13f;

        private PlayerMotor _motor;
        private SpriteRenderer _renderer;
        private float _timer;
        private int _step;

        private void Awake()
        {
            _motor = GetComponent<PlayerMotor>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!_motor.IsGrounded)
            {
                _renderer.sprite = _jump;
                return;
            }

            bool moving = Mathf.Abs(_motor.Velocity.x) > 0.1f;
            if (!moving)
            {
                _renderer.sprite = _idle;
                _timer = 0f;
                _step = 0;
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= _frameTime)
            {
                _timer -= _frameTime;
                _step = (_step + 1) % 4;
            }

            switch (_step)
            {
                case 0:
                    _renderer.sprite = _walk1;
                    break;
                case 2:
                    _renderer.sprite = _walk2;
                    break;
                default:
                    _renderer.sprite = _idle;
                    break;
            }
        }
    }
}
