using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICopyAble<T>
{
    abstract T GetNewCopy();
}
public interface ICanBeEqualCompare<T>
{
    abstract bool IsEqualTo(T other);
}