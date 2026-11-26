using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// The loot. The player or any clone can take it. It comes back on every new loop.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Gem : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Collider2D _collider;

        public bool IsCollected { get; private set; }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            TimeLoopManager.LoopReset += ResetGem;
        }

        private void OnDisable()
        {
            TimeLoopManager.LoopReset -= ResetGem;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsCollected || other.GetComponent<PlayerMotor>() == null)
            {
                return;
            }

            SetCollected(true);
        }

        private void ResetGem()
        {
            SetCollected(false);
        }

        private void SetCollected(bool collected)
        {
            IsCollected = collected;
            _renderer.enabled = !collected;
            _collider.enabled = !collected;
        }
    }
}
