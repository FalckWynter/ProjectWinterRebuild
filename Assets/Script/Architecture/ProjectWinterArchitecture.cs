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
        Debug.Log("静态初始化");
    }
    protected override void Init()
    {
        Debug.Log("初始化");
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
        //Debug.Log("进入延迟初始化");
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
        //Debug.Log("更新结构");
    }
}
