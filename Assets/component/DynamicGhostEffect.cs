using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGhostEffect : MonoBehaviour {
    public int storeAnimFrameNum=10;
    public int ghostCount = 1;
	private AnimFrameData[] animFrams;
	public int firstAnimFrameIndex= 0;
	private int lastAnimFrameIndex=0;
	private int totalAnimFrameNum=0;
    private SkinnedMeshRenderer[] meshRenderers;
    public Material shadowMat = null;
    private int shadowPlayIndex=0;
    private bool startPlayAnim = true;
    private List<GameObject> shadowGameObjectList = null;
	// Use this for initialization
	void Start () 
	{
        animFrams = new AnimFrameData[storeAnimFrameNum];
        lastAnimFrameIndex = storeAnimFrameNum - 1;
        meshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        shadowGameObjectList = new List<GameObject>();
        StartCoroutine(startShadow());
    }
	IEnumerator startShadow()
    {
        int frameLen = 8;
        for (int i = 0; i < ghostCount; i++)
        {
            for (int j = 0; j < frameLen; j++)
            {
                yield return new WaitForEndOfFrame();
            }
            StartPlayShadowAnim(i);
        }
    }
	// Update is called once per frame
	void Update () 
	{
		if(animFrams == null|| meshRenderers==null)
		{
			return;
		}
        StoreAnimFrame();
        for(int i = 0; i < ghostCount; i++)
        {
            PlayShadowAnim(i);
        }
        shadowPlayIndex = GetNextAnimFrameIndex(shadowPlayIndex);
    }
    public void PlayShadowAnim(int index)
    {
        if (!startPlayAnim)
        {
            return;
        }
        int shadowGOListCount = shadowGameObjectList.Count;
        if (index> shadowGOListCount - 1)
        {
            return;
        }
        GameObject startGameObject = shadowGameObjectList[index];
        if (startGameObject == null)
        {
            return;
        }
        AnimFrameData animFrameData = animFrams[GetNextAnimFrameIndex(shadowPlayIndex,index*5+1)];
        if (animFrameData == null)
        {
            return;
        }
        MeshFilter[] meshFilters = startGameObject.GetComponentsInChildren<MeshFilter>();
        int meshFilterCount = meshFilters.Length;
        for(int i = 0; i < meshFilterCount; i++)
        {
            animFrameData.SetAnimDataByIndex(i,meshFilters[i]);
        }
        
    }
    public void StartPlayShadowAnim(int index)
    {
        int shadowGOListLen = shadowGameObjectList.Count;
        while(shadowGOListLen <= index)
        {
            shadowGameObjectList.Add(null);
            shadowGOListLen = shadowGameObjectList.Count;
        }
        GameObject startGameObject = shadowGameObjectList[index];
        if (startGameObject != null)
        {
            return;
        }
        startPlayAnim = true;
        shadowPlayIndex = firstAnimFrameIndex;
        startGameObject = new GameObject();
        shadowGameObjectList[index]= startGameObject;
        int skinmeshRenderedCount = meshRenderers.Length;
        for(int i = 0; i < skinmeshRenderedCount; i++)
        {
            GameObject go = new GameObject();
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer meshRender = go.AddComponent<MeshRenderer>();
            meshRender.material = shadowMat;
            go.transform.SetParent(startGameObject.transform);
            go.transform.localScale = Vector3.one;
        }
    }
	void StoreAnimFrame()
	{
        int storeIndex = GetNextAnimFrameIndex(lastAnimFrameIndex);
        if (totalAnimFrameNum >= storeAnimFrameNum)
        {
            firstAnimFrameIndex = GetNextAnimFrameIndex(firstAnimFrameIndex);
        }
        else
        {
            totalAnimFrameNum++;
        }
        if (animFrams[storeIndex] == null)
        {
            animFrams[storeIndex] = new AnimFrameData(meshRenderers);
        }
        else
        {
            animFrams[storeIndex].StoreAnimMesh(meshRenderers);
        }
        lastAnimFrameIndex = storeIndex;
    }
    int GetNextAnimFrameIndex(int index,int addFrameNum=1)
    {
        int nextIndex = 0;
        if (index+ addFrameNum <= storeAnimFrameNum - 1)
        {
            nextIndex = index + addFrameNum;
        }
        else
        {
            nextIndex = index + addFrameNum - storeAnimFrameNum;
        }
        return nextIndex;
    }
}
public class AnimFrameData
{
    public List<Vector3> positions = new List<Vector3>();
    public List<Quaternion> rotations = new List<Quaternion>();
    public List<Mesh> meshes = new List<Mesh>();
    public AnimFrameData(SkinnedMeshRenderer[] meshRenders)
    {
        StoreAnimMesh(meshRenders, true);
    }
    public void StoreAnimMesh(SkinnedMeshRenderer[] meshRenders, bool isFirstInit = false)
    {
        if (meshRenders == null)
        {
            return;
        }
        int meshRendersCount = meshRenders.Length;
        for (int i = 0; i < meshRendersCount; i++)
        {
            Mesh mesh;

            if (isFirstInit)
            {
                mesh = new Mesh();
                meshes.Add(mesh);
                positions.Add(Vector3.zero);
                rotations.Add(Quaternion.identity);
            }
            else
            {
                mesh = meshes[i];
            }
            if (mesh == null)
            {
                break;
            }
            meshRenders[i].BakeMesh(mesh);
            positions[i]= meshRenders[i].transform.position;
            rotations[i] = meshRenders[i].transform.rotation;
        }
    }
    
    public bool SetAnimDataByIndex(int index,MeshFilter meshFilter)
    {
        Mesh mesh = meshes[index];
        Vector3 pos = positions[index];
        Quaternion quaternion = rotations[index];
        if (mesh == null|| pos==null)
        {
            return false;
        }
        meshFilter.mesh = mesh;
        meshFilter.transform.position = pos;
        meshFilter.transform.rotation = quaternion;
        return true;
    }
}