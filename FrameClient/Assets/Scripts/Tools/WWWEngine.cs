using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;


/// <summary>
/// 基于www封装，通讯下载
/// </summary>
public class WWWEngine : SingletonObject<WWWEngine>
{

    private string ImagePath = "/ImageCache/";
    private string AudioPath = "/AudioCache/";
    private static string ABundlePath = "/AbundleCache/";

    private static string suffix;

    private WWWEngine() { }

    /// <summary>
    /// 路径
    /// </summary>
    public string Path
    {
        get
        {
            return Application.persistentDataPath + "/WebCache";
        }
    }

    /// <summary>
    /// 初始化 下载路径
    /// </summary>
    protected override void Spawn()
    {
        if (!Directory.Exists(Path + ImagePath))
        {
            Directory.CreateDirectory(Path + ImagePath);
        }

        if (!Directory.Exists(Path + AudioPath))
        {
            Directory.CreateDirectory(Path + AudioPath);
        }
    }

    /// <summary>
    /// 下载音频
    /// </summary>
    /// <param name="url"></param>
    public static void LoadMusicByUrl(string url, AudioSource source)
    {
        string[] subArr = url.Split('.');
        suffix = "." + subArr[subArr.Length - 1];

        if (!string.IsNullOrEmpty(url))
        {
            WWWEngine.Ins.LoadAsyncWebMusic(url, source);
        }
        else
        {
             UnityEngine.Debug.LogError("GetMusicByUrl url is null!" + " url=" + url);
        }
    }

    private void LoadAsyncWebMusic(string url, AudioSource source)
    {
        //是否第一次加载
        if (!File.Exists(Path + AudioPath + url.GetHashCode() + suffix))
        {
            StartCoroutine(LoadWebMusic(url, source));
        }
        else
        {
            StartCoroutine(LoadLocalMusic(url, source));
        }
    }


    /// <summary>
    //  音效
    /// </summary>
    IEnumerator LoadLocalMusic(string url, AudioSource source)
    {
        string filePath = "file:///" + Path + AudioPath + url.GetHashCode() + suffix;
        if (url != "" || source != null)
        {
            WWW www = new WWW(filePath);
            yield return www;

            if (www.error == null)
            {
                AudioClip clip = www.GetAudioClip();
                if (clip != null)
                {
                    source.clip = clip;
                    source.Play();
                }
                else
                {
                     UnityEngine.Debug.LogError("LoadLocalMusic:" + "clip null " + " url=" + url);
                }
            }
            else
            {
                 UnityEngine.Debug.LogError("LoadLocalMusic:" + www.error + " url=" + url);
            }
        }
    }

    IEnumerator LoadWebMusic(string url, AudioSource source)
    {
        if (url != "")
        {
            WWW www = new WWW(url);
            yield return www;
    
            if (www.error == null)
            {
                try
                {
                    AudioClip clip = www.GetAudioClip();
                    if (clip != null)
                    {
                        File.WriteAllBytes(Path + AudioPath + url.GetHashCode() + suffix, www.bytes);
                        source.clip = clip;
                        source.Play();
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("LoadWebMusic:clip null");
                    }
                }
                catch (System.Exception error)
                {
                     UnityEngine.Debug.LogError("LoadWebMusic:" + error + " url=" + url);
                    throw error;
                }
            }
            else
            {
                 UnityEngine.Debug.LogError("LoadWebMusic:" + www.error + " url=" + url);
            }
        }
    }

    /// <summary>
    /// url
    /// </summary>
    public static void SetGameAsyncImagebyUrl(Image image, string url)
    {
        if (url != null)
        {
            WWWEngine.Ins.SetAsyncImage(url, image);
        }
        else
        {
             UnityEngine.Debug.LogError("SetAsyncImage url null");
        }
    }

    /// <summary>
    /// 静态设置
    /// </summary>
    public static void SetGameAsyncImage(Image image, string imagename)
    {
        string url = imagename;
        if (url != null)
        {
            WWWEngine.Ins.SetAsyncImage(url, image);
        }
        else
        {
             UnityEngine.Debug.LogError("SetAsyncImage url null");
        }
    }

