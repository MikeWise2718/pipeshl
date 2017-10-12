using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


// Gradient Decent Optimizer

namespace GraphAlgos
{
    public enum optTypeSelectorE { rotY, transXYZ, rotYtransXYZ }
    public enum optStatusE { uninitialized,initialized, converged, didnotconverge }

    public class optimAnchorPoint
    {
        public string name;
        public float pathlength;
        public Vector3 source;
        public Vector3 target;
        public optimAnchorPoint(string name, Vector3 source, Vector3 target,float pathlength=0)
        {
            this.name = name;
            this.source = source;
            this.target = target;
            this.pathlength = pathlength;
        }
    }
    public class oapOptimizer
    {
        optTypeSelectorE mode;
        LinkedList<optimAnchorPoint> oaplist = null;
        public optStatusE status;
        public int iter;
        int npars;
        float[] curpars;
        float[] lstpars;
        float[] bstpars;
        public float bstval;
        public float bstiter;

        float convergenceEps = 1e-6f;
        float stepSize = 0.01f;
        int maxiter = 100;
        int nInitIters = 1024;
        public enum verbosityE { quiet, info, verbose }
        public verbosityE verbosity = verbosityE.info;
        public int fcalled;
        public float ferr;

        public oapOptimizer(optTypeSelectorE mode)
        {
            this.mode = mode;
            oaplist = new LinkedList<optimAnchorPoint>();
            switch (mode)
            {
                case optTypeSelectorE.rotY:
                    npars = 1;
                    break;
                case optTypeSelectorE.transXYZ:
                    npars = 3;
                    break;
                case optTypeSelectorE.rotYtransXYZ:
                    npars = 4;
                    break;
            }
            status = optStatusE.uninitialized;
        }
        public void init()
        {
            if (oaplist.Count == 0)
            {
                throw new Exception("No anchor points to initialize on");
            }
            curpars = new float[npars];
            bstpars = new float[npars];
            lstpars = new float[npars];
            bstval = 9e30f;
            iter = -2;
            var v = F(curpars);
            status = optStatusE.initialized;
            randomSamplePhase();
        }
        public void randomSamplePhase()
        {
            iter = -1;
            float minrange = -0.1f;
            float maxrange = 0.1f;
            var befbestval = bstval;
            // do
            for (int i=0; i< nInitIters; i++)
            {
                for (int ip=0; ip<npars; ip++)
                {
                    curpars[ip] = GraphUtil.NextFloat(minrange, maxrange);
                }
                F(curpars);
            }
            Debug.Log("randomSamplePhase bef:" + befbestval + "  aft:" + bstval);
        }
        public void addOap(optimAnchorPoint oap)
        {
            oaplist.AddLast(oap);
            if (verbosity==verbosityE.verbose)
            {
                GraphUtil.Log("addOap  source:" + oap.source.ToString("F4"));
                GraphUtil.Log("addOap  target:" + oap.target.ToString("F4"));
                Debug.Log("addOap  source:" + oap.source.ToString("F4"));
                Debug.Log("addOap  target:" + oap.target.ToString("F4"));
            }
        }
        public void addOapList(LinkedList<optimAnchorPoint> oaplist)
        {
            foreach (var oap in oaplist)
            {
                addOap(oap);
            }
        }
        public Vector3 rotvek = new Vector3(0, 0, 0);
        public Vector3 trnvek = new Vector3(0, 0, 0);
        public Vector3 scavek = new Vector3(1, 1, 1);

