using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WinFormBlankTest
{
    public class SNTest
    {
        string lotid;
        string rollid;
        int snid;
        //double homo_tube_averages;
        //double tube_averages_nano_amper;
        //double limitOfHomogenity;
        //double limitOfBlank;
        double differenceHomogenity;
        //double DifferenceBlank;
        //public double avgOfTube;

        public SNTest(string lot,
                        string roll, string sn,
               //       double tube_averages,double limitHomogenity,
           double diffHomogemnity)
        {
            lotid = lot;
            rollid = roll;
            snid = Convert.ToInt32(sn);
            //avgOfTube = tube_averages;
            //limitOfHomogenity = limitHomogenity;
             differenceHomogenity=diffHomogemnity;
            
           

        }


        //public double TubeAVG
        //{
        //    get
        //    {
        //        return Math.Round(avgOfTube,4);
        //    }
        //    set { avgOfTube = value; }
        //}

        public double DiffFromHomogenityAVG
        {
            get
            {
                return Math.Abs(Math.Round(differenceHomogenity,4));
            }
            set { differenceHomogenity = value; }
        }
        //public double DiffFromBlankAVG
        //{
        //    get
        //    {
        //        return Math.Round(DifferenceBlank);
        //    }
        //    set { DifferenceBlank = value; }
        //}
        public string CentralLot
        {
            get
            {
                return lotid;
            }
            set { this.lotid= value; }
        }
        public string CentralRoll
        {
            get
            {
                return rollid;
            }
            set { this.rollid= value; }
        }
        public int CentralSN
        {
            get
            {
                return snid;
            }
            set { this.snid = value; }
        }
        //public double TubeAveragesGlus
        //{
        //    get
        //    {
        //        return homo_tube_averages;
        //    }
        //    set { this.homo_tube_averages = value; }
        //}
        //public double TubeAveragesNanoAmper
        //{
        //    get
        //    {
        //        return tube_averages_nano_amper;
        //    }
        //    set { this.tube_averages_nano_amper = value; }
        //}
        //public double AVGLimit_Blank
        //{
        //    get
        //    {
        //        return limitOfBlank;
        //    }
        //    set { this.limitOfBlank = value; }
        //}
        //public double AVGLimit_Homogenity
        //{
        //    get
        //    {
        //        return limitOfHomogenity;
        //    }
        //    set { this.limitOfHomogenity = value; }
        //}
    }
}
