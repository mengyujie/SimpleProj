using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;

public class ExportCSharpCode
{
    public StringBuilder viewContent = new StringBuilder();
    public const string prefix = "    ";
    public string variabeArea = "";
    public string findArea = "";
    public string destroyArea = "";

    [MenuItem("madeByMYJ/Export CSHarp Controller Code", false, 10)]
    public static void ExportCSharpController()
    {
        Transform[] obj = Selection.transforms;
        ExportCSharpCode exp = new ExportCSharpCode();
        exp.ExportCSharpController(obj[0].name);
        AssetDatabase.Refresh();
    }
    private void ExportCSharpController(string name)
    {
        viewContent.Append("using Frame.MVC;\n");
        viewContent.Append("using UnityEngine;\n");
        viewContent.Append("namespace MVC.");
        viewContent.Append(name);
        viewContent.Append("\n{\n");
        viewContent.Append(prefix);
        viewContent.Append("public class ");
        viewContent.Append(name);
        viewContent.Append("Ctrl : ControllerBase\n");
        viewContent.Append(prefix);
        viewContent.Append("{\n");
        viewContent.Append(string.Format("{0}{1}{2}{3}{4}{5}", prefix, prefix, "public override void OnViewCreated(GameObject obj)\n", prefix, prefix, "{\n}\n}\n}"));
        string outputPath = Application.dataPath + "/prefabView";
        if (Directory.Exists(outputPath) == false)
        {
            Directory.CreateDirectory(outputPath);
        }
        File.WriteAllText(outputPath + "/" + name+"Ctrl.cs", viewContent.ToString());

    }
    [MenuItem("madeByMYJ/Export CSHarp Model Code", false, 10)]
    public static void ExportCSharpModel()
    {
        Transform[] obj = Selection.transforms;
        ExportCSharpCode exp = new ExportCSharpCode();
        exp.ExportCSharpModel(obj[0].name);
        AssetDatabase.Refresh();
    }
    private void ExportCSharpModel(string name)
    {
        viewContent.Append("using Frame.MVC;\n");
        viewContent.Append(string.Format("namespace MVC.{0}\n{{\n", name));
        viewContent.Append(string.Format("{0}public class {1}Model : ModelBase\n{2}{{\n\n}}\n}}",prefix,name,prefix));
        string outputPath = Application.dataPath + "/prefabView";
        if (Directory.Exists(outputPath) == false)
        {
            Directory.CreateDirectory(outputPath);
        }
        File.WriteAllText(outputPath + "/" + name + "Model.cs", viewContent.ToString());
    }
    [MenuItem("madeByMYJ/Export CSHarp View Code", false, 10)]
    public static void ExportCSharpView()
    {
        Transform[] obj = Selection.transforms;
        ExportCSharpCode exp = new ExportCSharpCode();
        exp.AddFrame(obj[0].name);
        exp.AddContentArea(obj[0], "");
        exp.TidyAndOutput(obj[0].name + "View.cs");
        AssetDatabase.Refresh();
    }
    private void AddFrame(string classname)
    {
        viewContent.Append("using UnityEngine;\n");
        viewContent.Append("using UnityEngine.UI;\n\n");
        viewContent.Append(string.Format("namespace MVC.{0}\n{{\n", classname));
        viewContent.Append(string.Format("{0}public class {1}View : MonoBehaviour\n{2}{{\n",prefix,classname,prefix));
        viewContent.Append("[variabeArea]\n");
        viewContent.Append(string.Format("{0}{1}void Awake()\n{2}{3}{{\n",prefix,prefix,prefix,prefix));
        viewContent.Append(string.Format("[findArea]\n{0}{1}}}\n",prefix,prefix));
        viewContent.Append(string.Format("{0}{1}void OnDestroy()\n{2}{3}{{\n", prefix, prefix, prefix, prefix));
        viewContent.Append(string.Format("[destroyArea]\n{0}{1}}}\n{2}}}\n}}",prefix,prefix,prefix));
    }
    private void AddContentArea(Transform obj, string path)
    {
        bool isAdd = false;
        if (obj.name.StartsWith("#"))
        {
            string varriabeName =string.Format("{0}", obj.name.Substring(1));
            switch (obj.name.Substring(1, 3).ToLower())
            {
                case "img":   //image
                    {
                        variabeArea += prefix+ prefix + "public Image " + varriabeName + ";\n";
                        findArea += prefix + prefix + prefix + varriabeName + "=transform.Find(\"" + path +"\").GetComponent<Image>();\n";
                        isAdd = true;
                        break;
                    }
                case "txt":  //text
                    {
                        variabeArea += prefix + prefix + "public Text " + varriabeName + ";\n";
                        findArea += prefix + prefix + prefix + varriabeName + "=transform.Find(\"" + path +"\").GetComponent<Text>();\n";
                        isAdd = true;
                        break;
                    }
                case "btn": //button
                    {
                        variabeArea += prefix + prefix + "public Button " + varriabeName + ";\n";
                        findArea += prefix + prefix + prefix + varriabeName + "=transform.Find(\"" + path +"\").GetComponent<Button>();\n";
                        isAdd = true;
                        break;
                    }
                case "inp":  //inputfield
                    {
                        variabeArea += prefix + prefix + "public InputField " + varriabeName + ";\n";
                        findArea += prefix + prefix + prefix + varriabeName + "=transform.Find(\"" + path + "\").GetComponent<InputField>();\n";
                        isAdd = true;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            if (!isAdd)
            {
                switch (obj.name.Substring(1, 5).ToLower())
                {
                    case "conta":  //container
                        variabeArea += prefix + prefix + "public Transform " + varriabeName + ";\n";
                        findArea += prefix + prefix + prefix + varriabeName + "=transform.Find(\"" + path + "\");\n";
                        isAdd = true;
                        break;
                    case "templ":  //template
                        variabeArea += prefix + prefix + "public GameObject " + varriabeName + ";\n";
                        findArea += prefix + prefix + prefix + varriabeName + "=transform.Find(\"" + path + "\").gameObject;\n";
                        isAdd = true;
                        break;
                    case "scrol":   //scrollview
                        variabeArea += prefix + prefix + "public ScrollRect " + varriabeName + ";\n";
                        findArea += prefix + prefix + prefix + varriabeName + "=transform.Find(\"" + path + "\").GetComponent<ScrollRect>();\n";
                        isAdd = true;
                        break;
                    default:
                        break;
                }
            }
            if (isAdd)
            {
                destroyArea += string.Format("{0}{1}{2}{3}=null;\n", prefix, prefix, prefix, varriabeName);
            }

        }
        for (int i = 0; i < obj.childCount; i++)
        {
            if (path == "")
            {
                AddContentArea(obj.GetChild(i), obj.GetChild(i).name);
            }
            else
            {
                AddContentArea(obj.GetChild(i), path+"/"+obj.GetChild(i).name);
            }
        }
    }
    private void TidyAndOutput(string classname)
    {
        viewContent = viewContent.Replace("[variabeArea]", variabeArea);
        viewContent = viewContent.Replace("[findArea]", findArea);
        viewContent = viewContent.Replace("[destroyArea]", destroyArea);
        string outputPath = Application.dataPath + "/prefabView";
        if (Directory.Exists(outputPath)==false)
        {
            Directory.CreateDirectory(outputPath);
        }
        File.WriteAllText(outputPath+"/" + classname, viewContent.ToString());

    }
    
}
