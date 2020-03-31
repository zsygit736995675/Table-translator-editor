using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例基类
/// </summary>
public class BaseClass<T> where T : class, new()
{
    private static T _instance;

    private static readonly object syslock = new object();

    public static T Ins
    {
        get
        {
            if (_instance == null)
            {
                //lock关键字用来确保代码块完成运行，避免多线程操作的时候出现问题
                lock (syslock)
                {

                    if (_instance == null)
                    {
                        _instance = new T();
                        
                    }
                }
            }
            return _instance;
        }
    }

    

}