using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class GameModel : AbstractModel
{
    public List<ICanDragComponentMono> dragMonoList = new List<ICanDragComponentMono>();
    protected override void OnInit()
    {
    }
}
