using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GhostEffect : MonoBehaviour {
    public float ghostCreateCircle=5;
	private float runTime=0;
	private Vector3 lastPosition=Vector3.zero;
	public Material material;
	private Shader ghostShader;
	SkinnedMeshRenderer[] meshRender;
	// Use this for initialization
	void Start () {
		meshRender=this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		lastPosition=this.transform.position;
		ghostShader=Shader.Find("lijia/Xray");
	}
	
	// Update is called once per frame
	void Update () 
	{
		runTime+=Time.deltaTime;
		if(runTime<ghostCreateCircle)
		{
			return;
		}
		runTime=0;
		if(Vector3.Distance(lastPosition,transform.position)<0.5f)
		{
			return;
		}
		lastPosition=transform.position;
		if (meshRender == null)
            return;
        for (int i = 0; i < meshRender.Length; i++) {
            Mesh mesh = new Mesh ();
            meshRender[i].BakeMesh(mesh);
             
            GameObject go = new GameObject();
            go.hideFlags = HideFlags.HideAndDontSave;
             
            //GhostItem item = go.AddComponent<GhostItem>();//控制残影消失
            //item.duration = duration;
            //item.deleteTime = Time.time + duration;
             
            MeshFilter filter = go.AddComponent<MeshFilter>();
            filter.mesh = mesh;
             
            MeshRenderer meshRen = go.AddComponent<MeshRenderer>();
             
			 meshRen.material = material;
            //meshRen.material = meshRender[i].material;
            //meshRen.material.shader = ghostShader;//设置xray效果
            //meshRen.material.SetFloat("_Intension", Intension);//颜色强度传入shader中
             
            go.transform.localScale = meshRender[i].transform.localScale;
            go.transform.position = meshRender[i].transform.position;
            go.transform.rotation = meshRender[i].transform.rotation;
             
            //item.meshRenderer = meshRen;
        }
	}
}
