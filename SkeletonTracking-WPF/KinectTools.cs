using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;

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
    }
}
