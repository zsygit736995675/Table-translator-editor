using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 路径工具
/// </summary>
public class PathUtils
{

    public const string bundleFilePathRoot = "/res/buildRes/";//打包资源根路径
    public const string AssetBundlesOutPutPath = "Assets/StreamingAssets";//Ab打包保存跟目录
    public static string tablePath = "/exp/exp_bytes/";//表格路径
    public static string persiPath = Application.persistentDataPath;//缓存地址，有可能会在回调中使用，所以先保存一下，只能在主线程使用
    

    public const string localIconPath = "Texture/Icon/";//头像本地路径
    public const string localTopicPath = "Texture/Topic/";//题目图片本地地址
    public const string localMedalIcon = "Texture/medal/";//勋章图标
    public const string localAvaterBox = "Texture/IconBox/";//头像框
    public const string localRoom = "Texture/room/";//房间图标


    public const string Progressbar = "Texture/Progressbar/";//进度条
    public const string How = "Texture/UIHow/";//简介
    public const string Line = "Texture/Line/";//线
    public const string Hook = "Texture/Hook/";//钩子

    public const string localPropIcon = "Texture/PropIcon/";//道具icon本地地址

    public const string obstaclePre = "Prefabs/Obstacle/";//板子预制体位置
    public const string uiPre = "Prefabs/UI/";//ui预制体
    public const string uiModPre = "Prefabs/UI/Normal/";//ui预制体
    public const string modelPre = "Prefabs/Model/";//模型预制体
    public const string propPre = "Prefabs/Prop/";//道具预制体
    public const string levelPre = "Prefabs/Level/";//关卡预制体
    public const string skillPre = "Prefabs/Skill/";//技能预制体
    public const string SmearPre = "Prefabs/Smear/";//拖尾

    public const string commonEffPre = "Prefabs/Effect/Common/";//通用特效位置
    public const string skillEffPre = "Prefabs/Effect/Skill/";//技能特效
    public const string sceneEffPre = "Prefabs/Effect/Scene/";//场景特效
    public const string buffIconPre = "Prefabs/Effect/BuffIcon/";//特效图标
    public const string effectPre = "Prefabs/Effect/";//特效


    public const string screenPre = "Screen/";//场景预制体

    public const string material = "Material/";//材质球


    /// <summary>
    /// 版本管理文件url
    /// </summary>
    public static readonly string versionUrl = "http://u3d-model.oss-cn-beijing.aliyuncs.com/model/Res/Ver3/z_version.txt";

    /// <summary>
    /// 获取资源保存路径，打包时保存路径
    /// </summary>
    public static string GetAssetOutPath(int target)
    {
        string path = "";
        switch (target)
        {
            case 5://win
                path = AssetBundlesOutPutPath + "/Android/";
                break;

            case 9://ios
                path = AssetBundlesOutPutPath + "/IOS/";
                break;

            case 13://android
                path = AssetBundlesOutPutPath + "/Android/";
                break;
        }
        return path;
    }

    /// <summary>
    /// 获取平台名称，打包时用，传平台枚举值
    /// </summary>
    public static string GetPlatformName(int target)
    {
        string name = "";
        switch (target)
        {
            case 19://win
                name = "Android";
                break;

            case 9://ios
                name = "IOS";
                break;

            case 13://android
                name = "Android";
                break;
        }
        return name;
    }


    /// <summary>
    /// 不同平台下StreamingAssets的路径
    /// </summary>
    public static readonly string dataPath =
#if UNITY_ANDROID && !UNITY_EDITOR
		"jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IOS && !UNITY_EDITOR
		"file://" + Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN
        "file://" + Application.dataPath + "/StreamingAssets/";
#else
        "file://" + Application.dataPath + "/StreamingAssets/";
#endif

    /// <summary>
    /// assetbundle包本地存放位置
    /// </summary>
    public static string ABPath
    {
        get
        {
            string path = persiPath + "/WebCache/ResCache/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }

    /// <summary>
    /// json保存地址
    /// </summary>
    public static string JsonSavePath
    {
        get
        {
            string path = Application.persistentDataPath + "/WebCache/Json/";

            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }

    /// <summary>
    /// 读取的时候要加file
    /// </summary>
    public static string JsonReadPath
    {
        get
        {
            return  "file:///" +  JsonSavePath;
        }
    }



    /// <summary>
    /// assetbundle包本地存放位置
    /// </summary>
    public static string LoadABPath
    {
        get
        {
            string path = persiPath + "/WebCache/ResCache/";
            path = "file:///" + path;
            return path;
        }
    }

    /// <summary>
    /// 网络图片保存目录
    /// </summary>
    public static string WebImageSavePath
    {
        get
        {
            string path = persiPath + "/WebCache/ImageCache/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }

    /// <summary>
    /// 平台名称，下载时只区分安卓和ios，pc使用安卓的资源
    /// </summary>
    public static readonly string platName =
#if UNITY_IOS 
		 "IOS";
#else
       "Android";
#endif


}
