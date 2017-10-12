using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

namespace BirdRouter
{
    public class SpatialMapperMan : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }
        GameObject smgo = null;
        GameObject GetSpatialMapper()
        {
            if (smgo != null) return smgo;
            smgo = GameObject.Find("Spatial Mapping");
            if (smgo == null)
            {
                RouteMan.Log("Could not find SpatialMapper GameObject");
                return null;
            }
            RouteMan.Log("Found SpatialMapper GameObject");
            Debug.Log("Found SpatialMapper GameObject");
            return smgo;
        }
        public void SetSpatialMapping(bool onoff)
        {
            var smgo = GetSpatialMapper();
            if (smgo == null) return;
           // var sm = smgo.GetComponent<SpatialMapping>();
            RouteMan.Log("Found Spatial Mapping Component");
            Debug.Log("Found Spatial Mapping Component");
          //  sm.MappingEnabled = onoff;
          //  sm.DrawVisualMeshes = onoff;
            RouteMan.Log("Spatial Mapping " + onoff);
            Debug.Log("Spatial Mapping " + onoff);
        }

        public void ChangeSpatialExtent(float val)
        {
            var smgo = GetSpatialMapper();
            var smr = smgo.GetComponent<SpatialMappingRenderer>();

            var hbe = smr.halfBoxExtents;
            var newhbe = hbe + new Vector3(val, val, val);
            smr.halfBoxExtents = newhbe;
            RouteMan.Log("Spatial Mapping halfBoxExtents set to :" + newhbe);
            Debug.Log("Spatial Mapping halfBoxExtents set to :" + newhbe);
        }
        public SpatialMappingBase.LODType NextLodVal(SpatialMappingBase.LODType oldlod, int incval)
        {
            var newlodp1 = oldlod;
            var newlodm1 = oldlod;
            switch (oldlod)
            {
                case SpatialMappingBase.LODType.High:
                    {
                        newlodm1 = SpatialMappingBase.LODType.Medium;
                        break;
                    }
                case SpatialMappingBase.LODType.Medium:
                    {
                        newlodp1 = SpatialMappingBase.LODType.High;
                        newlodm1 = SpatialMappingBase.LODType.Low;
                        break;
                    }
                case SpatialMappingBase.LODType.Low:
                    {
                        newlodp1 = SpatialMappingBase.LODType.Medium;
                        break;
                    }
            }

            var newlod = (incval < 0 ? newlodm1 : newlodp1);
            return newlod;
        }
        public void ChangeSpatialDetail(int val)
        {
            var smgo = GetSpatialMapper();
            var smr = smgo.GetComponent<UnityEngine.VR.WSA.SpatialMappingRenderer>();

            var lod = smr.lodType;
            var newlod = NextLodVal(lod, val);
            smr.lodType = newlod;
            RouteMan.Log("Spatial Mapping Lod Changed old:"+lod+"  new:"+newlod);
            Debug.Log("Spatial Mapping Lod Changed old:" + lod + "  new:" + newlod);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}