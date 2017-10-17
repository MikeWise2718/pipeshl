using System.Collections.Generic;
using UnityEngine;
using GraphAlgos;

namespace BirdRouter
{
    public class StatusCtrl : MonoBehaviour {

        private RouteMan rman;
        public void setRouteMan(RouteMan rman)
        {
            // this is attached as a component, thus we cannot set it in a contructor
            this.rman = rman;
        }

        public enum outModeE {  none=0,geninfo=1,trace=2,voiceCmdHistory=3,version=4,help=5 }
        int noutModeE = 5; // typeof(outModeE).GetEnumValues(); // this should work?

        outModeE nextOutMode(outModeE oe)
        {
            var next = (outModeE)(((int)oe + 1) % noutModeE );
            return (next);
        }

        public outModeE outMode = outModeE.none;

        public void NextInfoMode()
        {
            outMode = nextOutMode(outMode);
        }

        void initVals()
        {
            initHelp();
        }
        void Start()
        {
            initVals();
        }
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        GameObject statusgos=null;
        Vector3 statuspos = new Vector3(-0.5f,0,8f);
        public void CreateStatusGos()
        {

            if (statusgos == null)
            {
                statusgos = new GameObject();
                statusgos.name = "status";
                var mc = Camera.main;
                statusgos.transform.parent = mc.transform;
                statusgos.transform.localPosition = statuspos;
                statusgos.transform.Rotate(0,90,0);
                stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
            }
            var fwd = Camera.main.transform.forward;
            fwd.y = 0;
            fwd = Vector3.Normalize(fwd);
            float ang =  -180*Mathf.Atan2(fwd.z, fwd.x)/Mathf.PI;
            //RouteMan.Log("ang:" + ang);
            addFloatingText(statusgos, statuspos,"Hello World", "white", ang, 0);
        }
        public void DeleteStatusGos()
        {
            if (statusgos != null)
            { 
                Destroy(statusgos);
            }
            statusgos = null;
        }
        public void RealizeMode()
        {
            switch (outMode)
            {
                case outModeE.none:
                    {
                        DeleteStatusGos();
                        break;
                    }
                default:
                    {
                        DeleteStatusGos();
                        CreateStatusGos();
                        break;
                    }
            }
        }
        TextMesh tm = null;
        public void addFloatingText(GameObject go, Vector3 pt, string text, string colorname, float yrot = 0, float yoff = 0)
        {
            tm = go.AddComponent<TextMesh>();
            int linecount = text.Split('\n').Length; // count number of newlines
            tm.text = text;
            tm.fontSize = 12;
            tm.anchor = TextAnchor.UpperCenter;
            tm.color = GraphUtil.getcolorbyname(colorname);
            float sfak = 0.1f;
            tm.transform.localScale = new Vector3(sfak, sfak, sfak);
            tm.transform.Rotate(0, yrot, 0);
            tm.transform.localPosition = pt + new Vector3(0, sfak*yoff+linecount * 0.25f, 0);
        }
        public string getLastVoiceCmd()
        {
            var lcmd = "...";
            if (rman.keyman!=null)
            {
                var rv = rman.keyman.getlastwordused();
                if (rv != "") lcmd = rv;
            }
            return lcmd;
        }
        int ucnt = 0;
        int lastfrcnt = 0;
        float lastelapsecs = 0;
        float lastfrsecs = 0;
        float lastfrrate = 0;
        float lastmaxdeltat = 0;
        float maxdeltat = 0;

        public int scrollpos = 0;
        public int scrolllen = 0;
        public int screensize = 8;
        public int scrollmaxs = 0;

        public int maxscroll()
        {
            int maxs = 0;
            switch (outMode)
            {
                default:
                    {
                        maxs = 0;
                        break;
                    }
                case outModeE.trace:
                    {
                        maxs = rman.loglist.Count - screensize;
                        break;
                    }
                case outModeE.voiceCmdHistory:
                    {
                        maxs = rman.keyman.kwhistory.Count - screensize;
                        break;
                    }
                case outModeE.help:
                    {
                        maxs = helplist.Count - screensize;
                        break;
                    }
            }
            return (maxs);
        }
        public void scroll(int n)
        {
            scrollpos += n;
            if (scrollpos < 0) scrollpos = 0;
            scrollmaxs = maxscroll();
            if (scrollpos>=scrollmaxs) scrollpos = scrollmaxs-1;
        }
        public void scrollpage(int n)
        {
            if (n > 0)
            {
                scroll(screensize -1);
            }
            else
            {
                scroll(1-screensize);
            }
        }

