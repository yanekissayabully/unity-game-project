using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// Moves a character with a Rigidbody2D. Used by the player and by every clone,
    /// the only difference is where the InputFrame comes from.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMotor : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _jumpSpeed = 13f;
        [SerializeField] private float _jumpCutMultiplier = 0.6f;

        [Header("Feel")]
        [SerializeField] private float _coyoteTime = 0.1f;
        [SerializeField] private float _jumpBufferTime = 0.1f;

        [Header("Ground check")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.6f, 0.12f);
        [SerializeField] private LayerMask _groundLayer;

        private Rigidbody2D _rb;
        private float _coyoteTimer;
        private float _jumpBufferTimer;

        public bool IsGrounded { get; private set; }
        public Vector2 Velocity => _rb.linearVelocity;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>Call exactly once per FixedUpdate.</summary>
        public void Simulate(InputFrame input)
        {
            float dt = Time.fixedDeltaTime;

            IsGrounded = Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0f, _groundLayer) != null;
            _coyoteTimer = IsGrounded ? _coyoteTime : _coyoteTimer - dt;
            _jumpBufferTimer = input.JumpPressed ? _jumpBufferTime : _jumpBufferTimer - dt;

            Vector2 velocity = _rb.linearVelocity;
            velocity.x = input.Move * _moveSpeed;

            if (_jumpBufferTimer > 0f && _coyoteTimer > 0f)
            {
                velocity.y = _jumpSpeed;
                _jumpBufferTimer = 0f;
                _coyoteTimer = 0f;
            }
            else if (!input.JumpHeld && velocity.y > 0f)
            {
                velocity.y *= _jumpCutMultiplier;
            }

            _rb.linearVelocity = velocity;
        }

        public void ResetTo(Vector2 position)
        {
            transform.position = position;
            _rb.position = position;
            _rb.linearVelocity = Vector2.zero;
            _coyoteTimer = 0f;
            _jumpBufferTimer = 0f;
        }

        private void OnDrawGizmosSelected()
        {
            if (_groundCheck == null)
            {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_groundCheck.position, _groundCheckSize);
        }
    }
}
