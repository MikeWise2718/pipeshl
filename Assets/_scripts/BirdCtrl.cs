using UnityEngine;
using GraphAlgos;

namespace BirdRouter
{

    public class BirdCtrl : MonoBehaviour
    {
        RouteMan rman;
        public void setRouteMan(RouteMan rman)
        {
            // this is attached as a component, thus we cannot set it in a contructor
            this.rman = rman;
        }


        // These are the bird state variables - in principle they are all private
        // Some are declared "public", but on so that the Unity Editor can display them
        public enum BirdFormE { none, sphere, olive, hummingbird, utahteapot }
        public enum BirdStateE { unbuilt, atstart, running, paused, atgoal }

        public float rundist = 0;
        float lookaheadtime;
        public Vector3 curptlc = Vector3.zero;
        public Vector3 curptwc = Vector3.zero;

        public BirdFormE birdform = BirdFormE.none;

        public float initBirdVel;
        float pausedBirdVel;
        public float BirdVel;
        public float BirdFlyHeight = 1.2f;
        public BirdStateE BirdState;

        GameObject birdformgo=null;

        public bool isRunning() { return (BirdState == BirdStateE.running); }
        public bool isAtStart() { return (BirdState == BirdStateE.atstart); }
        public bool isAtGoal() { return (BirdState == BirdStateE.atgoal); }

        GameObject birdgo=null;


        BirdFormE BirdForm
        {
            set
            {
                birdform = value;
                RefreshGos();
            }
        }
        Path path = null;
        PathPos pathpos = null;
        public void SetBirdPath(Path path)
        {
            this.path = path;
            curptlc = Vector3.zero;
            rundist = 0;
            sbsAtStart();
        }
        public void PauseBird()
        {
            if (isRunning())
            {
                pausedBirdVel = BirdVel;
                BirdVel = 0;
                sbsPaused();
            }
        }
        public void UnPauseBird()
        {
            if (BirdState == BirdStateE.paused)
            {
                BirdVel = pausedBirdVel;
                if (BirdVel == 0) BirdVel = 1;
                sbsRunning();
            }
        }
        public bool sbsAtStart()
        {
            if (BirdState == BirdStateE.running)
            {
                initBirdVel = BirdVel;
                BirdVel = 0;
            }
            BirdState = BirdStateE.atstart;
            return true;
        }
        public bool sbsRunning()
        {
            BirdState = BirdStateE.running;
            return true;
        }
        public bool sbsPaused()
        {
            BirdState = BirdStateE.paused;
            return true;
        }
        public bool sbsAtGoal()
        {
            BirdState = BirdStateE.atgoal;
            return true;
        }
        Vector3 GetPathPoint(float gpprdist,bool setpathpos=true)
        {
            if (path == null) return Vector3.zero;
            var pp = path.MovePositionAlongPath(gpprdist);
            
            if (setpathpos)
            {
                pathpos = pp;
            }
            var ptlc = new Vector3(pp.pt.x, pp.pt.y + BirdFlyHeight, pp.pt.z);
            return (ptlc);
        }
        TextMesh tm = null;
        void addFloatingText(GameObject go, Vector3 pt, string text, string colorname, float yrot = 0)
        {
            if (rman != null && rman.garnish == RouterGarnishE.none) return;
            var ggo = new GameObject();
            ggo.transform.parent = go.transform;
            tm = ggo.AddComponent<TextMesh>();
            int linecount = text.Split('\n').Length;

            tm.anchor = TextAnchor.MiddleCenter;
            tm.text = text;
            tm.fontSize = 12;
            tm.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            if (yrot > 0)
            {
                tm.transform.Rotate(0, yrot, 0);
            }
            tm.transform.localPosition = pt + new Vector3(0f, linecount * 0.25f, 0.0f);
            tm.color = GraphUtil.getcolorbyname(colorname);
        }
        void assignTransform(Transform rhs)
        {
            if (rhs != null)
            {
                birdformgo.transform.localScale = rhs.localScale;
                birdformgo.transform.rotation = rhs.rotation;
                birdformgo.transform.position = rhs.position;
            }
        }
        void CreateBirdFormGos()
        {
            Transform pvt = null;
            if (birdformgo!=null)
            {
                pvt = birdformgo.transform;
                Destroy(birdformgo);
                birdformgo = null;
            }
            switch (birdform)
            {
                case BirdFormE.sphere:
                    {
                        var sphsize = 0.2f;
                        birdformgo = GraphUtil.CreateMarkerSphere("sphere", Vector3.zero, size: sphsize, clr: "yellow");
                        break;
                    }
                case BirdFormE.olive:
                    {
                        var sphsize = 0.2f;
                        birdformgo = GraphUtil.CreateMarkerSphere("sphere", Vector3.zero, size: sphsize, clr: "olive");
                        birdformgo.transform.localScale = new Vector3(0.2f, 0.2f, 0.28f);

                        var nosept = new Vector3(0, 0, 0.1f);
                        var gonose = GraphUtil.CreateMarkerSphere("nose", nosept, size: 0.1f, clr: "red");
                        gonose.transform.parent = birdformgo.transform;
                        break;
                    }
                default:
                case BirdFormE.hummingbird:
                    {
                        var objPrefab = (GameObject)Resources.Load("hummingbird");
                        birdformgo = Instantiate(objPrefab) as GameObject;
                        var s = 0.5e-3f;
                        var minscale = 0.1375f * s; // value experimentally discovered
                        if (s*rman.rgoScale < minscale)
                        {
                            s = minscale / rman.rgoScale;
                        }
                        birdformgo.transform.localScale = new Vector3(s, s, s);
                        break;
                    }
                case BirdFormE.utahteapot:
                    {
                        var objPrefab = (GameObject)Resources.Load("teapot");
                        birdformgo = Instantiate(objPrefab) as GameObject;
                        birdformgo.GetComponent<Renderer>().material.SetColor("_Color", GraphUtil.getcolorbyname("chinawhite"));
                        var s = 60.0f;
                        birdformgo.transform.localScale = new Vector3(s, s, s);
                        birdformgo.transform.Rotate(0, 90, 0);
                        break;
                    }
            }
            var pt = birdgo.transform.position;
            var text = "Hummingbird\n" + pt;
            addFloatingText(birdgo, pt, text, "yellow");
            birdformgo.transform.parent = birdgo.transform;
        }
      

