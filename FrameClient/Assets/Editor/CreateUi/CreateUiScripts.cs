using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using MenuItem = UnityEditor.MenuItem;

/* 注意事项：  UI命名规则
   自动生成ui脚本
   2019.03.14
   by zsy
 */
public class CreateUiScripts : Editor
{

    /// <summary>
    /// 存放类型标识符，ui前缀需按照规则设定
    /// </summary>
    public static Dictionary<string, string> typMap = new Dictionary<string, string>()
    {
        {"btn",typeof(Button).Name },
        {"txt",typeof(Text).Name },
        {"img",typeof(Image).Name },
        {"trs",typeof(Transform).Name},
        {"scr",typeof(ScrollRect).Name},
        {"inp",typeof(InputField).Name},
        {"ani",typeof(Animator).Name},
        {"rec",typeof(RectTransform).Name},
        {"sli",typeof(Slider).Name},
        {"tog",typeof(Toggle).Name},
        {"raw",typeof(RawImage).Name},
    };

    //每个info等于一个组件
    private static List<UIInfo> uinfo = new List<UIInfo>();

    private static string Eg_str =
        "using System.Collections;\r" +
        "using System.Collections.Generic;\r" +
        "using UnityEngine;\r" +
        "using UnityEngine.UI;\r" +
        "using System;\r" +



        "public class @Name : BaseEuiView {\r\r\t\t" +
        "@File1 \r\t" +
        "public @Name(BaseController controller, LayoutUI lay, ViewDef id) : base(controller, lay, id){}\r\r\t" +
        "public override void initUI () \r\t{\r\t\tbase.initUI();\r\r\t\t\t@Body1\r}\r\r\t" +
        "public override void initData () \r\t{\r\t\tbase.initData();\r\r\t}\r\r\t" +
        "public override void open (params object[] args)\r\t{\r\t\tbase.open(args);\r\r\t}\r\r\t" +
        "public override void close (params object[] args)\r\t{\r\t\tbase.close(args);\r\r\t}\r\r\r\r\r"

        + "}";

    //属性
    private static string File1 = "";
    private static string Body1 = "";
    private static string ClassName = "";

    /// <summary>
    /// 写入文档
    /// </summary>
    static void WriteScript()
    {
        for (int i = 0; i < uinfo.Count; i++)
        {
            File1 += uinfo[i].File1 + ";\r\t\t";
            Body1 += uinfo[i].Body1 + ";\r\t\t\t";
        }

        Eg_str = Eg_str.Replace("@File1", File1);
        Eg_str = Eg_str.Replace("@Body1", Body1);
        string nameStr = ClassName;
        Eg_str = Eg_str.Replace("@Name", nameStr);

        //储存文档
        SaveFileDialog saveFile = new SaveFileDialog();
        saveFile.FileName = ClassName + ".cs";

        string path = Environment.CurrentDirectory.Replace("/", @"\");
        if (saveFile.ShowDialog() == DialogResult.OK)
        {
            string[] name = saveFile.FileName.Split('\\');
            //  string nameStr = name[name.Length - 1].Replace(".cs", "");
            Eg_str = Eg_str.Replace("@Name", nameStr);
            File.WriteAllText(saveFile.FileName, Eg_str);
        }
    }

    /// <summary>
    /// 获取物体的路径
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    static string GetgameObjectPath(Transform go)
    {
        string path = "";
        while (go.name != Selection.gameObjects[0].name)
        {
            path = path.Insert(0, "/");
            path = path.Insert(0, go.name);
            go = go.parent;
        }
        return path;
    }

    /// <summary>
    /// 获取物体的基本信息(名字，校准key，路径)，并储存
    /// </summary>
    /// <param name="tf"></param>
    static void GetChildinfo(Transform tf)
    {
        foreach (Transform tfChild in tf)
        {
            if (tfChild.name.Length < 3)
            {
                continue;
            }
            string contrastKey = tfChild.name.Substring(0, 3);
            string valueName = "";
            if (typMap.TryGetValue(contrastKey, out valueName))
            {
                string parentName = "";
                if (tfChild.parent.name == ClassName)
                {
                    parentName = "Frame";
                }
                else
                {
                    parentName = tfChild.parent.name;
                }
                UIInfo uinf = new UIInfo(tfChild.name, valueName, GetgameObjectPath(tfChild), parentName);
                uinfo.Add(uinf);
            }
            if (tfChild.childCount >= 0)
            {
                GetChildinfo(tfChild);
            }
        }
    }

    /// <summary>
    /// 生成脚本
    /// </summary>
    [MenuItem("Tools/CreateScript", false, 12)]
    public static void CreateScript()
    {
        GameObject[] select = Selection.gameObjects;
        if (select.Length == 1)
        {
            Transform selectGo = select[0].transform;
            ClassName = selectGo.name;
            GetChildinfo(selectGo);
            WriteScript();
        }
        else
        {
            EditorUtility.DisplayDialog("警告", "你只能选择一个GameObject", "确定");
        }
    }


}

/// <summary>
/// ui脚本模板
/// </summary>
public class UIInfo
{
    private string file1;
    private string body1;
    private string body2;

    public string File1
    {
        get { return file1; }
        set { file1 = value; }
    }

    public string Body1
    {
        get { return body1; }
        set { body1 = value; }
    }

    public string Body2
    {
        get { return body2; }
        set { body2 = value; }
    }

    /// <param name="name">属性名称</param>
    /// <param name="contrastName">属性类型</param>
    /// <param name="path">子物体路径，因为直接通过路径查找耗时比较多，改为直接父物体查找</param>
    /// <param name="parentName">父物体</param>
    public UIInfo(string name, string contrastName, string path, string parentName)
    {
        file1 = string.Format("private {0} {1} = null", contrastName, name);
        body1 = string.Format("{0} = FindChild<{1}>({2}.transform,\"{3}\")", name, contrastName, parentName, name);
        body2 = string.Format("{0}=null", name);
    }
}