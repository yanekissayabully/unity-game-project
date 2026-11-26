using System;
using UnityEngine;

namespace RewindHeist
{
    /// <summary>
    /// The exit. It works only for the live player and only when all required gems are taken.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class ExitGate : MonoBehaviour
    {
        public static event Action LevelCompleted;

        [SerializeField] private Gem[] _requiredGems;

        private bool _completed;

        private void OnEnable()
        {
            TimeLoopManager.LoopReset += HideMessage;
        }

        private void OnDisable()
        {
            TimeLoopManager.LoopReset -= HideMessage;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<LivePlayer>() == null)
            {
                return;
            }

            foreach (Gem gem in _requiredGems)
            {
                if (!gem.IsCollected)
                {
                    return;
                }
            }

            _completed = true;
            Debug.Log("LEVEL CLEAR");
            LevelCompleted?.Invoke();
        }

        private void HideMessage()
        {
            _completed = false;
        }

        // Temporary on-screen message until the real UI is made.
        private void OnGUI()
        {
            if (!_completed)
            {
                return;
            }

            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 48,
                alignment = TextAnchor.MiddleCenter
            };
            style.normal.textColor = Color.green;
            GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), "LEVEL CLEAR", style);
        }
    }
}
