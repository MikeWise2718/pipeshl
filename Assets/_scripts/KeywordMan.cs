using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using GraphAlgos;

namespace BirdRouter
{
    public class KeywordMan
    {
        RouteMan rman=null;

        KeywordRecognizer kR1 = null;
        KeywordRecognizer kR2 = null;

        public LinkedList<string> kwhistory = new LinkedList<string>();


        Dictionary<string, System.Action> keywords1 = null;
        Dictionary<string, System.Action> keywords2 = null;

        public KeywordMan(RouteMan rman)
        {
            this.rman = rman;
            initKeywords();
        }
        public int totalKeywordCount()
        {
            int ncnt = 0;
            if (keywords1 != null) ncnt += keywords1.Count;
            if (keywords2 != null) ncnt += keywords2.Count;
            return ncnt;
        }
        public int totalKeys1()
        {
            int ncnt = 0;
            if (keywords1 != null) ncnt += keywords1.Count;
            return ncnt;
        }
        public int totalKeys2()
        {
            int ncnt = 0;
            if (keywords2 != null) ncnt += keywords2.Count;
            return ncnt;
        }
        public string getlastwordused()
        {
            var rval = "";
            if (kwhistory != null)
            {
                if (kwhistory.Count > 0)
                {
                    rval = kwhistory.Last.Value;
                }
            }
            return rval;
        }
        void addStandardKeywords(Dictionary<string, System.Action> keywords)
        {
            keywords.Add("generate", () => { rman.CreateLinkCloud(); });
            keywords.Add("regen", () => { rman.Regen(); }); // why does this one not work
            keywords.Add("start router", () => { rman.CreateLinkCloud(); });
            keywords.Add("hide route", () => { rman.HideRoute(); });
            keywords.Add("show route", () => { rman.ShowRoute(); });
            keywords.Add("reset", () => { rman.ResetCalled(); });
            keywords.Add("begin", () => { rman.ResetCalled(); });
            keywords.Add("pause", () => { rman.PauseBird(); });
            keywords.Add("unpause", () => { rman.UnPauseBird(); });
            keywords.Add("resume", () => { rman.UnPauseBird(); });
            keywords.Add("change", () => { rman.NextBirdForm(); });
            keywords.Add("higher", () => { rman.FlyBirdHigher(); });
            keywords.Add("lower", () => { rman.FlyBirdLower(); });
            keywords.Add("stop", () => { rman.StopBird(); });
            keywords.Add("go fast", () => { rman.SetSpeed(4); });
            keywords.Add("faster", () => { rman.FasterBird(); });
            keywords.Add("slower", () => { rman.SlowerBird(); });
            keywords.Add("go slow", () => { rman.SetSpeed(0.5f); });
            keywords.Add("go", () => { rman.StartBird(); });
            keywords.Add("start", () => { rman.StartBird(); });
            keywords.Add("continue", () => { rman.StartBird(); });
            keywords.Add("reverse", () => { rman.ReversePath(); });

            keywords.Add("black balls", () => { rman.SetBallColor("black"); });
            keywords.Add("blue balls", () => { rman.SetBallColor("blue"); });
            keywords.Add("black pipes", () => { rman.SetPipeColor("black"); });
            keywords.Add("yellow pipes", () => { rman.SetPipeColor("yellow"); });
            keywords.Add("blue pipes", () => { rman.SetPipeColor("steelblue"); });

            
            keywords.Add("field of view 10", () => { rman.SetFov(10); });
            //keywords.Add("field of view 20", () => { rman.SetFov(20); });
            //keywords.Add("field of view 30", () => { rman.SetFov(30); });
            //keywords.Add("field of view 40", () => { rman.SetFov(30); });
            //keywords.Add("field of view 50", () => { rman.SetFov(30); });
            //keywords.Add("field of view 60", () => { rman.SetFov(60); });
            //keywords.Add("field of view 70", () => { rman.SetFov(60); });
            //keywords.Add("field of view 80", () => { rman.SetFov(60); });
            //keywords.Add("field of view 90", () => { rman.SetFov(90); });

            keywords.Add("grid on", () => { rman.GridOn(); });
            keywords.Add("grid off", () => { rman.GridOn(); });
            keywords.Add("grid bigger", () => { rman.GridBigger(); });
            keywords.Add("grid smaller", () => { rman.GridSmaller(); });


            keywords.Add("inc inc", () => { rman.IncInc(); });
            keywords.Add("dec inc", () => { rman.DecInc(); });

            keywords.Add("grow", () => { rman.Grow(); });
            keywords.Add("shrink", () => { rman.Shrink(); });

            keywords.Add("translate up", () => { rman.TranslateUp(); });
            keywords.Add("translate down", () => { rman.TranslateDown(); });

            keywords.Add("translate left", () => { rman.TranslateLeft(); });
            keywords.Add("translate right", () => { rman.TranslateRight(); });

            keywords.Add("translate forward", () => { rman.TranslateForward(); });
            keywords.Add("translate backward", () => { rman.TranslateBack(); });
            keywords.Add("translate back", () => { rman.TranslateBack(); });

            keywords.Add("rotate clockwise", () => { rman.RotateCw(); });
            keywords.Add("rotate counter clockwise", () => { rman.RotateCcw(); });

            keywords.Add("rotate right", () => { rman.RotateCw(); });
            keywords.Add("rotate left", () => { rman.RotateCcw(); });

            keywords.Add("grow 10", () => { rman.Grow10(); });
            keywords.Add("grow 50", () => { rman.Grow50(); });
            keywords.Add("grow 75", () => { rman.Grow75(); });

            keywords.Add("shrink 10", () => { rman.Shrink10(); });
            keywords.Add("shrink 50", () => { rman.Shrink50(); });
            keywords.Add("shrink 75", () => { rman.Shrink75(); });

            keywords.Add("gen b h o", () => { rman.GenBHO(); });
            keywords.Add("gen Redwest B 3", () => { rman.GenRedwb3(); });
            keywords.Add("gen Redwest Simple", () => { rman.GenRedwb3simple(); });
            keywords.Add("gen 43 1", () => { rman.Gen43_1(); });
            keywords.Add("gen 43 2", () => { rman.Gen431p2(); });
            //keywords.Add("john b h o", () => { rman.GenBHO(); });
            //keywords.Add("john Redwest B 3", () => { rman.GenRedwb3(); });
            //keywords.Add("john Redwest Simple", () => { rman.GenRedwb3simple(); });
            //keywords.Add("John 43 1", () => { rman.Gen43_1(); });
            //keywords.Add("john 43 2", () => { rman.Gen431p2(); });
            keywords.Add("gen sphere", () => { rman.GenSphere(); });

            keywords.Add("garnish", () => { rman.NextGarnish(); });
            keywords.Add("correct position", () => { rman.CorrectPosition(); });
            keywords.Add("correct angle", () => { rman.CorrectAngle(); });
            keywords.Add("correct both", () => { rman.CorrectPositionAndAngle(); });

            keywords.Add("home", () => { rman.SetRoomAction(RouteMan.RoomActionE.makeHome); });
            keywords.Add("make start", () => { rman.SetRoomAction(RouteMan.RoomActionE.makeStart); });
            keywords.Add("orient", () => { rman.SetRoomAction(RouteMan.RoomActionE.orientOn); });
            keywords.Add("destination", () => { rman.SetRoomAction(RouteMan.RoomActionE.makeDestination); });

            keywords.Add("hide links", () => { rman.HideLinks(); });
            keywords.Add("show links", () => { rman.ShowLinks(); });

            keywords.Add("revert to home", () => { rman.RevertToHome(); });

            keywords.Add("scroll up", () => { rman.ScrollStatus(-1); });
            keywords.Add("scroll down", () => { rman.ScrollStatus(1); });
            keywords.Add("page up", () => { rman.ScrollPage(-1); });
            keywords.Add("page down", () => { rman.ScrollPage(1); });
            keywords.Add("top", () => { rman.ScrollStatus(-100000); });
            keywords.Add("bottom", () => { rman.ScrollStatus(100000); });

            keywords.Add("hide status", () => { rman.SetStatusInfoMode(StatusCtrl.outModeE.none); });
            keywords.Add("show status", () => { rman.SetStatusInfoMode(StatusCtrl.outModeE.geninfo); });

            keywords.Add("help", () => { rman.SetStatusInfoMode(StatusCtrl.outModeE.help); });
            keywords.Add("status trace", () => { rman.SetStatusInfoMode(StatusCtrl.outModeE.trace); });
            keywords.Add("status voice", () => { rman.SetStatusInfoMode(StatusCtrl.outModeE.voiceCmdHistory); });
            keywords.Add("status info", () => { rman.SetStatusInfoMode(StatusCtrl.outModeE.geninfo); });

            keywords.Add("load all rooms", () => { rman.SetStatusInfoMode(StatusCtrl.outModeE.geninfo); });

            keywords.Add("inc keyword limit", () => { rman.IncKeyLimit(); });
            keywords.Add("dec keyword limit", () => { rman.DecKeyLimit(); });

            keywords.Add("enable spatial mapping", () => { rman.EnableSpatialMapping(); });
            keywords.Add("disable spatial mapping", () => { rman.DisableSpatialMapping(); });
            keywords.Add("increase spatial extent", () => { rman.IncSpatialExtent(); });
            keywords.Add("decrease spatial extent", () => { rman.DecSpatialExtent(); });
            keywords.Add("more spatial detail", () => { rman.IncSpatialDetail(); });
            keywords.Add("less spatial detail", () => { rman.DecSpatialDetail(); });

//            keywords.Add("toggle move camera", () => { rman.ToggleMoveCamera(); });
            keywords.Add("toggle drop error markers", () => { rman.ToggleDropErrorMarkers(); });
            keywords.Add("correct error markers", () => { rman.CorrectOnErrorMarkers(); });
            keywords.Add("start error marking", () => { rman.StartErrorMarking(); });
            keywords.Add("finish error marking", () => { rman.FinishErrorMarking(); });
            keywords.Add("toggle floor plan", () => { rman.ToggleFloorPlan(); });
            keywords.Add("Error corect on", () => { rman.SetErrorCorrect(true); });
            keywords.Add("Error correct off", () => { rman.SetErrorCorrect(false); });

            keywords.Add("Save Log", () => { rman.writeLogToAzureBlob(); });

        }

