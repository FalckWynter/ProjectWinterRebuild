using UnityEngine;

namespace FlappyBird
{
    public class SpriteAnimator : MonoBehaviour
    {
        public Sprite[] frames;
        public int framesPerSecond = 30;
        public bool useUnscaledDeltaTime;
        
        private bool isActive = true;
        private float timer;
        private float timerMax;
        private int currentFrame;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            timerMax = 1f / framesPerSecond;
            spriteRenderer = transform.GetComponent<SpriteRenderer>();
            if (frames != null)
            {
                spriteRenderer.sprite = frames[0];
            }
            else
            {
                isActive = false;
            }
        }

        private void Update()
        {
            if (!isActive) return;
            timer += useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
            bool newFrame = false;
            while (timer >= timerMax)
            {
                timer -= timerMax;
                currentFrame = (currentFrame + 1) % frames.Length;
                newFrame = true;
            }
            if (newFrame)
            {
                spriteRenderer.sprite = frames[currentFrame];
            }
        }
    }
}