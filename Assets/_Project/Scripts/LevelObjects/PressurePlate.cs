using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// A plate that is active while the player or any clone stands on it.
    /// It checks the area every physics step, so it also works when a character is teleported or destroyed.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class PressurePlate : MonoBehaviour
    {
        [SerializeField] private LayerMask _characterLayers;
        [SerializeField] private Vector2 _detectOffset = new Vector2(0f, 0.15f);
        [SerializeField] private Vector2 _detectSize = new Vector2(0.6f, 0.3f);
        [SerializeField] private Sprite _upSprite;
        [SerializeField] private Sprite _downSprite;

        private SpriteRenderer _renderer;

        public bool IsActive { get; private set; }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            Vector2 center = (Vector2)transform.position + _detectOffset;
            IsActive = Physics2D.OverlapBox(center, _detectSize, 0f, _characterLayers) != null;
            _renderer.sprite = IsActive ? _downSprite : _upSprite;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube((Vector2)transform.position + _detectOffset, _detectSize);
        }
    }
}