        // Use this for initialization
        void initKeywords()
        {
            keywords1 = new Dictionary<string, System.Action>();
            addStandardKeywords(keywords1);
            kR1 = new KeywordRecognizer(keywords1.Keys.ToArray());
            kR1.OnPhraseRecognized += kROnPhraseRecognized;
            kR1.Start();
            RouteMan.Log("keywords1 started num keys:" + keywords1.Count);
        }

        public void initKeywordsWithRooms()
        {
            keywords2 = new Dictionary<string, System.Action>();
            var keylist = rman.linkcloudctrl.GetKeywordKeys();
            int nadded = 0;
            foreach (var key in keylist)
            {
                var val = rman.linkcloudctrl.GetKeywordValue(key);
                keywords2.Add(key, () => { rman.NodeAction(val); });
                nadded += 1;
                RouteMan.Log("Adding key:" + key + "  Val:" + val);
            }
            if (kR2 != null)
            {
                kR2.Stop();
                kR2.Dispose();
                kR2 = null;
            }
            if (keywords2.Count > 0)
            {
                kR2 = new KeywordRecognizer(keywords2.Keys.ToArray());
                kR2.OnPhraseRecognized += kROnPhraseRecognized2;
                kR2.Start();
            }
            RouteMan.Log("keywords rooms num keys:" + keywords2.Count);
        }

        void kROnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            System.Action kwAction;
            if (keywords1.TryGetValue(args.text, out kwAction))
            {
                RouteMan.Log("You just said " + args.text);
                kwhistory.AddLast(args.text);
                kwAction.Invoke();
            }
            else
            {
                RouteMan.Log("Could not find " + args.text);
            }
        }
        void kROnPhraseRecognized2(PhraseRecognizedEventArgs args)
        {
            System.Action kwAction;
            if (keywords2.TryGetValue(args.text, out kwAction))
            {
                RouteMan.Log("You just said " + args.text);
                kwhistory.AddLast(args.text);
                kwAction.Invoke();
            }
            else
            {
                RouteMan.Log("Could not find " + args.text);
            }
        }
    }
}