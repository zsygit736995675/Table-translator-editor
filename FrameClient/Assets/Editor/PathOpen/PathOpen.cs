using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathOpen  {
    
    [MenuItem("Tools/OpenDirectory/dataPath")]
    static void OpendataPath()
    {
        System.Diagnostics.Process.Start(Application.dataPath);
    }

    [MenuItem("Tools/OpenDirectory/persistentDataPath")]
    static void OpenpersistentDataPath()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }

    [MenuItem("Tools/OpenDirectory/streamingAssetsPath")]
    static void OpenstreamingAssetsPath()
    {
        System.Diagnostics.Process.Start(Application.streamingAssetsPath);
    }

    [MenuItem("Tools/OpenDirectory/temporaryCachePath")]
    static void OpentemporaryCachePath()
    {
        System.Diagnostics.Process.Start(Application.temporaryCachePath);
    }

}