        void unpackIntoF(float[] pars)
        {
            switch (mode)
            {
                case optTypeSelectorE.rotY:
                    rotvek.y = pars[0];
                    break;
                case optTypeSelectorE.transXYZ:
                    trnvek.x = pars[0];
                    trnvek.y = pars[1];
                    trnvek.z = pars[2];
                    break;
                case optTypeSelectorE.rotYtransXYZ:
                    rotvek.y = pars[0];
                    trnvek.x = pars[1];
                    trnvek.y = pars[2];
                    trnvek.z = pars[3];
                    break;
            }
            // Normalize the angle
            rotvek.y = normAngle(rotvek.y);
        }
        public float normAngle(float angin)
        {
            const float pi = Mathf.PI;
            const float pi2 = 2 * Mathf.PI;
            int niter = 0;
            float ang = angin;
            while( niter<100 && (ang < -pi))
            {
                ang += pi2;
                niter += 1;
            }
            niter = 0;
            while (niter<100 && (pi < ang))
            {
                ang -= pi2;
                niter += 1;
            }
            return (ang);
        }
        public static Vector3 rotateY(float rad, Vector3 v1)
        {
            var s = Mathf.Sin(rad);
            var c = Mathf.Cos(rad);
            var x =  c*v1.x + s*v1.z;
            var z = -s*v1.x + c*v1.z;
            return (new Vector3(x, v1.y, z));
        }
        void packcurpars()
        {

            switch (mode)
            {
                case optTypeSelectorE.rotY:
                    curpars[0] = rotvek.y;
                    break;
                case optTypeSelectorE.transXYZ:
                    curpars[0] = trnvek.x;
                    curpars[1] = trnvek.y;
                    curpars[2] = trnvek.z;
                    break;
                case optTypeSelectorE.rotYtransXYZ:
                    curpars[0] = rotvek.y;
                    curpars[1] = trnvek.x;
                    curpars[2] = trnvek.y;
                    curpars[3] = trnvek.z;
                    break;
            }
        }
        float F(optimAnchorPoint oap)
        {
            var v0 = oap.source;

            // scale
            var v1 = new Vector3(v0.x*scavek.x, v0.y*scavek.y, v0.z*scavek.z);
            // rotate (just y for now)
            var v2 = rotateY(rotvek.y, v1);

            // translate
            var v3 = v2 + trnvek;

            var dt = v3 - oap.target;
            float err = dt.x*dt.x + dt.y*dt.y + dt.z*dt.z;
 
            return (err);
        }

