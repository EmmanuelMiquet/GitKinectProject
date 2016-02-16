using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using System;

namespace PL.Kinect
{
    class KinectTools
    {
        int flag = 0; //Necessary variables
        float t = 0;
        string time , coordinates;
        List<float> vect_t = new List<float>();
        List<float> Xj = new List<float>();
        List<float> Yj = new List<float>();
        List<float> Zj = new List<float>();
        List<List<float>> Xi = new List<List<float>>();
        List<List<float>> Yi = new List<List<float>>();
        List<List<float>> Zi = new List<List<float>>();
        
        List<JointType> AngleNeck = new List<JointType>(3);
        List<JointType> AngleSpine = new List<JointType>(3);
        List<JointType> AngleElbowRight = new List<JointType>(3);
        List<JointType> AngleShoulderRight = new List<JointType>(3);
        List<JointType> AngleKneeRight = new List<JointType>(3);
        List<JointType> AngleHipRight = new List<JointType>(3);
        List<JointType> AngleElbowLeft = new List<JointType>(3);
        List<JointType> AngleShoulderLeft = new List<JointType>(3);
        List<JointType> AngleKneeLeft = new List<JointType>(3);
        List<JointType> AngleHipLeft = new List<JointType>(3);

        List<List<JointType>> WantedAngles = new List<List<JointType>>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public KinectTools()
        {
            AngleNeck.Add(JointType.ShoulderCenter);
            AngleNeck.Add(JointType.Head);
            AngleNeck.Add(JointType.Spine);

            AngleSpine.Add(JointType.Spine);
            AngleSpine.Add(JointType.ShoulderCenter);
            AngleSpine.Add(JointType.HipCenter);
            
            AngleElbowRight.Add(JointType.ElbowRight);
            AngleElbowRight.Add(JointType.WristRight);
            AngleElbowRight.Add(JointType.ShoulderRight);

            AngleShoulderRight.Add(JointType.ShoulderRight);
            AngleShoulderRight.Add(JointType.ShoulderCenter);
            AngleShoulderRight.Add(JointType.ElbowRight);

            AngleKneeRight.Add(JointType.KneeRight);
            AngleKneeRight.Add(JointType.AnkleRight);
            AngleKneeRight.Add(JointType.HipRight);

            AngleHipRight.Add(JointType.HipRight);
            AngleHipRight.Add(JointType.HipCenter);
            AngleHipRight.Add(JointType.KneeRight);

            AngleElbowLeft.Add(JointType.ElbowLeft);
            AngleElbowLeft.Add(JointType.WristLeft);
            AngleElbowLeft.Add(JointType.ShoulderLeft);

            AngleShoulderLeft.Add(JointType.ShoulderLeft);
            AngleShoulderLeft.Add(JointType.ShoulderCenter);
            AngleShoulderLeft.Add(JointType.ElbowLeft);

            AngleKneeLeft.Add(JointType.KneeLeft);
            AngleKneeLeft.Add(JointType.AnkleLeft);
            AngleKneeLeft.Add(JointType.HipLeft);

            AngleHipLeft.Add(JointType.HipLeft);
            AngleHipLeft.Add(JointType.HipCenter);
            AngleHipLeft.Add(JointType.KneeLeft);

            WantedAngles.Add(AngleNeck);
            WantedAngles.Add(AngleSpine);
            WantedAngles.Add(AngleElbowRight);
            WantedAngles.Add(AngleShoulderRight);
            WantedAngles.Add(AngleKneeRight);
            WantedAngles.Add(AngleHipRight);
            WantedAngles.Add(AngleElbowLeft);
            WantedAngles.Add(AngleShoulderLeft);
            WantedAngles.Add(AngleKneeLeft);
            WantedAngles.Add(AngleHipLeft);
        }

        /// <summary>
        /// Extract skeleton points RAW potsiton to a file
        /// </summary>
        /// <param name="TAB">File in which we want to write</param>
        public void joints2fileRAW(TextWriter TAB)
        {
            if (vect_t.Count != 0)
            {
                for (int i = 0; i < vect_t.Count; i++)
                {
                    time = string.Format("{0:0.00000000}", vect_t[i]);
                    TAB.Write(time.Replace(",", ".")); //Go to next line and print t ("time")
                    for (int j = 0; j < Xi[i].Count; j++)
                    {
                        coordinates = coordinates + "\t" + string.Format("{0:0.00000000}", Xi[i][j]);
                        coordinates = coordinates + "\t" + string.Format("{0:0.00000000}", Yi[i][j]);
                        coordinates = coordinates + "\t" + string.Format("{0:0.00000000}", Zi[i][j]);
                    }
                    TAB.Write(coordinates.Replace(",", ".") + "\n");
                    coordinates = null;
                }
            }
        }

