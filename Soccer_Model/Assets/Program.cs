using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace project
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils util = new Utils();
            List<LegModel> legModels = util.getRamdomLegs();
            Console.WriteLine("before");
            Console.Write(util.toString(legModels));

            var eliteModels = util.selectElite(legModels);
            var newModels = util.crossModels(eliteModels);

            Console.WriteLine("elite");
            Console.Write(util.toString(newModels));
        }
    }

    class LegModel
    {
        public int DNALength = 10;
        public static double MinTorque = -50;
        public static double MaxTorque = 50;

        public List<double> DNA { get; set; }
        System.Random random = new System.Random();
        public double Result { get; set; }

        public LegModel ()
        {
            DNA = new List<double>();
            Result = 0;
        }

        public String toString() {
            return "res: " + Result + ", [" + string.Join(", ", DNA) + "]";
        }

        public void setRandDNA()
        {
            int i;
            for (i=0; i<DNALength; i++)
                DNA.Add(random.Next((int)MinTorque, (int)MaxTorque));
        }

        public void setRandReslut()
        {
            Result = random.Next(0, 100);
        }
    }

    class Utils {
        System.Random random = new System.Random();
        public double eliteRate = 0.2;
        public int modelNum = 100;
        public int DNALength = 10;

        public String toString(List<LegModel> legModels) {
            String str = "";
            str += "------------------------------------------------\n";
            int i= 0;
            foreach (LegModel leg in legModels) {
                i++;
                str += leg.toString() + "\n";
            }
            str += "------------------------------------------------\n";

            return str;
        }

        public List<LegModel> getRamdomLegs() 
        {
            List<LegModel> legModels = new List<LegModel>();
            for(int i=0; i<modelNum; i++) {
                LegModel tmpLeg = new LegModel();
                tmpLeg.setRandDNA();
                tmpLeg.setRandReslut();
                legModels.Add(tmpLeg);
            }

            return legModels;
        }

        public int compareLegs(LegModel a, LegModel b, Boolean reverse)
        {
            int m = 1;
            if (reverse)
                m = -1;

            if (a.Result - b.Result < 0)
                return -1 * m;
            else if (a.Result - b.Result >0)
                return 1 * m;
            else 
                return 0;
        }

        public List<LegModel> sortModels(List<LegModel> legModels)
        {
            List<LegModel> tmpList = legModels;
            tmpList.Sort((a, b) => compareLegs(a, b, true));
            return tmpList;
        }

        public List<LegModel> selectElite(List<LegModel> legModels) {
            List<LegModel> eliteModels = new List<LegModel>();
            List<LegModel>  tmpModels = sortModels(legModels);
            for(int i=0; i<eliteRate*modelNum; i++) {
                eliteModels.Add(tmpModels[i]);
            }

            return eliteModels;
        }

        public List<LegModel> crossModels(List<LegModel> eliteModels)
        {
            double epsilon = Math.Sqrt(DNALength + 2);
            List<LegModel> newModels = new List<LegModel>();

            for(int i=0; i<eliteRate*2*modelNum; i++) {
                LegModel newModel = new LegModel();
                newModel.DNA = new List<double>();
                for (int j=0; j<DNALength; j++)
                {
                    newModel.DNA.Add(0);
                }
                newModels.Add(newModel);
            }

            // legModels = newModels;
            // Console.Write(this.toString());

            for(int i=0; i<eliteRate*modelNum; i++) {
                List<double> DNA1 = eliteModels[i].DNA;
                List<double> resDNA1 = newModels[i].DNA;
                List<double> DNA2;
                List<double> DNA3;
                List<double> resDNA2;
                List<double> resDNA3;

                if (i+1 >= eliteModels.Count) {
                    DNA2 = eliteModels[0].DNA;
                    DNA3 = eliteModels[1].DNA;
                    resDNA2 = newModels[0].DNA;
                    resDNA3 = newModels[1].DNA;
                } else if (i+2 >= eliteModels.Count) {
                    DNA2 = eliteModels[i+1].DNA;
                    DNA3 = eliteModels[0].DNA;
                    resDNA2 = newModels[i+1].DNA;
                    resDNA3 = newModels[0].DNA;
                } else {
                    DNA2 = eliteModels[i+1].DNA;
                    DNA3 = eliteModels[i+2].DNA;
                    resDNA2 = newModels[i+1].DNA;
                    resDNA3 = newModels[i+2].DNA;
                }

                List<double> gPointList = new List<double>();

                for (int j=0; j<DNALength; j++)
                {
                    gPointList.Add((DNA1[j] + DNA2[j] + DNA3[j]) / 3.0);
                }

                List<double> sPointList1 = new List<double>();
                List<double> sPointList2 = new List<double>();
                List<double> sPointList3 = new List<double>();

                for (int j=0; j<DNALength; j++)
                {
                    sPointList1.Add(gPointList[j] + epsilon * (DNA1[j] - gPointList[j]));
                    sPointList2.Add(gPointList[j] + epsilon * (DNA2[j] - gPointList[j]));
                    sPointList3.Add(gPointList[j] + epsilon * (DNA3[j] - gPointList[j]));
                }

                for (int j=0; j<DNALength; j++)
                {
                    double r1 = random.NextDouble();
                    var r2 = Math.Pow(random.NextDouble(), 0.5f);

                    resDNA1[j] = sPointList1[j];
                    resDNA2[j] = r1 * (sPointList1[j] - sPointList2[j]) + sPointList2[j];
                    resDNA3[j] = r2 * (sPointList2[j] - sPointList3[j] + r1 * (sPointList1[j] - sPointList2[j]) + sPointList3[j]);
                }
                
            }
        
            foreach (LegModel legModel in newModels) {
                for (int i=0; i<DNALength; i++) {
                    legModel.DNA[i] = Mathf.Clamp((float)legModel.DNA[i], (float)LegModel.MinTorque, (float)LegModel.MaxTorque);
                }
            }

            return newModels;
        }
    }
}

