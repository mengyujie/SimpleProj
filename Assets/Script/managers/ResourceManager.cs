using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using constDefine;

public class ResourceManager : MonoBehaviour {
    private string m_uiPath = "uipanel/{0}/{1}.prefab";
    private string m_modelPath = "model/";
    private string m_animPath = "anim/";
    private string m_picPath = "pic/";

    private Dictionary<string, GameObject> m_panelDic = new Dictionary<string, GameObject>();
    private Transform m_uiCanvas = null;
    public Transform uiCanvas
    {
        get
        {
            if (m_uiCanvas == null)
            {
                m_uiCanvas = transform.Find("UI/Canvas");
            }
            return m_uiCanvas;
        }
    }
    // Use this for initialization
    void Awake()
    {

    }
    void Start () {
		
	}

    //*******************************ui*********************************************
    public GameObject ShowPanel(string panelName,ShowType showtype)
    {
        GameObject go;
        if (m_panelDic.ContainsKey(panelName))
        {
            go = m_panelDic[panelName];
            go.SetActive(true);
            return go;
        }
        go= Resources.Load(string.Format(m_uiPath, panelName))as GameObject ;
        if (go == null)
        {
            Debug.LogError(string.Format("{0} not found!!!",panelName));
            return null;
        }
        m_panelDic.Add(panelName,go);
        SetParentByShowtype(go, showtype);
        return go;
    }
    public void RemovePanel(string panelName)
    {
        GameObject go;
        if (m_panelDic.TryGetValue(panelName,out go))
        {
            DestroyImmediate(go);
            m_panelDic.Remove(panelName);
        }
    }
    public void HidePanel(string panelName)
    {
        GameObject go;
        if (m_panelDic.TryGetValue(panelName, out go))
        {
            go.SetActive(false);
        }
    }
    private void SetParentByShowtype(GameObject go, ShowType showType)
    {
        Transform parent = null;
        switch (showType)
        {
            case ShowType.DOWN:
                parent = uiCanvas.Find("downLayer");
                break;
            case ShowType.MIDDLE:
                parent = uiCanvas.Find("middleLayer");
                break;
            case ShowType.TOP:
                parent = uiCanvas.Find("topLayer");
                break;
            case ShowType.LOADING:
                parent = uiCanvas.Find("loadingLayer");
                break;
        }
        if (parent != null)
        {
            go.transform.SetParent(parent);
        }
    }
    //*******************************ui end******************************************
}