        string scrollThroughLinkedList(LinkedList<string> ll)
        {
            var text = "Last Cmd:" + getLastVoiceCmd(); // the zeroth line
            int isv = 1; // we wrote one line already   
            var lp = ll.First;
            for (int i = 0; i < scrollpos && lp.Next != null; i++) lp = lp.Next;
            while (isv < screensize && lp != null)
            {
                text += "\n";
                text += lp.Value;
                lp = lp.Next;
                isv += 1;
            }
            return text;
        }
        void setFloatingTextAtts()
        {
            switch (outMode)
            {
                default:
                case outModeE.none:
                    {
                        break;
                    }
                case outModeE.geninfo:
                    {
                        screensize = 12;
                        tm.fontSize = 10;
                        tm.anchor = TextAnchor.UpperCenter;
                        tm.color = GraphUtil.getcolorbyname("white");
                        break;
                    }
                case outModeE.voiceCmdHistory:
                    {
                        screensize = 12;
                        tm.fontSize = 8;
                        tm.anchor = TextAnchor.UpperCenter;
                        tm.color = GraphUtil.getcolorbyname("pink");
                        break;
                    }
                case outModeE.trace:
                    {
                        screensize = 12;
                        tm.fontSize = 8;
                        tm.anchor = TextAnchor.UpperCenter;
                        tm.color = GraphUtil.getcolorbyname("forestgreen");
                        break;
                    }
                case outModeE.help:
                    {
                        screensize = 10;
                        tm.fontSize = 12;
                        tm.anchor = TextAnchor.UpperCenter;
                        tm.color = GraphUtil.getcolorbyname("blue");
                        break;
                    }

            }
        }
        void setFloatingText()
        {
            var text = "";
            switch (outMode)
            {
                default:
                case outModeE.none:
                    {
                        text = "None";
                        break;
                    }
                case outModeE.geninfo:
                    {
                        text = "";
                        if (rman != null)
                        {
                            var campos = Camera.main.transform.position;
                            var cfwd = Camera.main.transform.forward;
                            var cpop = campos;
                            if (rman.pathctrl != null && rman.pathctrl.path != null)
                            {
                                cpop = rman.pathctrl.FindClosestPointOnPath(campos);
                            }
                            var delta = cpop - campos + new Vector3(0, rman.home_height, 0);
                            var nlink = rman.linkcloudctrl.GetLinkCount();
                            var nnode = rman.linkcloudctrl.GetLinkCount();
                            var lnkvis = rman.linkcloudctrl.linksvisible;
                            var snode = rman.pathctrl.startnodename;
                            var enode = rman.pathctrl.endnodename;

                            text += "Last Cmd:" + getLastVoiceCmd();
                            text += "\nLinks:" + nlink + "  Nodes:" + nnode + "Visible:" + lnkvis;
                            text += "\nStartNode:" + snode + "  EndNode:" + enode;
                            text += "\nrgoScale:" + rman.rgoScale.ToString("F2") + "   (" + rman.scaleIncFak + " Fak )";
                            text += "\nrgoRotate:" + rman.rgoRotate.ToString("F1") + "   (" + rman.rotationIncDeg + " deg )";
                            text += "\nrgoTrans:" + rman.rgoTranslate.ToString("F2") + "   (" + rman.tranlsationIncMeter + " M )";
                            text += "\nMC.pos:" + campos.ToString("F2") + " hh:" + rman.home_height;
                            text += "\nMC.fwd:" + cfwd.ToString("F2") + "  CPOP:" + cpop.ToString("F2");
                            text += "\nDelta:" + delta.ToString("F2" +" Error Correct:"+rman.autoerrorcorrect);
                            text += "\nVoice Total Keys:" + rman.keycount + " k1:" + rman.keyman.totalKeys1()+ " k2:"+rman.keyman.totalKeys2();
#if NETFX_CORE
                            text += "\nUniversal Windows Platform (NETFX_CORE defined)";
#else
                            text += "\n.NET Framework:" + System.Environment.Version;
#endif
                            // Following Won't compile in UWP
                            //object[] list = Assembly.GetExecutingAssembly().GetCustomAttributes(true);
                            //if (list.Length > 0)
                            //{
                            //    var alist = list.OfType<TargetFrameworkAttribute>();
                            //    foreach (var att in alist)
                            //    {
                            //        text += "Framework:" + att.FrameworkName + "  DisplayName:" + att.FrameworkDisplayName;
                            //    }
                            //}
                        }
                        text += "\nUpdate:" + ucnt;

                        text += " Secs:" + lastelapsecs.ToString("F1");

                        text += " Frate:" + lastfrrate.ToString("F1");
                        text += " dt:" + lastmaxdeltat.ToString("F3");

                        break;
                    }
                case outModeE.voiceCmdHistory:
                    {
                        text = scrollThroughLinkedList(rman.keyman.kwhistory);
                        break;
                    }
                case outModeE.trace:
                    {
                        text = scrollThroughLinkedList(rman.loglist);
                        break;
                    }
                case outModeE.version:
                    {
                        text = "";


                        break;
                    }
                case outModeE.help:
                    {
                        text = scrollThroughLinkedList(helplist);
                        break;
                    }

            }
            tm.text = text;
        }

