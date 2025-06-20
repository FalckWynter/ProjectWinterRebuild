using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace FlappyBird
{
    public class Level : BaseController
    {
        private const float cameraOrthoSize = 50f;
        private const float pipeWidth = 7.8f;
        private const float pipeHeadHeight = 3.75f;
        private const float pipeMoveSpeed = 30f;
        private const float pipeDestroyXPosition = -100f;
        private const float pipeSpawnXPosition = +100f;
        private const float groundDestroyXPosition = -200f;
        private const float cloudDestroyXPosition = -160f;
        private const float cloudSpawnXPosition = +160f;
        private const float cloudSpawnYPosition = +30f;
        private const float birdXPosition = 0f;
        private const float groundWidth = 192f;

        private List<Transform> groundList;
        private List<Transform> cloudList;
        private float cloudSpawnTimer;
        private List<Pipe> pipeList;
        private int pipesSpawned;
        private float pipeSpawnTimer;
        private float pipeSpawnTimerMax;
        private float gapSize;

        private Transform pipeHead;
        private Transform pipeBody;
        private Transform ground;
        private Transform cloud1;
        private Transform cloud2;
        private Transform cloud3;

        private GameRuntimeModel.State state;
        
        private enum Difficulty
        {
            Easy,
            Medium,
            Hard,
            Impossible,
        }

        private void Awake()
        {
            pipeList = new List<Pipe>();
            pipeSpawnTimerMax = 1f;
            this.GetModel<GameRuntimeModel>().GameState.Register(UpdateState);
        }

        private void Start()
        {
            pipeHead = Resources.Load<Transform>("Prefabs/pfPipeHead");
            pipeBody = Resources.Load<Transform>("Prefabs/pfPipeBody");
            ground = Resources.Load<Transform>("Prefabs/pfGround");
            cloud1 = Resources.Load<Transform>("Prefabs/pfClouds_1");
            cloud2 = Resources.Load<Transform>("Prefabs/pfClouds_2");
            cloud3 = Resources.Load<Transform>("Prefabs/pfClouds_3");
        }

        private void InitMap()
        {
            SpawnInitialGround();
            SpawnInitialClouds();
            SetDifficulty(Difficulty.Easy);
        }
        
        private void UpdateState(GameRuntimeModel.State state)
        {
            this.state = state;
            switch (state)
            {
                case GameRuntimeModel.State.WaitingToStart:
                    foreach (var trans in cloudList)
                    {
                        Destroy(trans.gameObject);
                    }
                    cloudList.Clear();
                    foreach (var trans in groundList)
                    {
                        Destroy(trans.gameObject);
                    }
                    groundList.Clear();
                    foreach (var trans in pipeList)
                    {
                        trans.DestroySelf();
                    }
                    pipeList.Clear();
                    break;
                case GameRuntimeModel.State.Playing:
                    InitMap();
                    break;
                case GameRuntimeModel.State.BirdDead:
                    break;
            }
        }

        private void Update()
        {
            if(state != GameRuntimeModel.State.Playing) return;
            HandlePipeMovement();
            HandlePipeSpawning();
            HandleGround();
            HandleClouds();
        }

        private void SpawnInitialClouds()
        {
            cloudList = new List<Transform>();
            var cloudTransform = Instantiate(GetCloudPrefabTransform(), new Vector3(0, cloudSpawnYPosition, 0),
                Quaternion.identity);
            cloudList.Add(cloudTransform);
        }

        private Transform GetCloudPrefabTransform()
        {
            switch (Random.Range(0, 3))
            {
                default:
                case 0: return cloud1;
                case 1: return cloud2;
                case 2: return cloud3;
            }
        }

        private void HandleClouds()
        {
            // 云的生成
            cloudSpawnTimer -= Time.deltaTime;
            if (cloudSpawnTimer < 0)
            {
                float cloudSpawnTimerMax = 6f;
                cloudSpawnTimer = cloudSpawnTimerMax;
                Transform cloudTransform = Instantiate(GetCloudPrefabTransform(),
                    new Vector3(cloudSpawnXPosition, cloudSpawnYPosition, 0), Quaternion.identity);
                cloudList.Add(cloudTransform);
            }

            // 云的移动
            for (int i = 0; i < cloudList.Count; i++)
            {
                Transform cloudTransform = cloudList[i];
                cloudTransform.position += new Vector3(-1, 0, 0) * pipeMoveSpeed * Time.deltaTime * .7f;

                if (cloudTransform.position.x < cloudDestroyXPosition)
                {
                    // 移动超过屏幕就销毁
                    Destroy(cloudTransform.gameObject);
                    cloudList.RemoveAt(i);
                    i--;
                }
            }
        }

        private void SpawnInitialGround()
        {
            groundList = new List<Transform>();
            float groundY = -47.5f;
            float groundWidth = 192f;
            var groundTransform = Instantiate(ground, new Vector3(0, groundY, 0),
                Quaternion.identity);
            groundList.Add(groundTransform);
            groundTransform = Instantiate(ground, new Vector3(groundWidth, groundY, 0),
                Quaternion.identity);
            groundList.Add(groundTransform);
            groundTransform = Instantiate(ground, new Vector3(groundWidth * 2f, groundY, 0),
                Quaternion.identity);
            groundList.Add(groundTransform);
        }

        private void HandleGround()
        {
            foreach (Transform groundTransform in groundList)
            {
                groundTransform.position += new Vector3(-1, 0, 0) * pipeMoveSpeed * Time.deltaTime;

                if (groundTransform.position.x < groundDestroyXPosition)
                {
                    // 地面移动到最左边，就重新放到右边的位置
                    float rightMostXPosition = -100f;
                    for (int i = 0; i < groundList.Count; i++)
                    {
                        if (groundList[i].position.x > rightMostXPosition)
                        {
                            rightMostXPosition = groundList[i].position.x;
                        }
                    }
                    
                    var position = groundTransform.position;
                    position = new Vector3(rightMostXPosition + groundWidth, position.y,
                        position.z);
                    groundTransform.position = position;
                }
            }
        }

        private void HandlePipeSpawning()
        {
            pipeSpawnTimer -= Time.deltaTime;
            if (pipeSpawnTimer < 0)
            {
                // Time to spawn another Pipe
                pipeSpawnTimer += pipeSpawnTimerMax;

                float heightEdgeLimit = 10f;
                float minHeight = gapSize * .5f + heightEdgeLimit;
                float totalHeight = cameraOrthoSize * 2f;
                float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

                float height = Random.Range(minHeight, maxHeight);
                CreateGapPipes(height, gapSize, pipeSpawnXPosition);
            }
        }

        private void HandlePipeMovement()
        {
            for (int i = 0; i < pipeList.Count; i++)
            {
                Pipe pipe = pipeList[i];

                bool isToTheRightOfBird = pipe.GetXPosition() > birdXPosition;
                pipe.Move();
                if (isToTheRightOfBird && pipe.GetXPosition() <= birdXPosition && pipe.IsBottom())
                {
                    // 小鸟过去了
                    this.SendCommand<GainScoreCommand>();
                    this.GetSystem<AudioSystem>().PlaySingleSound("Sounds/Score");
                }

                if (pipe.GetXPosition() < pipeDestroyXPosition)
                {
                    pipe.DestroySelf();
                    pipeList.Remove(pipe);
                    i--;
                }
            }
        }

        private void SetDifficulty(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    gapSize = 50f;
                    pipeSpawnTimerMax = 1.4f;
                    break;
                case Difficulty.Medium:
                    gapSize = 40f;
                    pipeSpawnTimerMax = 1.3f;
                    break;
                case Difficulty.Hard:
                    gapSize = 33f;
                    pipeSpawnTimerMax = 1.1f;
                    break;
                case Difficulty.Impossible:
                    gapSize = 24f;
                    pipeSpawnTimerMax = 1.0f;
                    break;
            }
        }

        private Difficulty GetDifficulty()
        {
            if (pipesSpawned >= 24) return Difficulty.Impossible;
            if (pipesSpawned >= 12) return Difficulty.Hard;
            if (pipesSpawned >= 5) return Difficulty.Medium;
            return Difficulty.Easy;
        }

        private void CreateGapPipes(float gapY, float gapSize, float xPosition)
        {
            CreatePipe(gapY - gapSize * .5f, xPosition, true);
            CreatePipe(cameraOrthoSize * 2f - gapY - gapSize * .5f, xPosition, false);
            pipesSpawned++;
            SetDifficulty(GetDifficulty());
        }

        private void CreatePipe(float height, float xPosition, bool createBottom)
        {
            Transform head = Instantiate(this.pipeHead);
            float pipeHeadYPosition;
            if (createBottom)
            {
                pipeHeadYPosition = -cameraOrthoSize + height - pipeHeadHeight * .5f;
            }
            else
            {
                pipeHeadYPosition = +cameraOrthoSize - height + pipeHeadHeight * .5f;
            }
            head.position = new Vector3(xPosition, pipeHeadYPosition);
            
            Transform body = Instantiate(this.pipeBody);
            float pipeBodyYPosition;
            if (createBottom)
            {
                pipeBodyYPosition = -cameraOrthoSize;
            }
            else
            {
                pipeBodyYPosition = +cameraOrthoSize;
                body.localScale = new Vector3(1, -1, 1);
            }
            body.position = new Vector3(xPosition, pipeBodyYPosition);

            SpriteRenderer pipeBodySpriteRenderer = body.GetComponent<SpriteRenderer>();
            pipeBodySpriteRenderer.size = new Vector2(pipeWidth, height);

            BoxCollider2D pipeBodyBoxCollider = body.GetComponent<BoxCollider2D>();
            pipeBodyBoxCollider.size = new Vector2(pipeWidth, height);
            pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);

            Pipe pipe = new Pipe(head, body, createBottom);
            pipeList.Add(pipe);
        }
        
        /*
         * Represents a single Pipe
         * */
        private class Pipe
        {

            private Transform pipeHeadTransform;
            private Transform pipeBodyTransform;
            private bool isBottom;

            public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom)
            {
                this.pipeHeadTransform = pipeHeadTransform;
                this.pipeBodyTransform = pipeBodyTransform;
                this.isBottom = isBottom;
            }

            public void Move()
            {
                pipeHeadTransform.position += new Vector3(-1, 0, 0) * pipeMoveSpeed * Time.deltaTime;
                pipeBodyTransform.position += new Vector3(-1, 0, 0) * pipeMoveSpeed * Time.deltaTime;
            }

            public float GetXPosition()
            {
                return pipeHeadTransform.position.x;
            }

            public bool IsBottom()
            {
                return isBottom;
            }

            public void DestroySelf()
            {
                Destroy(pipeHeadTransform.gameObject);
                Destroy(pipeBodyTransform.gameObject);
            }
        }
    }
}