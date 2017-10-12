using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphAlgos;

namespace BirdRouter
{
    public class FloorPlanCtrl : MonoBehaviour
    {

        private RouteMan rman;
        public void setRouteMan(RouteMan rman)
        {
            // this is attached as a component, thus we cannot set it in a contructor
            this.rman = rman;
            var gt = new graphtex();
            var sca = new Vector3(7, 1, 9.76f);
            var rot = new Vector3(0, -90, 0);
            var trn = new Vector3(38.55f, 0, -27.79f);
            gt.SetMaterialPlane("RedwestBfloor3", 676,938,sca, rot, trn);
            this.gt = gt;
        }

        public bool visible = false;


        GameObject flplgo = null;
        graphtex gt = null;

        public void setGraphtex(graphtex gt)
        {
            // this is attached as a component, thus we cannot set it in a contructor
            this.gt = gt;
        }
        Material GetMaterial(GameObject go,string matname)
        {
            var wholename = "FloorPlans/" + gt.materialName;
            var matt = (Material)Resources.Load(wholename);
            return matt;
        }

        void CreateGos()
        {
            DeleteGos(); // delete any old floor plan gos before we redraw the new ones
           // Debug.Log("Creating floorplan");
            if (!visible) return; // nothing to do

            if (flplgo == null)
            {
                flplgo = new GameObject("FloorPlan");
                flplgo.transform.parent = rman.rgo.transform;
            }
            var pln = GameObject.CreatePrimitive(PrimitiveType.Plane);
            pln.name = "bitmapframeplane";

            pln.GetComponent<Renderer>().material = GetMaterial(pln,gt.materialName);
            pln.transform.localScale = gt.scale;
            pln.transform.Rotate(gt.rotate);
            pln.transform.localPosition = gt.translate;

            pln.transform.parent = flplgo.transform;
        }
        void DeleteGos()
        {
            if (flplgo != null)
            {
                Destroy(flplgo);
                flplgo = null;
            }
        }
        public void RefreshGos()
        {
            DeleteGos();
            CreateGos();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}