        /// <summary>
        /// Extract skeleton points RAW potsiton to several lists
        /// </summary>
        /// <param name="skeleton">Skeleton detected in the current frame</param>
        /// <param name="wantedJoints">List of wanted joints to put in the file</param>
        public void joints2list(Skeleton skeleton, List<JointType> wantedJoints)
        {
            foreach (Joint joint in skeleton.Joints) //For each joint in the current skeleton
            {
                foreach (JointType wanted in wantedJoints) //For each joint type that we want
                {
                    if (joint.JointType == wanted) //If current joint is wanted
                    {
                        if (flag == 0) //If it's the first time for this image
                        {
                            vect_t.Add(t / 30);
                            t++; //Increment "time"
                        }
                        if (joint.TrackingState == JointTrackingState.Tracked) //X Y Z if the joint is tracked
                        {
                            Xj.Add(joint.Position.X);
                            Yj.Add(joint.Position.Y);
                            Zj.Add(joint.Position.Z);
                        }
                        else if (joint.TrackingState == JointTrackingState.Inferred || joint.TrackingState == JointTrackingState.NotTracked) //0 0 0 if the joint is inferred or not tracked
                        {
                            Xj.Add(0);
                            Yj.Add(0);
                            Zj.Add(0);
                        }
                        flag++; //Increment the number 
                        if (flag == wantedJoints.Count) //If we have donne all the wanted joints of the current image
                        {
                            flag = 0; //Re-initialize number of wanted seen
                            Xi.Add(Xj.ToList());
                            Xj.Clear();
                            Yi.Add(Yj.ToList());
                            Yj.Clear();
                            Zi.Add(Zj.ToList());
                            Zj.Clear();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clean the data from the inferred or non tracked points to smooth processing
        /// </summary>
        /// <param name="nbPoints">Number of tracked points</param>
        private void cleanSingularity(int nbPoints)
        {
            int ndt = 0, k = 0;
            float dt = vect_t[1] - vect_t[0];

            for (int i = 0 ; i < nbPoints ; i++)
            {
                for (int j = 0 ; j < Xi.Count ; j++)
                {
                    if (Xi[j][i] == 0) //For X
                    {
                        for (k = j; k < Xi.Count; k++)
                        {
                            ndt++;
                            if (Xi[k][i] != 0)
                            {
                                break;
                            }
                        }
                        if (k != Xi.Count)
                        {
                            if (j == 0)
                            {
                                Xi[j][i] = Xi[k][i];
                            }
                            else
                            {
                                Xi[j][i] = ((Xi[k][i] - Xi[j - 1][i]) / ((ndt+1) * dt)) * dt + Xi[j - 1][i];
                            }
                        }
                        else
                        {
                            Xi[j][i] = Xi[j - 1][i];
                        }
                        ndt = 0;
                        k = 0;
                    }

                    if (Yi[j][i] == 0) //For Y
                    {
                        for (k = j; k < Yi.Count; k++)
                        {
                            ndt++;
                            if (Yi[k][i] != 0)
                            {
                                break;
                            }
                        }
                        if (k != Yi.Count)
                        {
                            if (j == 0)
                            {
                                Yi[j][i] = Yi[k][i];
                            }
                            else
                            {
                                Yi[j][i] = ((Yi[k][i] - Yi[j - 1][i]) / ((ndt+1) * dt)) * dt + Yi[j - 1][i];
                            }
                        }
                        else
                        {
                            Yi[j][i] = Yi[j - 1][i];
                        }
                        ndt = 0;
                        k = 0;
                    }

                    if (Zi[j][i] == 0) //For Z
                    {
                        for (k = j; k < Zi.Count; k++)
                        {
                            ndt++;
                            if (Zi[k][i] != 0)
                            {
                                break;
                            }
                        }
                        if (k != Zi.Count)
                        {
                            if (j == 0)
                            {
                                Zi[j][i] = Zi[k][i];
                            }
                            else
                            {
                                Zi[j][i] = ((Zi[k][i] - Zi[j - 1][i]) / ((ndt+1) * dt)) * dt + Zi[j - 1][i];
                            }
                        }
                        else
                        {
                            Zi[j][i] = Zi[j - 1][i];
                        }
                        ndt = 0;
                        k = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Extract skeleton points processed potsiton to a file
        /// </summary>
        /// <param name="TAB">File in which we want to write</param>
        public void joints2file(TextWriter TAB)
        {
            if (vect_t.Count != 0)
            {
                cleanSingularity(Xi[0].Count);
                for (int i = 0; i < vect_t.Count; i++)
                {
                    time = string.Format("{0:0.00000000}", vect_t[i]);
                    TAB.Write(time.Replace(",", ".")); //Go to next line and print t ("time")
                    for (int j = 0; j < Xi[i].Count; j++)
                    {
                        coordinates = coordinates + "\t" + string.Format("{0:0.00000000}", Xi[i][j]);
                        coordinates = coordinates + "\t" + string.Format("{0:0.00000000}", Yi[i][j]);
                        coordinates = coordinates + "\t" + string.Format("{0:0.00000000}", Zi[i][j]);
                    }
                    TAB.Write(coordinates.Replace(",", ".") + "\n");
                    coordinates = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wantedJoints"></param>
        private void initWantedAngles(string anglesLegendPath, List<JointType> wantedJoints)
        {
            List<List<JointType>> toRemove = new List<List<JointType>>();

            TextWriter anglesLegendFile = new StreamWriter(anglesLegendPath);

            foreach (List<JointType> triplet in WantedAngles)
            {
                foreach(JointType joint in triplet)
                {
                    if(!wantedJoints.Contains(joint))
                    {
                        toRemove.Add(triplet);
                        break;
                    }
                }
            }

            foreach(List<JointType> i in toRemove)
            {
                WantedAngles.Remove(i);
            }

            foreach(List<JointType> triplet in WantedAngles)
            {
                anglesLegendFile.WriteLine((triplet[0].ToString() + " - " + triplet[1].ToString() + " - " + triplet[2].ToString()).PadRight(48));
            }

            anglesLegendFile.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonesLength"></param>
        /// <param name="wantedJoints"></param>
        private void bonesLength(string bonesLengthPath, List<JointType> wantedJoints)
        {
            TextWriter bonesLengthFile = new StreamWriter(bonesLengthPath);

            double length1;
            double length2;
            
            foreach(List<JointType> triplet in WantedAngles)
            {
                length1 = lengthEvaluation(triplet[0],triplet[1],wantedJoints);
                length2 = lengthEvaluation(triplet[0],triplet[2],wantedJoints);

                bonesLengthFile.Write((triplet[0].ToString() + " - " + triplet[1].ToString() + " - " + triplet[2].ToString()).PadRight(48));

                bonesLengthFile.Write("\t" + (triplet[0].ToString() + " - " + triplet[1].ToString()).PadRight(31) + "\t" + length1.ToString().PadRight(16, '0'));

                bonesLengthFile.WriteLine("\t" + (triplet[0].ToString() + " - " + triplet[2].ToString()).PadRight(31) + "\t" + length2.ToString().PadRight(16, '0'));
            }

            bonesLengthFile.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="extremity"></param>
        private double lengthEvaluation(JointType origin, JointType extremity, List<JointType> wantedJoints)
        {
            if (wantedJoints.Contains(origin) && wantedJoints.Contains(extremity))
            {
                double length = 0;

                int i = 0;
                int j = wantedJoints.IndexOf(origin);
                int k = wantedJoints.IndexOf(extremity);

                for(i = 0 ; i < vect_t.Count ; i++)
                {
                    length += norm2(new double[] {(Xi[i][j]-Xi[i][k]), (Yi[i][j] - Yi[i][k]), (Zi[i][j] - Zi[i][k])});
                }
                length /= i;

                return length;
            }
            else
            {
                Console.WriteLine("ERROR in lengthEvaluation ! Joint not tracked !");
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anglesLegendPath"></param>
        /// <param name="bonesLengthPath"></param>
        /// <param name="anglesDataPath"></param>
        /// <param name="wantedJoints"></param>
        public void manageAngles(string anglesLegendPath, string bonesLengthPath, string anglesDataPath, List<JointType> wantedJoints)
        {
            TextWriter anglesDataFile = new StreamWriter(anglesDataPath);

            initWantedAngles(anglesLegendPath, wantedJoints);
            bonesLength(bonesLengthPath, wantedJoints);

            for(int i = 0 ; i < vect_t.Count ; i++)
            {
                anglesDataFile.Write(string.Format("{0:0.00000000}", vect_t[i]) + "\t");

                foreach(List<JointType> triplet in WantedAngles)
                {
                    anglesDataFile.Write(jointAngle(new double[] {
                        (Xi[i][wantedJoints.IndexOf(triplet[1])] - Xi[i][wantedJoints.IndexOf(triplet[0])]),
                        (Yi[i][wantedJoints.IndexOf(triplet[1])] - Yi[i][wantedJoints.IndexOf(triplet[0])]),
                        (Zi[i][wantedJoints.IndexOf(triplet[1])] - Zi[i][wantedJoints.IndexOf(triplet[0])])},
                                                    new double[] {
                        (Xi[i][wantedJoints.IndexOf(triplet[2])] - Xi[i][wantedJoints.IndexOf(triplet[0])]),
                        (Yi[i][wantedJoints.IndexOf(triplet[2])] - Yi[i][wantedJoints.IndexOf(triplet[0])]),
                        (Zi[i][wantedJoints.IndexOf(triplet[2])] - Zi[i][wantedJoints.IndexOf(triplet[0])])})
                        .ToString().PadRight(16,'0') + "\t");
                }

                anglesDataFile.WriteLine();
            }

            anglesDataFile.Close();
        }

        /// <summary>
        /// Give RGB values of required pixel in image
        /// </summary>
        /// <param name="imageFrame">Image color frame</param>
        /// <param name="x">X-coordonate in the image (between 0 and imageFrame.Height-1)</param>
        /// <param name="y">Y-coordonate in the image (between 0 and imageFrame.Width-1)</param>
        /// <returns>Array containing red, green and blue values of selected pixel</returns>
        public byte[] getRGBValue(ColorImageFrame imageFrame, int x, int y)
        {
            byte[] pixelData = new byte[imageFrame.PixelDataLength];
            imageFrame.CopyPixelDataTo(pixelData);

            byte[] RGBValue = new byte[3];

            RGBValue[2] = pixelData[(x + y * imageFrame.Width) * 4];
            RGBValue[1] = pixelData[(x + y * imageFrame.Width) * 4 + 1];
            RGBValue[0] = pixelData[(x + y * imageFrame.Width) * 4 + 2];

            return RGBValue;
        }

        /// <summary>
        /// Give depth value of required pixel in image
        /// </summary>
        /// <param name="imageFrame">Image depth frame</param>
        /// <param name="x">X-coordonate in the image (between 0 and imageFrame.Height-1)</param>
        /// <param name="y">Y-coordonate in the image (between 0 and imageFrame.Width-1)</param>
        /// <returns>Int containing depth of selected pixel</returns>
        public int getDepthValue(DepthImageFrame imageFrame, int x, int y)
        {
            short[] pixelData = new short[imageFrame.PixelDataLength];
            imageFrame.CopyPixelDataTo(pixelData);
            return ((ushort)pixelData[x + y * imageFrame.Width]) >> 3;
        }

        /// <summary>
        /// Calculate angle between three joints
        /// </summary>
        /// <param name="middleJoint">Center joint of the angle</param>
        /// <param name="connectedJoint1">Joint at one extremity of a bone linked to center joint</param>
        /// <param name="connectedJoint2">Joint at one extremity of another bone linked to center joint</param>
        /// <returns>Double value of the angle in degrees</returns>
        private double jointAngle(Joint middleJoint, Joint connectedJoint1, Joint connectedJoint2)
        {
            double[] vector1 = new double[3] { connectedJoint1.Position.X - middleJoint.Position.X, connectedJoint1.Position.Y - middleJoint.Position.Y, connectedJoint1.Position.Z - middleJoint.Position.Z };
            double[] vector2 = new double[3] { connectedJoint2.Position.X - middleJoint.Position.X, connectedJoint2.Position.Y - middleJoint.Position.Y, connectedJoint2.Position.Z - middleJoint.Position.Z };
            
            return (360 * Math.Acos(scalarProduct(vector1,vector2) / (norm2(vector1)*norm2(vector2))) / (2 * Math.PI));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        private double jointAngle(double[] vector1, double[]vector2)
        {
            return (360 * Math.Acos(scalarProduct(vector1, vector2) / (norm2(vector1) * norm2(vector2))) / (2 * Math.PI));
        }

        /// <summary>
        /// Calculate scalar product of of two vectors (must be the same dimension)
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second vector</param>
        /// <returns>Scalar result</returns>
        public double scalarProduct(double[] vector1, double[] vector2)
        {
            double result = 0;

            if(vector1.Length != vector2.Length)
            {
                System.Console.WriteLine("ERROR : Trying to process scalar product of vectors of different dimensions !");
                return 0;
            }

            for(int i = 0 ; i < vector1.Length ; i++)
            {
                result += (vector1[i]*vector2[i]);
            }
            return result;
        }

        /// <summary>
        /// Calculate the eucilian norm of a vector
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns>Norm</returns>
        public double norm2(double[] vector)
        {
            return Math.Sqrt(scalarProduct(vector,vector));
        }
    }
}