    /// <summary>
    /// 异步设置图片
    /// </summary>
    private void SetAsyncImage(string url, Image image)
    {
        ////是否第一次加载
        if (!File.Exists(Path + ImagePath + url.GetHashCode()))
        {
            StartCoroutine(LoadWebImage(url, image));
        }
        else
        {
            StartCoroutine(LoadLocalImage(url, image));
        }
    }

    /// <summary>
    /// 加载网图
    /// </summary>
    IEnumerator LoadWebImage(string url, Image image)
    {
        if (url != "" || image != null)
        {
//             UnityEngine.Debug.Log(url);
            WWW www = new WWW(url);
            yield return www;

            if (string.IsNullOrEmpty( www.error))
            {
                try
                {
                    Texture2D tex2d = www.texture;
                    if (tex2d != null)
                    {
                        //保存图片
                        byte[] pngData = tex2d.EncodeToPNG();
                        File.WriteAllBytes(Path + ImagePath + url.GetHashCode(), pngData);
                        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.zero);
                        image.sprite = m_sprite;
                    }
                    else
                    {
                         UnityEngine.Debug.LogError("LoadWebImage Texture2D null" + " url=" + url);
                    }
                }
                catch (System.Exception error)
                {
                      UnityEngine.Debug.LogError("LoadWebImage exception:"+error + " url=" + url);
                }
            }
            else
            {
                UnityEngine.Debug.LogError("LoadWebImage error:"+www.error + " url="+ url);
            }
        }
    }


    /// <summary>
    /// 从本地加载图片
    /// </summary>
    IEnumerator LoadLocalImage(string url, Image image)
    {
        string filePath = "file:///" + Path + ImagePath + url.GetHashCode();
        if (url != "" || image != null)
        {
            WWW www = new WWW(filePath);
            yield return www;

            if (string .IsNullOrEmpty( www.error) )
            {
                Texture2D texture = www.texture;
                if (texture != null)
                {
                    Sprite m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                    image.sprite = m_sprite;
                }
                else
                {
                    UnityEngine.Debug.LogError("LoadLocalImage Texture2D null" + " url=" + url);
                }
            }
            else
            {
                UnityEngine.Debug.LogError("LoadLocalImage error:"+www.error + " url=" + url);
            }
        }
    }


    public static IEnumerator LoadModel(string url, string name, Action<WWW> process)
    {
        yield return WWWEngine.Ins.DownFile(url, name, process);
    }


    static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
    /// <summary>
    /// 下载文件
    /// </summary>
    public IEnumerator DownFile(string url, string savePath, Action<WWW> process)
    {
        savePath = Path + ABundlePath + savePath;
        FileInfo file = new FileInfo(savePath);
        stopWatch.Start();
        UnityEngine.Debug.Log("Start:" + Time.realtimeSinceStartup);
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return 0;
            if (process != null)
                process(www);
        }
        yield return www;
        if (www.isDone)
        {
            byte[] bytes = www.bytes;
            CreatFile(savePath, bytes, www);
            if (process != null)
                process(www);
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="bytes"></param>
    public void CreatFile(string savePath, byte[] bytes, WWW www)
    {
        FileStream fs = new FileStream(savePath, FileMode.Append);
        BinaryWriter bw = new BinaryWriter(fs);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();     //流会缓冲，此行代码指示流不要缓冲数据，立即写入到文件。
        fs.Close();     //关闭流并释放所有资源，同时将缓冲区的没有写入的数据，写入然后再关闭。
        fs.Dispose();   //释放流
        www.Dispose();

        stopWatch.Stop();
        UnityEngine.Debug.Log("下载完成,耗时:" + stopWatch.ElapsedMilliseconds);
        UnityEngine.Debug.Log("End:" + Time.realtimeSinceStartup);
    }

    /// <summary>
    /// 加载文本
    /// </summary>
    public void LoadTextStr(string url, Action<string> action)
    {
        StartCoroutine(WWWEngine.Ins.LoadText(url, action));
    }


    /// <summary>
    /// 加载文本
    /// </summary>
    IEnumerator LoadText(string jsonfile,Action<string> action)
    {
        WWW www = new WWW(jsonfile);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            string str = www.text;
            if (action != null)
            {
                action(str);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("LoadText error: file"+jsonfile +"  error"+www.text);
        }
    }




}