        // Update is called once per frame
        void Update() {
            lastelapsecs = stopwatch.ElapsedMilliseconds / 1000.0f;
            if (lastfrsecs == 0)
            {
                lastfrsecs = lastelapsecs;
                lastfrcnt = ucnt;
                lastmaxdeltat = 0;
                maxdeltat = 0;
            }
            else if ((lastelapsecs - lastfrsecs) > 1)
            {
                lastfrrate = (ucnt - lastfrcnt) / (lastelapsecs - lastfrsecs);
                lastfrsecs = lastelapsecs;
                lastfrcnt = ucnt;
                lastmaxdeltat = maxdeltat;
                maxdeltat = 0;
            }
            maxdeltat = Mathf.Max(Time.deltaTime, maxdeltat);
            if (tm != null)
            {
                setFloatingTextAtts();
                setFloatingText();
            }
            ucnt += 1;
        }
        LinkedList<string> helplist = new LinkedList<string>();
        void initHelp()
        {
            helplist.AddLast("Available Voice Commands");
            helplist.AddLast("   scroll down - scroll this list down one line");
            helplist.AddLast("   scroll up - scroll this list down one line");
            helplist.AddLast("   page down - scroll this list down one page");
            helplist.AddLast("   page up - scroll this list down one page");
            helplist.AddLast("   top - scroll to top of list");
            helplist.AddLast("   bottom - scroll to bottom of list");
            helplist.AddLast("Status");
            helplist.AddLast("   help - brings up this screen");
            helplist.AddLast("   status trace - brings up trace messages");
            helplist.AddLast("   status voice - brings up voice history");
            helplist.AddLast("   status info - brings up genral info screen");
            helplist.AddLast("   hide status - closes info");
            helplist.AddLast("Floor Initialization commands");
            helplist.AddLast("   gen redwest b 3 - sets up RedWest B 3rd floor");
            helplist.AddLast("   gen 43 1 - sets up a part of Bld 43 floor 1 ");
            helplist.AddLast("   gen 43 2 - sets up a part of Bld 43 floor 2 (minituraized)");
            helplist.AddLast("Room commands");
            helplist.AddLast("   destination - sets 'destination' as action mode");
            helplist.AddLast("   orient - sets 'orientation' as action mode");
            helplist.AddLast("   room 1234 - executes action on room 1234");
            helplist.AddLast("Bird commands");
            helplist.AddLast("   start - starts bird moving towards the destination");
            helplist.AddLast("   stop - stops bird (and creates a new start node there)");
            helplist.AddLast("   pause - pauses bird on a path");
            helplist.AddLast("   unpause - resumes bird journey on path");
            helplist.AddLast("   reset - puts the bird back at the start");
            helplist.AddLast("   faster - speeds up the bird by a factor of 2");
            helplist.AddLast("   slower - slows up the bird by a factor of 12");
            helplist.AddLast("   slow - makes the bird go at 0.5 m/s");
            helplist.AddLast("   fast - makes the bird go at 4.0 m/s");
            helplist.AddLast("Path commands");
            helplist.AddLast("   show path - shows the path");
            helplist.AddLast("   hide path - hides the path");
            helplist.AddLast("   reverse - reverses the path");
        }
    }
}