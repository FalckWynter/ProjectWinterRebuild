using System;
using System.Collections;
using QFramework;
using UnityEngine;

namespace FlappyBird
{
    public class Bird : BaseController
    {
        private const float JUMP_AMOUNT = 90f;
        private Rigidbody2D birdRigidbody2D;
        private GameRuntimeModel.State state;

        private void Awake()
        {
            birdRigidbody2D = GetComponent<Rigidbody2D>();
            this.GetModel<GameRuntimeModel>().GameState.Register(UpdateState)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void UpdateState(GameRuntimeModel.State state)
        {
            this.state = state;
            switch (state)
            {
                case GameRuntimeModel.State.WaitingToStart:
                    transform.position = Vector3.zero;
                    birdRigidbody2D.simulated = false;
                    break;
                case GameRuntimeModel.State.Playing:
                    birdRigidbody2D.simulated = true;
                    break;
                case GameRuntimeModel.State.BirdDead:
                    break;
            }
        }

        private void Update()
        {
            if (state == GameRuntimeModel.State.Playing && Input.GetMouseButtonDown(0))
            {
                Jump();
            }
            transform.eulerAngles = new Vector3(0, 0, birdRigidbody2D.velocity.y * .15f);
        }

        private void Jump()
        {
            birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
            this.GetSystem<AudioSystem>().PlaySingleSound("Sounds/BirdJump");
        }
        
        private void OnCollisionEnter2D(Collision2D collider)
        {
            this.SendCommand<BirdDeadCommand>();
            
            this.GetSystem<AudioSystem>().PlaySingleSound("Sounds/Lose");
            
            StartCoroutine(Delay(1, () =>
            {
                this.GetSystem<UISystem>().OpenUI(nameof(GameOverWindow));
                this.GetSystem<UISystem>().CloseUI(nameof(ScoreWindow));
            }));
        }

        IEnumerator Delay(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }
    }
}