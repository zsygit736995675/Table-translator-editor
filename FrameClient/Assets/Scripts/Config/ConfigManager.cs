using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System;

public class ConfigManager : BaseClass<ConfigManager>
{

    public bool userLocalConfig = false;

    public int loadCount = 0;

    public Action LoadCallback;

    public List<string> jsons = new List<string>();

    public void Init(Action action)
    {
        LoadCallback = action;

        //jsons.Add(StrConfig.configName);
        //jsons.Add(ClearanceConfig.configName);
       // jsons.Add(CharacterConfig.configName);
        //jsons.Add(LevelConfig.configName);
        //jsons.Add(SceneConfig.configName);
        //jsons.Add(RewardConfig.configName);
        //jsons.Add(SmearConfig.configName);
        //jsons.Add(SystemConfig.configName);
        //jsons.Add(GuideConfig.configName);
        //jsons.Add(TurnTableConfig.configName);


        LoadJson(() => {
            //StrConfig.Init();
            //ClearanceConfig.Init();
            //CharacterConfig.Init();
            //LevelConfig.Init();
            //SceneConfig.Init();
            //RewardConfig.Init();
            //SmearConfig.Init();
            //SystemConfig.Init();
            //GuideConfig.Init();
            //TurnTableConfig.Init();

        });
    }



    /// <summary>
    /// 更新配置表
    /// </summary>
    /// <param name="action"></param>
    public void LoadJson(Action action)
    {
        //if (userLocalConfig)
        //{
        //    if (action != null)
        //    {
        //        action();
        //        return;
        //    }
        //}
        if (jsons.Count > 0)
        {
            string fileName = jsons[0];
            string localVer = PlayerPrefs.GetString("brother_" + fileName);//获取本地版本号
            string jsonfile = PathUtils.JsonSavePath + fileName + ".json";//JSON文件路径
            string version = "";// GetVersion(fileName);

            if (!File.Exists(jsonfile))
            {
                string resName = fileName;
                if (string.IsNullOrEmpty(resName))
                {
                    jsons.RemoveAt(0);
                    LoadJson(action);
                    return;
                }
                ReadjsonFromLoacl(fileName, (json) =>
                {
                    string[] jsondata = new string[1];
                    jsondata[0] = json;
                    File.WriteAllLines(jsonfile, jsondata);
                }, true);

                CheckVersion(fileName, version, action, jsonfile, true);
            }
            else
            {
                CheckVersion(fileName, version, action, jsonfile, false);
            }

        }
        else
        {
            action();
        }
    }


    public void CheckVersion(string fileName, string version, Action action, string jsonfile, bool isLocal)
    {

        //ReadjsonFromLoacl(fileName, (jsonstr) => {
        //    JObject jobj = (JObject)JsonConvert.DeserializeObject(jsonstr);
        //    if (jobj["version"].ToString() != version)
        //    {
        //        HTTP.Ins.HttpAsyncGet(HTTPConst.getres + fileName, (str) =>
        //        {
        //            //Logger.LogUI("HttpAsyncGet:" + fileName);
        //            Debug.Log(fileName + "::" + str);

        //            JObject json = (JObject)JsonConvert.DeserializeObject(str);
        //            string _code = json["code"].ToString();
        //            string _msg = json["msg"].ToString();
        //            string _data = json["data"].ToString();

        //            if (_code == "0")
        //            {
        //                Action act1 = () => { LoadJson(action); };
        //                EventMgrHelper.Ins.PushEventEx(EventDef.Callback, act1);
        //                return;
        //            }

        //            if (File.Exists(jsonfile))
        //            {
        //                File.Delete(jsonfile);
        //            }

        //            string[] info = new string[1];
        //            info[0] = _data;
        //            File.WriteAllLines(jsonfile, info);

        //            Action act = () => { jsons.RemoveAt(0); LoadJson(action); };
        //            EventMgrHelper.Ins.PushEventEx(EventDef.Callback, act);

        //        });
        //    }
        //    else
        //    {
        //        jsons.RemoveAt(0);
        //        LoadJson(action);
        //    }
        //}, isLocal);
    }

    /// <summary>   
    /// 读取本地JSON文件
    /// </summary>
    /// <param name="key">JSON文件中的key值</param>
    /// <returns>JSON文件中的value值</returns>
    public void ReadjsonFromLoacl(string fileName, Action<string> action, bool local = true)
    {
        if (local)
        {
            string jsonfile = PathUtils.dataPath + "Json/" + fileName + ".json";//JSON文件路径
            WWWLoadLocal(jsonfile, action);
        }
        else
        {
            string url = PathUtils.JsonReadPath + fileName + ".json";//JSON文件路径
            Debug.Log(url);
            WWWLoadLocal(url, action);
        }
    }

    /// <summary>   
    /// 读取JSON文件
    /// </summary>
    /// <param name="key">JSON文件中的key值</param>
    /// <returns>JSON文件中的value值</returns>
    public void Readjson(string fileName, Action<string> action, bool local = false)
    {
        loadCount++;

        if (local)
        {
            string jsonfile = PathUtils.dataPath + "Json/" + fileName + ".json";//JSON文件路径
            WWWLoad(jsonfile, action);
        }
        else
        {
            if (userLocalConfig)
            {
                string jsonfile = PathUtils.dataPath + "Json/" + fileName + ".json";//JSON文件路径
                WWWLoad(jsonfile, action);
            }
            else
            {
                string url = PathUtils.JsonReadPath + fileName + ".json";//JSON文件路径
                WWWLoad(url, action);
            }
        }
    }

    public void WWWLoad(string path, Action<string> action)
    {

        WWWEngine.Ins.LoadTextStr(path, (str) =>
        {
            if (action != null)
            {
                action(str);
            }

            loadCount--;

            string[] strs = path.Split('/');

            if (loadCount <= 0)
            {
                if (LoadCallback != null)
                    LoadCallback();
            }
        });
    }

    public void WWWLoadLocal(string path, Action<string> action)
    {

        WWWEngine.Ins.LoadTextStr(path, (str) =>
        {
            if (action != null)
            {
                action(str);
            }

        });
    }


    public string GetResName(string conName)
    {
        switch (conName)
        {
            case "GuiConfig":
                return "";
            case "QuestionConfig":
                return "questions";
            case "StrConfig":
                return "localinfo";
            case "AchievementConfig":
                return "achievement";
            case "PropConfig":
                return "prop";
            case "TaskConfig":
                return "mission";
            case "RankingConfig":
                return "ranking";
            case "AbilityConfig":
                return "ability";
        }
        return "";
    }

   

}
