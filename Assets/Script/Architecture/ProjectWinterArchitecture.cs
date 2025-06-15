using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class ProjectWinterArchitecture : Architecture<ProjectWinterArchitecture>
{
    public static ProjectWinterArchitecture architecture;
    GameSystem gameSystem;
    //public UIStateSystem uiSystem;
    //public LevelSystem levelSystem;
    public ProjectWinterArchitecture()
    {
        architecture = this;
        Debug.Log("��̬��ʼ��");
    }
    protected override void Init()
    {
        Debug.Log("��ʼ��");
        architecture = this;
        this.RegisterSystem<UtilSystem>(new UtilSystem());
        this.RegisterSystem<GameSystem>(new GameSystem());
        this.RegisterModel<GameModel>(new GameModel());
        gameSystem = this.GetSystem<GameSystem>();

        gameSystem.LateInit();
        //this.RegisterModel<CachePoolModel>(new CachePoolModel());
        //this.RegisterModel<LevelModel>(new LevelModel());
        //this.RegisterModel<UIStateModel>(new UIStateModel());
        //this.RegisterSystem<CachePoolSystem>(new CachePoolSystem());
        //this.RegisterSystem<LevelSystem>(new LevelSystem());
        //this.RegisterSystem<UIStateSystem>(new UIStateSystem());
        //uiSystem = this.GetSystem<UIStateSystem>();
        //levelSystem = this.GetSystem<LevelSystem>();
        //Debug.Log("�����ӳٳ�ʼ��");
        //this.GetSystem<CachePoolSystem>().LateInit();
        //this.GetSystem<LevelSystem>().LateInit();

        //this.GetSystem<CachePoolSystem>().LoadNextSituation();
        //levelModel = this.GetModel<LevelModel>();
    }
    //LevelModel levelModel;
    public void Update()
    {
        //uiSystem.Update();
        //levelSystem.Update();
        // levelModel.currentRecipe.CheckRecipe();
        //Debug.Log("���½ṹ");
    }
}
