using UnityEngine;

namespace VisualLayer.GamePlay.Handlers
{
    public class LoseHandler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("StandingFruit"))
            {
                Debug.Log("Game Over!");
                // Call your game over logic here
            }
        }
    }
}