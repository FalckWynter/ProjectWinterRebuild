using PlentyFishFramework;
using QFramework;
using QFramework.PointGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class GameCore : MonoBehaviour, IController
    {
        // ��Ϸ���Ľű�
        UtilSystem utilSystem;
        GameModel gameModel;
        ProjectWinterArchitecture architecture;
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }

        // Start is called before the first frame update
        void Start()
        {
            architecture = (ProjectWinterArchitecture)this.GetArchitecture();
            utilSystem = this.GetSystem<UtilSystem>();
            GameObject ob = utilSystem.CreateCardGameObject(CardDataBase.TryGetCard("DefaultCard"));
            //this.GetSystem<GameSystem>().MoveCardToClosestNullGrid(ob.GetComponent<ITableElement>(), null);
            gameModel = this.GetModel<PlentyFishFramework.GameModel>();

        }

        // Update is called once per frame
        // ���¿������
        void Update()
        {
            architecture.PreUpdate();
            architecture.Update();
        }
        public void LateUpdate()
        {
            architecture.LateUpdate();
            //if (gameModel.dragMonoList.Count > 0)
            //{
            //    StartCoroutine(ClearDragListsAtFrameEnd());
            //}

        }
        private IEnumerator ClearDragListsAtFrameEnd()
        {
            yield return new WaitForEndOfFrame();

            gameModel.dragMonoList.Clear(); // ��մ�����б�ļ�¼
        }
    }
}