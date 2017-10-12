using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// GraphAlgos.cs  - This file contains static algoritms that we need in various places. 
/// </summary>


namespace GraphAlgos
{
    public class GraphUtil
    {
        static string colornames = "red,pink,orange,brown,olive,forestgreen,seagreen,limegreen,navyblue,steelblue,"+
                                   "blue,green,magenta,lilac,purple,yellow,cyan,black,white,clear,gray,grey";
        public static bool iscolorname(string name)
        {
            return (colornames.IndexOf(name) >= 0);
        }
        public static Color getcolorbyname(string name)
        {
            // this is a hack that voids a wierd error message about constants not being available
            switch (name)
            {
                case "red": return (Color.red);
                case "pink": return (new Color(1, 0.412f, 0.71f));
                case "scarlet": return (new Color(1, 0.14f, 0.0f));
                case "orange": return (new Color(1, 0.412f, 0));
                case "brown": return (new Color(0.647f, 0.164f, 0.164f));
                case "olive": return (new Color(0.5f, 0.5f, 0f));
                case "forestgreen": return (new Color(0.132f, 0.543f, 0.132f));
                case "seagreen": return (new Color(0.33f, 1.0f, 0.62f));
                case "limegreen": return (new Color(0.195f, 0.8f, 0.195f));
                case "steelblue": return (new Color(0.27f, 0.51f, 0.71f));
                case "navyblue": return (new Color(0.0f, 0.0f, 0.398f));
                case "blue": return (Color.blue);
                case "green": return (Color.green);
                case "magenta":
                case "purple": return (Color.magenta);
                case "lilac": return (new Color(0.86f, 0.8130f, 1.0f));
                case "yellow": return (Color.yellow);
                case "cyan": return (Color.cyan);
                case "black": return (Color.black);
                case "white": return (Color.white);
                case "chinawhite": return (new Color(0.937f, 0.910f, 0.878f));
                case "clear": return (Color.clear);
                case "grey":
                case "gray":
                default:
                    return (Color.gray);
            }
        }
        static public GameObject CreateMarkerSphere(string name, Vector3 pt, float size = 0.2f, string clr = "blue")
        {
            var sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sph.GetComponent<Renderer>().material.SetColor("_Color", getcolorbyname(clr));
            sph.name = name;
            sph.transform.localScale = new Vector3(size, size, size);
            sph.transform.localPosition = pt;
            return (sph);
        }

        static public GameObject CreatePipe(string pname, Vector3 frpt, Vector3 topt, float size = 0.1f, string clr = "yellow")
        {
            var dst2 = Vector3.Distance(frpt, topt) / 2;
            var dlt = topt - frpt;
            var dltxz = Mathf.Sqrt(dlt.x * dlt.x + dlt.z * dlt.z);
            var anglng = 180 * Mathf.Atan2(dltxz, dlt.y) / Mathf.PI;
            var anglat = 180 * Mathf.Atan2(dlt.x, dlt.z) / Mathf.PI;

            var pcyl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pcyl.name = pname;

            pcyl.GetComponent<Renderer>().material.SetColor("_Color", getcolorbyname(clr));

            pcyl.transform.localScale = new Vector3(size, dst2, size);
            pcyl.transform.Rotate(anglng, anglat, 0);
            pcyl.transform.localPosition = frpt + 0.5f * dlt;
            return (pcyl);
        }
        static public float GetAngLatDegrees(Vector3 frpt, Vector3 topt)
        {
            var dlt = topt - frpt;
            var anglat = 180 * Mathf.Atan2(dlt.x, dlt.z) / Mathf.PI;
            return (anglat);
        }
        static public float GetAngLngDegrees(Vector3 frpt, Vector3 topt)
        {
            var dlt = topt - frpt;
            var dltxz = Mathf.Sqrt(dlt.x * dlt.x + dlt.z * dlt.z);
            var anglng = 180 * Mathf.Atan2(dltxz, dlt.y) / Mathf.PI;
            return (anglng);
        }
        static public void addFloatingTextStatic(GameObject go, Vector3 pt, string text, string colorname, float yrot = 0, float yoff = 0)
        {
            var tm = go.AddComponent<TextMesh>();

            //var mr = sgo.AddComponent<MeshRenderer>();
            int linecount = text.Split('\n').Length;
            //tm.transform.parent = sgo.transform;
            tm.text = text;
            tm.fontSize = 12;
            tm.anchor = TextAnchor.UpperCenter;
            float sfak = 0.1f;
            tm.transform.localScale = new Vector3(sfak, sfak, sfak);
            tm.transform.Rotate(0, yrot, 0);
            tm.transform.localPosition = pt + new Vector3(0, sfak * yoff + linecount * 0.25f, 0);
            tm.transform.parent = go.transform;
            tm.color = GraphUtil.getcolorbyname(colorname);
        }
        public static float FindClosestLambClampedTo01(Vector3 pt, Vector3 p1, Vector3 p2)
        {
            var lamb = FindClosestLambUnclamped(pt, p1, p2);
            var clamb = Math.Min(1, Math.Max(0, lamb));
            return (clamb);
        }
        public static float FindClosestLambUnclamped(Vector3 pt, Vector3 p1, Vector3 p2)
        {
            var dlt = p2 - p1;
            var vp = pt - p1;
            var lamb = Vector3.Dot(vp, dlt) / (dlt.x * dlt.x + dlt.y * dlt.y + dlt.z * dlt.z);
            return (lamb);
        }
        public static LinkedList<string> loglist = null;
        public static void Log(string msg)
        {
            if (loglist != null)
            {
                loglist.AddFirst(msg);
            }
            else
            {
                Debug.Log(msg);
            }
        }
        static System.Random rangen = new System.Random();
        public static float NextFloat(float minf=0,float maxf=1)
        {
            var diff = maxf - minf;
            var nxf = (float) rangen.NextDouble();
            var rv = diff*nxf + minf;
            return (rv);
        }
        public static void writeLinkedListToFile(LinkedList<string> llist,string filename)
        {
            string[] llar = llist.ToArray<string>();
            System.IO.File.WriteAllLines(filename, llar);
        }
    }
}