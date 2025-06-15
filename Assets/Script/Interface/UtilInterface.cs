using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICopyAble<T>
{
    abstract T GetNewCopy();
}
