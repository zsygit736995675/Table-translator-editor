using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 需要创建实体的单例对象，例如声音控制类等
/// </summary>

public class SingletonObject<T>  : MonoBehaviour where T:SingletonObject<T> {

    private static T _instance = null;

    private static readonly object syslock = new object();

    public static T Ins
    {
        get
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name,typeof(T));
                        _instance = obj.GetComponent<T>();
                         DontDestroyOnLoad(obj);
                        _instance.Spawn();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 初始化完成
    /// </summary>
    protected  virtual void Spawn()
    {

    }
	
}
