using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// A door that opens while its pressure plates are active.
    /// When open it has no collider, so characters can walk through.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private PressurePlate[] _plates;
        [SerializeField] private bool _requireAllPlates;
        [SerializeField] private Sprite _closedSprite;
        [SerializeField] private Sprite _openSprite;

        private SpriteRenderer _renderer;
        private BoxCollider2D _collider;

        public bool IsOpen { get; private set; }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            IsOpen = ShouldBeOpen();
            _renderer.sprite = IsOpen ? _openSprite : _closedSprite;
            _collider.enabled = !IsOpen;
        }

        private bool ShouldBeOpen()
        {
            if (_plates == null || _plates.Length == 0)
            {
                return false;
            }

            foreach (PressurePlate plate in _plates)
            {
                if (_requireAllPlates && !plate.IsActive)
                {
                    return false;
                }

                if (!_requireAllPlates && plate.IsActive)
                {
                    return true;
                }
            }

            return _requireAllPlates;
        }
    }
}
