using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using PlentyFishFramework;
namespace PlentyFishFramework
{
    public class ProjectWinterArchitecture : Architecture<ProjectWinterArchitecture>
    {
        public static ProjectWinterArchitecture architecture;
        GameSystem gameSystem;
        RecipeSystem recipeSystem;
        //public UIStateSystem uiSystem;
        //public LevelSystem levelSystem;
        public ProjectWinterArchitecture()
        {
            architecture = this;
            Debug.Log("静态初始化");
        }
        protected override void Init()
        {
            // 注册系统和数据模型
            Debug.Log("初始化");
            architecture = this;
            this.RegisterModel<GameModel>(new GameModel());
            this.RegisterModel<RecipeModel>(new RecipeModel());
            this.RegisterSystem<UtilSystem>(new UtilSystem());
            this.RegisterSystem<GameSystem>(new GameSystem());
            this.RegisterSystem<RecipeSystem>(new RecipeSystem());

            recipeSystem = this.GetSystem<RecipeSystem>();
            gameSystem = this.GetSystem<GameSystem>();

            gameSystem.LateInit();

            gameSystem.InitTable();
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
        public void PreUpdate()
        {
            recipeSystem.PreUpdate();
        }
        public void Update()
        {
            //uiSystem.Update();
            //levelSystem.Update();
            // levelModel.currentRecipe.CheckRecipe();
            //Debug.Log("更新结构");
        }
        public void LateUpdate()
        {
            gameSystem.LateUpdate();
        }
    }
}