        public void NextForm()
        {
            switch(birdform)
            {
                case BirdFormE.hummingbird:
                    birdform = BirdFormE.sphere;
                    break;
                case BirdFormE.sphere:
                    birdform = BirdFormE.olive;
                    break;
                case BirdFormE.olive:
                    birdform = BirdFormE.utahteapot;
                    break;
                case BirdFormE.utahteapot:
                    birdform = BirdFormE.hummingbird;
                    break;
            }
        }

        void CreateBirdGos()
        {
            if (path == null) return;

            birdgo = new GameObject("Bird");

            birdgo.transform.parent = rman.rgo.transform;
            CreateBirdFormGos();

            //RouteMan.Log("CreateBirdGos lp curpt:" + curpt);
        }
        void SetAtGoal()
        {
            if (BirdState == BirdStateE.atgoal) return; // we don't want to wipe out initBirdVel on 2nd call
            initBirdVel = BirdVel;
            BirdVel = 0;
            sbsAtGoal();
        }
        void MoveBirdGos(float deltatime)
        {
            if (BirdState == BirdStateE.running)
            {
                rundist += BirdVel * deltatime; // deltaTime is time to complete last frame
            }
            //var lastcurplc = curptlc;
            curptlc = GetPathPoint(rundist);
            //var delt = curptlc - lastcurplc;

            //curpt = GetPathPoint(rundist);
            var lookdist = rundist +System.Math.Min(2, System.Math.Max(1, BirdVel)*lookaheadtime);

            var wctrans = rman.rgo.transform;

            var curlookptlc = GetPathPoint(lookdist, setpathpos: false);
            var curlookptwc = wctrans.TransformPoint(curlookptlc);

            birdgo.transform.localPosition = curptlc; // note we are moving in local coordinates

            birdgo.transform.LookAt(curlookptwc);

            // stop bird if past point
            if (BirdState==BirdStateE.running && rundist >= path.pathLength)
            {
                RouteMan.Log("At Goal");
                SetAtGoal();
            } 
            //RouteMan.Log("MoveBirdGoes delt:" + delt + "  curpt:" + curpt + " lastcurpt:" + lastcurpt);

            if (tm!=null)
            {
                var pt = birdgo.transform.position;
                var cpt = Camera.main.transform.position;
                tm.text = BirdState+"   "+birdform +
                          "\nBird Pos:" + pt + 
                          "\nVel:"+BirdVel +  "  Height:"+BirdFlyHeight;
            }
            //GraphUtil.CreateMarkerSphere("lookat",  curlookpt, size: 0.1f * rman.rgoScale);
        }
        void DeleteBirdGos()
        {
            if(birdgo!=null)
            {
                Destroy(birdgo);
                birdgo = null;
            }
        }
        #region bird commands
        public void AdjustBirdHeight(float factor)
        {
            BirdFlyHeight *= factor;
        }
        public void RefreshGos()
        {
            DeleteBirdGos();
            CreateBirdGos();
        }
        public void ResetBirdToStartOfPath()
        {
            rundist = 0;
            MoveBirdGos(0);
            sbsAtStart();
        }
        public void StartBird()
        {
            BirdVel = initBirdVel;
            if (BirdVel==0)
            {
                BirdVel = 1; // this should not happen
            }
            sbsRunning();
            RouteMan.Log("StartBird called");
        }
        public void SetSpeed(float newspeed)
        {
            BirdVel = newspeed;
        }
        public void AdjustSpeed(float factor, float minspeed = 0)
        {
            BirdVel *= factor;
            if (BirdVel <= minspeed)
            {
                BirdVel = minspeed;
            }
        }
        public void DeleteBird()
        {
            if (birdgo != null)
            {
                // need to delete old bird form or it will hang around
                Destroy(birdgo);
                birdgo = null;
                //lastcurptlc = Vector3.zero;
            }
            var lastbirdform = birdform;
            initValues();
            if (lastbirdform != BirdFormE.none)
            {
                // have to do this becaues initValues sets the lastbirdform
                birdform = lastbirdform;
            }
        }
        public PathPos GetBirdPos()
        {
            // needed to punch a new link point when we stop bird
            return pathpos;
        }
        #endregion

        void initValues()
        {
            BirdVel = 0;
            BirdFlyHeight = 1.2f;
            lookaheadtime = 1.1f;
            initBirdVel = 1;
            rundist = 0;
            curptlc = Vector3.zero;
            birdform = BirdFormE.hummingbird;
            BirdState = BirdStateE.unbuilt;
      
            //RouteMan.Log("birdctrl initValues called");
        }
        void Start()
        {
            initValues();
            //RouteMan.Log("birdctrl starts called");
        }
        // Update is called once per frame
        public Vector3 debug_curposlc = Vector3.zero;
        public Vector3 debug_curposwc = Vector3.zero;
        void Update()
        {
            if (birdgo != null)
            {
                MoveBirdGos(Time.deltaTime);
                curptwc = rman.rgo.transform.TransformPoint(curptlc);
                debug_curposlc = birdgo.transform.localPosition;
                debug_curposwc = birdgo.transform.position;

            }
        }
    }
}
