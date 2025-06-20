using QFramework;
using UnityEngine;

namespace CubeMaster
{
    public class AddScoreCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Add score");
            this.GetModel<RuntimeData>().Score.Value++;
        }
    }
}