        float F(float[] pars)
        {
            fcalled += 1;
            unpackIntoF(pars); // this makes F use pars
            float serrsum = 0;
            var oap = oaplist.First;
            while (oap != null)
            {
                serrsum += F(oap.Value);
                oap = oap.Next;
            }
            if (serrsum < bstval)
            {
              //  Debug.Log("bstval assigned old:" + bstval+" new:"+serrsum);
                bstval = serrsum;
                bstpars = curpars;
                bstiter = iter;
            }
            return (serrsum);
        }
        float[] peturbcurpars(int idx, float perturbsize, int dir)
        {
            var rv = new float[npars];
            for (int i = 0; i < npars; i++)
            {
                rv[i] = curpars[i];
                if (i == idx)
                {
                    rv[i] += dir*perturbsize;
                }
            }
            return (rv);
        }
        public bool converged()
        {
            // check curpar to lstpar distance less than convergenceEps
            if (iter == 0) return (false);
            float err = 0;
            for (int ip = 0; ip < npars; ip++)
            {
                var dlt = (curpars[ip] - lstpars[ip]);
                err += dlt*dlt;
            }
            ferr = Mathf.Sqrt(err);
            if (verbosity==verbosityE.verbose)
            {
                GraphUtil.Log("iter:" + iter + " err:" + ferr);
                Debug.Log("iter:" + iter + " err:" + ferr);
            }
            return (ferr < convergenceEps);
        }
        public float[] calcGradPoint(float[] pars, float a, float[] grad)
        {
            var newpars = new float[npars];
            for (int ip = 0; ip < npars; ip++)
            {
                newpars[ip] = pars[ip] - a*grad[ip];
            }
            return (newpars);
        }
        public float findBestA(float[] pars, float[] grad)
        {
            int maxn = 64;
            var maxa = 4.0f / npars;
            float rv;
            if (iter <= 6) // 6 worked well once
            {
                rv = findBestABruteForce(maxn, maxa, pars, grad);
            }
            else
            {
                rv = findBestAdivideInterval(maxa, pars, grad);
            }
            return (rv);
        }
        public float findBestAdivideInterval(float maxa, float[] pars, float[] grad)
        {
            var hia = maxa;
            var hipar = calcGradPoint(pars, hia, grad);
            var hif = F(hipar);
            var loa = 0.0f;
            var lopar = calcGradPoint(pars, loa, grad);
            var lof = F(lopar);

            while (Math.Abs(hia - loa) / maxa > 0.0001)
            {
                var nea = (hia + loa) / 2;
                var nepar = calcGradPoint(pars, nea, grad);
                var nef = F(nepar);
                if (hif > lof)
                {
                    hia = nea;
                    hipar = nepar;
                    hif = nef;
                }
                else
                {
                    loa = nea;
                    lopar = nepar;
                    lof = nef;
                }
            }
            var rv = loa;
            if (hif < lof)
            {
                rv = hia;
            }
            return (rv);
        }
        public float findBestABruteForce(int maxn, float maxa, float[] pars, float[] grad)
        {
            var lsta = maxa;
            var lstminpars = calcGradPoint(pars, lsta, grad);
            var lstminval = F(lstminpars);
            var lstn = maxn;
            for (int n = 1; n < maxn; n++) // has to be greater than zero or it can get stuck
            {
                // back up until the values start getting bigger again
                var nxta = (((float)n) / maxn)*maxa;
                var nxtminpars = calcGradPoint(pars, nxta, grad);
                var nxtminval = F(nxtminpars);
                if (nxtminval < lstminval)
                {
                    lsta = nxta;
                    lstminpars = nxtminpars;
                    lstminval = nxtminval;
                    lstn = n;
                }
            }
            if (verbosity==verbosityE.verbose)
            {
                GraphUtil.Log("   besta:" + lsta + " lstn:" + lstn + " F:" + lstminval);
                Debug.Log("   besta:" + lsta + " lstn:" + lstn + " F:" + lstminval);
            }
            return (lsta);
        }
        public void optimize()
        {
            curpars = new float[npars];
            lstpars = new float[npars];

            fcalled = 0;
            iter = 0;
            if (verbosity == verbosityE.verbose)
            {
                GraphUtil.Log("Starting Optim - Oap num:" + oaplist.Count + " npars:" + npars);
                Debug.Log("Starting Optim - Oap num:" + oaplist.Count + " npars:" + npars);
            }
            while (!converged() && iter < maxiter)
            {
                // calculate gradient at curpnt
                var baseval = F(curpars);
                var grad = new float[npars];
                if (verbosity == verbosityE.verbose)
                {
                    GraphUtil.Log("baseval:" + baseval+ " grad:"+grad);
                    Debug.Log("baseval:" + baseval + " grad:" + grad);
                }
                for (int ip = 0; ip < npars; ip++)
                {
                    var pparsP = peturbcurpars(ip, stepSize, +1);
                    var pparsM = peturbcurpars(ip, stepSize, -1);
                    var valP = F(pparsP);
                    var valM = F(pparsM);
                    grad[ip] = (valP - valM) / (2*stepSize);
                    var c = curpars[ip];
                    var l = lstpars[ip];
                    var g = grad[ip];
                    if (verbosity == verbosityE.verbose)
                    {
                        GraphUtil.Log("    par:" + ip + " l:" + l + " c:" + c + "  valP:" + valP + "  valM:" + valM + " g:" + g);
                        Debug.Log("    par:" + ip + " l:" + l + " c:" + c + "  valP:" + valP + "  valM:" + valM + " g:" + g);
                    }
                }
                var a = findBestA(curpars, grad);
                var nxtpars = calcGradPoint(curpars, a, grad);

                lstpars = curpars;
                curpars = nxtpars;
                iter += 1;
            }
            if (iter >= 100)
            {
                status = optStatusE.didnotconverge;
                GraphUtil.Log("Did not converge");
                Debug.Log("Did not converge");
            }
            else
            {
                status = optStatusE.converged;
            }
            // optimized value is now in curpars
            //unpackIntoF(curpars);
            unpackIntoF(bstpars);
            if (verbosity == verbosityE.info)
            {
                GraphUtil.Log("Optimized status" + status + "  iter:" + iter + "  fcalled:" + fcalled);
                Debug.Log("Optimized status" + status + "  iter:" + iter + "  fcalled:" + fcalled);
            }
        }
    }


}