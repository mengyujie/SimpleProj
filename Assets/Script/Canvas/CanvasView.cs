using UnityEngine;
using UnityEngine.UI;

namespace MVC.Canvas
{
    public class CanvasView : MonoBehaviour
    {
        public Button btn666;

        void Awake()
        {
            btn666=transform.Find("#btn666").GetComponent<Button>();

        }
        void OnDestroy()
        {
            btn666=null;

        }
    }
}