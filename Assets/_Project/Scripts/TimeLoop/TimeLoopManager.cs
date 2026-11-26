using System;
using System.Collections.Generic;
using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// Runs the time loop: R saves the current run as a clone and starts over,
    /// T restarts the whole level and deletes all clones.
    /// </summary>
    public class TimeLoopManager : MonoBehaviour
    {
        /// <summary>Raised every time a new loop begins. Buttons, doors and guards reset themselves on it.</summary>
        public static event Action LoopReset;

        [SerializeField] private LivePlayer _player;
        [SerializeField] private CloneReplayer _clonePrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private int _maxClones = 2;
        [SerializeField] private float _loopDuration = 30f;
        [SerializeField] private KeyCode _rewindKey = KeyCode.R;
        [SerializeField] private KeyCode _restartKey = KeyCode.T;

        private readonly List<CloneReplayer> _clones = new List<CloneReplayer>();
        private float _loopTimer;

        public int CloneCount => _clones.Count;
        public int MaxClones => _maxClones;
        public float LoopTimeLeft => Mathf.Max(0f, _loopDuration - _loopTimer);

        private void Start()
        {
            StartLoop();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_rewindKey))
            {
                RewindToNextLoop();
            }
            else if (Input.GetKeyDown(_restartKey))
            {
                RestartLevel();
            }
        }

        private void FixedUpdate()
        {
            _loopTimer += Time.fixedDeltaTime;
            if (_loopTimer >= _loopDuration)
            {
                RewindToNextLoop();
            }
        }

        /// <summary>Saves the current run as a clone (if there is a free slot) and starts a new loop.</summary>
        public void RewindToNextLoop()
        {
            if (_clones.Count < _maxClones && _player.FrameCount > 0)
            {
                CloneReplayer clone = Instantiate(_clonePrefab, _spawnPoint.position, Quaternion.identity);
                clone.Init(new List<InputFrame>(_player.Recording));
                _clones.Add(clone);
            }

            StartLoop();
        }

        public void RestartLevel()
        {
            foreach (CloneReplayer clone in _clones)
            {
                Destroy(clone.gameObject);
            }

            _clones.Clear();
            StartLoop();
        }

        private void StartLoop()
        {
            _loopTimer = 0f;
            Vector2 spawn = _spawnPoint.position;
            _player.BeginLoop(spawn);

            foreach (CloneReplayer clone in _clones)
            {
                clone.Restart(spawn);
            }

            LoopReset?.Invoke();
        }
    }
}
