using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace GraphAlgos
{
    [Serializable]
    public class JsonLcNode
    {
        public string name;
        public Vector3 pt;
    }

    [Serializable]
    public class JsonLcLink
    {
        public string name;
        public string n1;
        public string n2;
    }

    [Serializable]
    public class JsonLcNodeList
    {
        public List<JsonLcNode> list;

        public JsonLcNodeList()
        {
            this.list = new List<JsonLcNode>();
        }

        public void Add(JsonLcNode p)
        {
            list.Add(p);
        }

    }

    [Serializable]
    public class JsonLcLinkList
    {
        public List<JsonLcLink> list;

        public JsonLcLinkList()
        {
            this.list = new List<JsonLcLink>();
        }

        public void Add(JsonLcLink lnk)
        {
            list.Add(lnk);
        }

    }
    [Serializable]
    public class JlcVersion
    {
        public string FormatVersion = "0.1.1";
        public string CreationTime = DateTime.Now.ToString("o");
#if !NETFX_CORE
        public string HostName = System.Environment.MachineName;
        public string OsName = System.Environment.OSVersion.ToString();
        public string DotNetFramework = System.Environment.Version.ToString();
#endif
        public string AssemblyVersion = "";
        public string AssemblyBuildDate = "";

        public JlcVersion()
        {
#if !NETFX_CORE
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            AssemblyVersion = version.ToString();
            DateTime buildDate = new DateTime(2000,1,1).AddDays(version.Build).AddSeconds(version.Revision*2);
            AssemblyBuildDate = buildDate.ToString("o");
#endif
        }
    }

    [Serializable]
    public class JsonLinkCloud
    {
        public JlcVersion version;
        public graphtex floorplan;
        public JsonLcNodeList nodes;
        public JsonLcLinkList links;
 
        public JsonLinkCloud(graphtex gt)
        {
            version = new JlcVersion(); 
           floorplan = new graphtex(gt);
           nodes = new JsonLcNodeList();
           links = new JsonLcLinkList();
        }

        public void AddNode(string name, Vector3 pt)
        {
            var node = new JsonLcNode();
            node.name = name;
            node.pt = pt;
            nodes.Add(node);
        }
        public void AddLink(string name, string nodename1, string nodename2)
        {
            var link = new JsonLcLink();
            link.name = name;
            link.n1 = nodename1;
            link.n2 = nodename2;
            links.Add(link);
        }

        public static void WriteToFile(JsonLinkCloud jlc, string fname)
        {
            string jstring = JsonUtility.ToJson(jlc);
            using (FileStream fs = new FileStream(fname, FileMode.Create))
            {
                var sw = new StreamWriter(fs);
                sw.Write(jstring);
                sw.Flush();
                fs.Dispose();
            }
        }

        public void WriteToConsole()
        {
#if UNITY_EDITOR
            Console.WriteLine(JsonUtility.ToJson(this));
#endif
        }
        public static JsonLinkCloud ReadFromFile(string fname)
        {
            var jstring = "";
            using (FileStream fs = new FileStream(fname, FileMode.Open))
            {
                var sr = new StreamReader(fs);
                jstring = sr.ReadToEnd();
                fs.Dispose();
            }
            var jlc = JsonUtility.FromJson<JsonLinkCloud>(jstring);
            return jlc;
        }
    }
}