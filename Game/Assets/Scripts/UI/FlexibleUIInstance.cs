using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class FlexibleUIInstance : Editor {

    /* menu option for button */
    [MenuItem("GameObject/Flexible UI/Button", priority = 0)]
    public static void AddButton()
    {
        Create("button");
    }

    /* menu option for text */
    [MenuItem("GameObject/Flexible UI/Text", priority = 0)]
    public static void AddText()
    {
        Create("Text");
    }

    static GameObject clickedObject;

    /* instantiate selected prefab */
    private static GameObject Create(string objectName)
    {
        GameObject instance = Instantiate(Resources.Load<GameObject>(objectName));
        instance.name = objectName;
        clickedObject = UnityEditor.Selection.activeObject as GameObject;
        if (clickedObject != null)
        {
                instance.transform.SetParent(clickedObject.transform, false);
        }
        return instance;
    }


}
#endif
