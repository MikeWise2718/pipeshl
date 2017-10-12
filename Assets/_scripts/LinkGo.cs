using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphAlgos;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BirdRouter
{
    [SelectionBase]
    public class LinkGo : MonoBehaviour
    {
        RouteMan rman;
        // Use this for initialization
        public LcLink link;
        public string linkName;
        public string nodeName1;
        public string nodeName2;
        public string lastmsg;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ChangeName()
        {
            var lcld = link.lc;
            var stat = lcld.ChangeLinkName(link.name, linkName);
            lastmsg = stat.fullstatus();
            if (stat.ok)
            {
                RouteMan.Log(lastmsg);
                rman.RequestRefresh(linkName);
            }
            else
            {
                RouteMan.Log(lastmsg);
                linkName = link.name; // change the go name back to avoid confusion
            }

        }
        public static GameObject MakeNewLinkGo(RouteMan rman, LcLink link,float linkRadius, string cname)
        {
            var go = new GameObject();
            var linkgo = go.AddComponent<LinkGo>();
            linkgo.rman = rman;
            linkgo.link = link;
            linkgo.name = link.name;
            linkgo.linkName = link.name;
            linkgo.nodeName1 = link.node1.name;
            linkgo.nodeName2 = link.node2.name;




            go.name = link.name; //  + "-go";
            var p1 = link.node1.pt;
            var p2 = link.node2.pt;
            var midpt = (p1 + p2) / 2;
            go.transform.localPosition = midpt;
            var pcyl = GraphUtil.CreatePipe(link.name, p1, p2, linkRadius, cname);
            if (rman != null && rman.garnish != RouterGarnishE.none)
            {
                var text = link.name;
                var anglat = GraphUtil.GetAngLatDegrees(p1, p2);
                GraphUtil.addFloatingTextStatic(go, midpt, text + "  ang:" + anglat, cname, anglat + 90);
            }
            pcyl.transform.parent = go.transform;
            return go;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LinkGo))]   //The script which you want to button to appear in
    public class CustomLinkGoInspectorScript : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();    //This goes first

            if (GUILayout.Button("Change Link Name"))    // If the button is clicked
            {
                LinkGo linkgo = (LinkGo)target;    //The target script
                linkgo.ChangeName();
            }
        }
    }
#endif
}