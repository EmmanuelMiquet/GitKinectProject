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
        /// <param name="targetJoint">Center joint of the angle</param>
        /// <param name="connectedJoint1">Joint at one extremity of a bone linked to center joint</param>
        /// <param name="connectedJoint2">Joint at one extremity of another bone linked to center joint</param>
        /// <returns>Double value of the angle in degrees</returns>
        public double jointAngle(Joint targetJoint, Joint connectedJoint1, Joint connectedJoint2)
        {
            double[] vector1 = new double[3] { connectedJoint1.Position.X - targetJoint.Position.X, connectedJoint1.Position.Y - targetJoint.Position.Y, connectedJoint1.Position.Z - targetJoint.Position.Z };
            double[] vector2 = new double[3] { connectedJoint2.Position.X - targetJoint.Position.X, connectedJoint2.Position.Y - targetJoint.Position.Y, connectedJoint2.Position.Z - targetJoint.Position.Z };

            double scalarProduct = vector1[0] * vector2[0] + vector1[1] * vector2[1] + vector1[2] * vector2[2];

            double norm1 = Math.Sqrt(vector1[0] * vector1[0] + vector1[1] * vector1[1] + vector1[2] * vector1[2]);
            double norm2 = Math.Sqrt(vector2[0] * vector2[0] + vector2[1] * vector2[1] + vector2[2] * vector2[2]);

            return (360 * Math.Acos((scalarProduct / (norm1 * norm2)))) / (2 * Math.PI);
        }
    }
}
