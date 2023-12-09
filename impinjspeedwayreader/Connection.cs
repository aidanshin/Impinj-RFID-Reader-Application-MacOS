using System;
using System.IO;
using System.Threading;
using System.Diagnostics;

using System.Globalization;
using Foundation;
using Impinj.OctaneSdk;
using AppKit;
using CsvHelper;
using System.Collections.Generic;

namespace impinjspeedwayreader
{
    public class Connection
    {

        public Connection()
        {

        }
        //public static StreamWriter sw = new StreamWriter("Test.txt");
        public static void ConnectToReader()
        {
            try
            {
                Console.WriteLine("Attempting to connect to {0} ({1}).",
                    GlobalVariable.reader.Name, GlobalVariable.Host_IP);

                // Number of milliseconds before a 
                // connection attempt times out.
                GlobalVariable.reader.ConnectTimeout = 6000;
                // Connect to the reader.
                // Change the ReaderHostname constant in SolutionConstants.cs 
                // to the IP address or hostname of your reader.

                GlobalVariable.reader.Connect(GlobalVariable.Host_IP);
                Console.WriteLine("Successfully connected.");

                // Tell the reader to send us any tag reports and 
                // events we missed while we were disconnected.
                GlobalVariable.reader.ResumeEventsAndReports();
            }
            catch (OctaneSdkException e)
            {
                Console.WriteLine("Failed to connect. Please try again");
                throw e;
            }
        }

        public static void OnConnectionLost(ImpinjReader reader)
        {
            // This event handler is called if the reader  
            // stops sending keepalive messages (connection lost).
            Console.WriteLine("Connection lost : {0} ({1})", reader.Name, reader.Address);

            // Cleanup
            reader.Disconnect();

            // Try reconnecting to the reader
            ConnectToReader();
        }

        public static void OnKeepaliveReceived(ImpinjReader reader)
        {
            // This event handler is called when a keepalive 
            // message is received from the reader.
            Console.WriteLine("Keepalive received from {0} ({1})", reader.Name, reader.Address);
        }

        public class ReaderData
        {
            public string EPC { get; set; }
            public string TID { get; set; }
            public string SeenCount { get; set; }
            public string LastTimeSeen { get; set; }
            public string FirstTimeSeen { get; set; }
            public string MaxRSSI { get; set; }
            public string MinRSSI { get; set; }
            public string AvgRSSI { get; set; }
            public string CurrentRSSI { get; set; }
            public string RfDopplerFreq { get; set; }
            public string PhaseAngle { get; set; }
            public string Distance { get; set; }
            public string Speed { get; set; }

        }

        public class RssiData
        {

            public double NewMaxRSSI { get; set; }
            public double NewMinRSSI { get; set; }
            public double NewAvgRSSI { get; set; }
            public double NewTagCount { get; set; }
            public double Distance { get; set; }
            public double Speed { get; set; }
            public double LastTimeSeen { get; set; }
            public double AvgDistance { get; set; }
            public double AvgSpeed { get; set; }
            public double FirstTimeSeen { get; set; }
            public double AvgDist10 { get; set; }
        }
        public class InfoData
        {
            
            public static string txtfileName = @"/Users/aidanshinfeld/Projects/impinjspeedwayreader/impinjspeedwayreader/bin/Debug/Trial"+GlobalVariable.TrialNumber+".txt";
            public static string csvfileName = @"/Users/aidanshinfeld/Projects/impinjspeedwayreader/impinjspeedwayreader/bin/Debug/Trial"+GlobalVariable.TrialNumber+".csv";

            public static StreamWriter sw = new StreamWriter(txtfileName);
            
            public static StreamWriter writer = new StreamWriter(csvfileName);
            public static CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            public static List<ReaderData> records = new List<ReaderData>();

            public static Dictionary<string, RssiData> newRssiData = new Dictionary<string, RssiData> { };

           

        }

        public static void UpdateFileName()
        {
            if (GlobalVariable.isTxtFileWanted)
            {
                GlobalVariable.TrialNumber++;
                InfoData.txtfileName = @"/Users/aidanshinfeld/Projects/impinjspeedwayreader/impinjspeedwayreader/bin/Debug/Trial" + GlobalVariable.TrialNumber + ".txt";
                InfoData.sw = new StreamWriter(InfoData.txtfileName);
            }
            else
            {
                GlobalVariable.TrialNumber++;
                InfoData.csvfileName = @"/Users/aidanshinfeld/Projects/impinjspeedwayreader/impinjspeedwayreader/bin/Debug/Trial" + GlobalVariable.TrialNumber + ".csv";
                InfoData.writer = new StreamWriter(InfoData.csvfileName);
                InfoData.csv = new CsvWriter(InfoData.writer, CultureInfo.InvariantCulture);
            }
        }


        public static void CalcMaxRSSI(Dictionary<string, RssiData> newRssiData, string EPC, double value)
        {
            if(newRssiData.TryGetValue(EPC, out RssiData rssiData))
            {
                //Console.WriteLine("Rssi Data Available");
                if (value > rssiData.NewMaxRSSI)
                {
                    newRssiData[EPC].NewMaxRSSI = value;
                    //Console.WriteLine("Updated new Max RSSI");
                } 
            }
            else
            {
                //Console.WriteLine("Rssi Data UN-Available has value");
            }
        }

        public static void ClearData()
        {
            InfoData.records.Clear();
            InfoData.newRssiData.Clear();
            Array.Clear(distance1, 0, 10);
            i = 0;
        }

        public static void CalcMinRSSI(Dictionary<string, RssiData> newRssiData, string EPC, double value)
        {
            if (newRssiData.TryGetValue(EPC, out RssiData rssiData))
            {
                //Console.WriteLine("Rssi Data Available");
                if (value < rssiData.NewMinRSSI)
                {
                    newRssiData[EPC].NewMinRSSI = value;
                    //Console.WriteLine("Updated new Min RSSI");
                }
            }
            else
            {
                //Console.WriteLine("Rssi Data UN-Available has value");
            }
        }

        public static void CalcTagCount(Dictionary<string, RssiData> newRssiData, string EPC, double value)
        {
            if (newRssiData.TryGetValue(EPC, out RssiData rssiData))
            {
                //Console.WriteLine("Rssi Data Available");
                newRssiData[EPC].NewTagCount += value;
                //Console.WriteLine("Updated Tag Count");
            }
            else
            {
                //Console.WriteLine("Rssi Data UN-Available has value");
            }
        }

        public static void CalcAvgRSSI(Dictionary<string, RssiData> newRssiData, string EPC, double value_RSSI, double value_COUNT, double value_OLDAVG)
        {
            if (newRssiData.TryGetValue(EPC, out RssiData rssiData))
            {
                //Console.WriteLine("Rssi Data Available");
                newRssiData[EPC].NewAvgRSSI = 10 * Math.Log10((Math.Pow(10.0, value_OLDAVG / 10.0) * (double)value_COUNT + Math.Pow(10.0, value_RSSI / 10.0)) / ((double)value_COUNT + 1));
                //Console.WriteLine("Updated Avg RSSI");
                //10.0 * Math.Log10((Math.Pow(10.0, oldAverage / 10.0) * (double) count + Math.Pow(10.0, newValue / 10.0)) / ((double) count + 1.0)); 
            }
        }
       
        public static double CalcDistance(double RSSI)
        {
            return Math.Sqrt((Math.Pow(10.0, (GlobalVariable.MeasuredValue - (RSSI)) / 10.0)));
        }
        

        public static int i = 0;
        public static double[] distance1 = new double[10];
        public static bool overTagCount = false;
        public static void AvgDist(Dictionary<string, RssiData> newRssiData, string EPC, double tagCount, double distance, double time)
        {
            if (tagCount % 10 == 0 && overTagCount)
            {
                double sum = 0;
                foreach(Double dist in distance1)
                {
                    sum += dist;
                    
                }
                double avgDist = newRssiData[EPC].AvgDist10;
                newRssiData[EPC].AvgDist10 = (float)sum / tagCount;
                newRssiData[EPC].Speed = newSpeed(newRssiData, EPC, distance1[0], time);
                double speed = newSpeed(newRssiData, EPC, avgDist, time);
                Console.WriteLine(speed);
                newRssiData[EPC].FirstTimeSeen = time;
                Array.Clear(distance1, 0, 10);
                i = 0;
            }
            else if(tagCount == 10)
            {
                double sum = 0;
                foreach (Double dist in distance1)
                {
                    sum += dist;

                }
                newRssiData[EPC].AvgDist10 = (float)sum / tagCount;
                newRssiData[EPC].FirstTimeSeen = time;
                Array.Clear(distance1, 0, 10);
                overTagCount = true;
                i = 0;
            }
            else
            {
                distance1[i] = distance;
                i++;
            }
        }

        public static double newSpeed(Dictionary<string, RssiData> newRssiData, string EPC, double distance, double time)
        {
            double time_change = (time - newRssiData[EPC].FirstTimeSeen) * Math.Pow(10, -6);
            
            double distance_change = newRssiData[EPC].AvgDist10 - distance;
            Console.WriteLine(distance_change);
            return distance_change / time_change;
        }

        public static void CalcAverageDistance(Dictionary<string, RssiData> newRssiData, string EPC, double tagCount, double distance)
        {
            newRssiData[EPC].AvgDistance = (newRssiData[EPC].AvgDistance * (tagCount - 1) + distance) / tagCount;

        }


        public static void CalcSpeed(Dictionary<string, RssiData> newRssiData, string EPC, double newTimeSeen, double RSSI)
        {
            
                double change_Time = (newTimeSeen - newRssiData[EPC].LastTimeSeen)/ (Math.Pow(10,6));
                double change_Distance = Math.Abs(CalcDistance(RSSI) - newRssiData[EPC].Distance);
                double speed = (change_Distance / change_Time);
                //Console.WriteLine("Change Distance: {0}. Change Time {1}. Speed: {2}", change_Distance, change_Time, speed);
                newRssiData[EPC].Speed = speed;

                if (newRssiData[EPC].NewTagCount == 2)
                {
                    //newRssiData[EPC].Speed = speed;
                    newRssiData[EPC].AvgSpeed = speed;
                }
            //else
            //{
            //    if (speed - newRssiData[EPC].Speed <= 1)
            //    {
            //        newRssiData[EPC].Speed = speed;
            //    }

            //}
            //string test = GlobalVariable.NewEPCName.ToString("X4");
            
        }

        public static void CalcAverageSpeed(Dictionary<string, RssiData> newRssiData, string EPC, double tagCount, double speed)
        {
            newRssiData[EPC].AvgSpeed = (newRssiData[EPC].AvgSpeed * (tagCount - 1) + speed) / tagCount;
        }
        public static void UpdateTime(Dictionary<string, RssiData> newRssiData, string EPC, double Time, double RSSI)
        {
            if ((Time - newRssiData[EPC].LastTimeSeen) * Math.Pow(10, -6) >= 1)
            {
                CalcSpeed(newRssiData, EPC, Time, RSSI);
                newRssiData[EPC].LastTimeSeen = Time;
            } 
        }
        public static void OnTagsReportedToCsv(ImpinjReader reader, TagReport report)
        {
            
            foreach (Tag tag in report)
            {
                
                if (!InfoData.newRssiData.ContainsKey(tag.Epc.ToHexString()))
                {
                    Console.WriteLine("Does not contain EPC");
                    string EPC = "", LastTimeSeen = "", MaxRSSI = "", MinRssi = "", RSSI = "", AvgRSSI = "", DopplerFrequency = "", PhaseAngle = "", TID = "", TagCount = "", FirstTimeSeen = "", Distance = "", Speed = "";
                    
                    InfoData.newRssiData.Add(tag.Epc.ToHexString(), new RssiData() { NewMaxRSSI = tag.PeakRssiInDbm, NewAvgRSSI = tag.PeakRssiInDbm, NewMinRSSI = tag.PeakRssiInDbm, NewTagCount = tag.TagSeenCount, LastTimeSeen = tag.LastSeenTime.Utc, Distance = CalcDistance(tag.PeakRssiInDbm)}) ;
                    
                    

                    if (GlobalVariable.want_EPC) { EPC = tag.Epc.ToHexWordString(); }
                    if (GlobalVariable.want_LastTimeSeen) { LastTimeSeen = tag.LastSeenTime.ToString(); }
                    if (GlobalVariable.want_MaxRSSI) { MaxRSSI = tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_DopplerFrequency) { DopplerFrequency = tag.RfDopplerFrequency.ToString(); }
                    if (GlobalVariable.want_PhaseAngle) { PhaseAngle = tag.PhaseAngleInRadians.ToString(); }
                    if (GlobalVariable.want_TID) { TID = tag.Tid.ToHexWordString(); }
                    if (GlobalVariable.want_TagCount) { TagCount = tag.TagSeenCount.ToString(); }
                    if (GlobalVariable.want_FirstTimeSeen) { FirstTimeSeen = tag.FirstSeenTime.ToString(); }
                    if (GlobalVariable.want_AvgRSSI) { AvgRSSI = tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_MinRSSI) { MinRssi = tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_RSSI) { RSSI = tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_Distance) { Distance = CalcDistance(tag.PeakRssiInDbm).ToString();}

                    InfoData.records.Add(new ReaderData()
                    {
                        EPC = EPC,
                        LastTimeSeen = LastTimeSeen,
                        MaxRSSI = MaxRSSI,
                        RfDopplerFreq = DopplerFrequency,
                        PhaseAngle = PhaseAngle,
                        TID = TID,
                        SeenCount = TagCount,
                        FirstTimeSeen = FirstTimeSeen,
                        AvgRSSI = AvgRSSI,
                        CurrentRSSI = RSSI,
                        MinRSSI = MinRssi,
                        Distance = Distance,
                        Speed = Speed
                    });
                    //Console.WriteLine(InfoData.records);
                    InfoData.csv.WriteRecords(InfoData.records);
                    InfoData.csv.Flush();
                }

                else if (InfoData.newRssiData.ContainsKey(tag.Epc.ToHexString())) //&& tag.Epc.ToHexWordString() == "4865 6C6C 6F20 4920 616D 2041")
                {
                    Console.WriteLine("Does contain EPC");
                    string EPC = "", LastTimeSeen = "", MaxRSSI = "", MinRssi = "", RSSI = "", AvgRSSI = "", DopplerFrequency = "", PhaseAngle = "", TID = "", TagCount = "", FirstTimeSeen = "", Distance = "", Speed = "";
                    
                    CalcMaxRSSI(InfoData.newRssiData, tag.Epc.ToHexString(), tag.PeakRssiInDbm);
                    CalcMinRSSI(InfoData.newRssiData, tag.Epc.ToHexString(), tag.PeakRssiInDbm);
                    CalcTagCount(InfoData.newRssiData, tag.Epc.ToHexString(), tag.TagSeenCount);
                    CalcAvgRSSI(InfoData.newRssiData, tag.Epc.ToHexString(), tag.PeakRssiInDbm, InfoData.newRssiData[tag.Epc.ToHexString()].NewTagCount, InfoData.newRssiData[tag.Epc.ToHexString()].NewAvgRSSI);
                    CalcSpeed(InfoData.newRssiData, tag.Epc.ToHexString(), tag.LastSeenTime.Utc, tag.PeakRssiInDbm);
                    InfoData.newRssiData[tag.Epc.ToHexString()].Distance = CalcDistance(tag.PeakRssiInDbm);
                    InfoData.newRssiData[tag.Epc.ToHexString()].LastTimeSeen = tag.LastSeenTime.Utc;

                    if (GlobalVariable.want_EPC) {  EPC = tag.Epc.ToHexWordString(); }
                    if (GlobalVariable.want_LastTimeSeen) { LastTimeSeen = tag.LastSeenTime.ToString(); }
                    if (GlobalVariable.want_DopplerFrequency) { DopplerFrequency = tag.RfDopplerFrequency.ToString(); }
                    if (GlobalVariable.want_PhaseAngle) { PhaseAngle = tag.PhaseAngleInRadians.ToString(); }
                    if (GlobalVariable.want_TID) { TID = tag.Tid.ToHexWordString(); }
                    if (GlobalVariable.want_FirstTimeSeen) { FirstTimeSeen = tag.FirstSeenTime.ToString(); }
                    if (GlobalVariable.want_TagCount) { TagCount = InfoData.newRssiData[tag.Epc.ToHexString()].NewTagCount.ToString(); }
                    if (GlobalVariable.want_MaxRSSI) { MaxRSSI = InfoData.newRssiData[tag.Epc.ToHexString()].NewMaxRSSI.ToString(); }
                    if (GlobalVariable.want_AvgRSSI) { AvgRSSI = InfoData.newRssiData[tag.Epc.ToHexString()].NewAvgRSSI.ToString(); }
                    if (GlobalVariable.want_MinRSSI) { MinRssi = InfoData.newRssiData[tag.Epc.ToHexString()].NewMinRSSI.ToString(); }
                    if (GlobalVariable.want_RSSI) {  RSSI = tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_Distance) { Distance = InfoData.newRssiData[tag.Epc.ToHexString()].Distance.ToString(); }
                    if (GlobalVariable.want_Speed) { Speed = InfoData.newRssiData[tag.Epc.ToHexString()].Speed.ToString(); }

                    InfoData.records.Add(new ReaderData()
                    {
                        EPC = EPC,
                        LastTimeSeen = LastTimeSeen,
                        MaxRSSI = MaxRSSI,
                        RfDopplerFreq = DopplerFrequency,
                        PhaseAngle = PhaseAngle,
                        TID = TID,
                        SeenCount = TagCount,
                        FirstTimeSeen = FirstTimeSeen,
                        AvgRSSI = AvgRSSI,
                        CurrentRSSI = RSSI,
                        MinRSSI = MinRssi,
                        Distance = Distance,
                        Speed = Speed
                    }); 

                    //Console.WriteLine(InfoData.records);
                    InfoData.csv.WriteRecords(InfoData.records);
                    InfoData.csv.Flush();

                }
              
            }
        }


        //public static string data = "";
        public static void OnTagsReportedToTxt(ImpinjReader reader, TagReport report)
        {
            foreach (Tag tag in report)
            {
                string data = "";

            
                if (!InfoData.newRssiData.ContainsKey(tag.Epc.ToHexString()) && tag.Epc.ToHexWordString() == GlobalVariable.TargetEPCName && GlobalVariable.FilterTag)
                {
                    InfoData.newRssiData.Add(tag.Epc.ToHexString(), new RssiData() { NewMaxRSSI = tag.PeakRssiInDbm, NewAvgRSSI = tag.PeakRssiInDbm, NewMinRSSI = tag.PeakRssiInDbm, NewTagCount = tag.TagSeenCount, LastTimeSeen = tag.LastSeenTime.Utc, Distance = CalcDistance(tag.PeakRssiInDbm), AvgDistance = CalcDistance(tag.PeakRssiInDbm), FirstTimeSeen = tag.FirstSeenTime.Utc });
                    //CalcAverageDistance(InfoData.newRssiData, tag.Epc.ToHexString(), tag.TagSeenCount, InfoData.newRssiData[tag.Epc.ToHexString()].Distance);
                    AvgDist(InfoData.newRssiData, tag.Epc.ToHexString(), InfoData.newRssiData[tag.Epc.ToHexString()].NewTagCount, InfoData.newRssiData[tag.Epc.ToHexString()].Distance, tag.LastSeenTime.Utc);


                    if (GlobalVariable.want_EPC) { data += "EPC, " + tag.Epc.ToHexWordString(); }
                    if (GlobalVariable.want_LastTimeSeen) { data += ", Last Time Seen, " + tag.LastSeenTime.ToString(); }
                    if (GlobalVariable.want_FirstTimeSeen) { data += ", First Time Seen, " + tag.FirstSeenTime.ToString(); }
                    if (GlobalVariable.want_DopplerFrequency) { data += ", Doppler Frequency, " + tag.RfDopplerFrequency.ToString(); }
                    if (GlobalVariable.want_PhaseAngle) { data += ", Phase Angle, " + tag.PhaseAngleInRadians.ToString(); }
                    if (GlobalVariable.want_TID) { data += ", TID, " + tag.Tid.ToHexWordString(); }
                    if (GlobalVariable.want_TagCount) { data += ", Tag Count, " + tag.TagSeenCount.ToString(); }
                    if (GlobalVariable.want_MaxRSSI) { data += ", Max RSSI, " + tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_MinRSSI) { data += ", Min RSSI, " + tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_AvgRSSI) { data += ", Avg RSSI, " + tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_RSSI) { data += ",  RSSI, " + tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_Distance) { data += ", Distance, " + InfoData.newRssiData[tag.Epc.ToHexString()].Distance; }
                    if (GlobalVariable.want_Speed) { data += ", Speed, "; }
                    if (GlobalVariable.want_AvgDistance) { data += ", Avg Distance, " + InfoData.newRssiData[tag.Epc.ToHexString()].AvgDistance; }
                    if (GlobalVariable.want_AvgSpeed) { data += ", Avg Speed,  "; }
                    if (GlobalVariable.want_ModelData) { data += ", Model Info, " + tag.ModelDetails.ModelName; }

                    Console.WriteLine(data);
                    InfoData.sw.WriteLine(data);
                    InfoData.sw.Flush();
                }
                else if (!InfoData.newRssiData.ContainsKey(tag.Epc.ToHexString()) && GlobalVariable.IndividualTag || GlobalVariable.PeriodicTag)
                {
                    InfoData.newRssiData.Add(tag.Epc.ToHexString(), new RssiData() { NewMaxRSSI = tag.PeakRssiInDbm, NewAvgRSSI = tag.PeakRssiInDbm, NewMinRSSI = tag.PeakRssiInDbm, NewTagCount = tag.TagSeenCount, LastTimeSeen = tag.LastSeenTime.Utc, Distance = CalcDistance(tag.PeakRssiInDbm), AvgDistance = CalcDistance(tag.PeakRssiInDbm), FirstTimeSeen = tag.FirstSeenTime.Utc });
                    //CalcAverageDistance(InfoData.newRssiData, tag.Epc.ToHexString(), tag.TagSeenCount, InfoData.newRssiData[tag.Epc.ToHexString()].Distance);
                    AvgDist(InfoData.newRssiData, tag.Epc.ToHexString(), InfoData.newRssiData[tag.Epc.ToHexString()].NewTagCount, InfoData.newRssiData[tag.Epc.ToHexString()].Distance, tag.LastSeenTime.Utc);


                    if (GlobalVariable.want_EPC) { data += "EPC, " + tag.Epc.ToHexWordString(); }
                    if (GlobalVariable.want_LastTimeSeen) { data += ", Last Time Seen, " + tag.LastSeenTime.ToString(); }
                    if (GlobalVariable.want_FirstTimeSeen) { data += ", First Time Seen, " + tag.FirstSeenTime.ToString(); }
                    if (GlobalVariable.want_DopplerFrequency) { data += ", Doppler Frequency, " + tag.RfDopplerFrequency.ToString(); }
                    if (GlobalVariable.want_PhaseAngle) { data += ", Phase Angle, " + tag.PhaseAngleInRadians.ToString(); }
                    if (GlobalVariable.want_TID) { data += ", TID, " + tag.Tid.ToHexWordString(); }
                    if (GlobalVariable.want_TagCount) { data += ", Tag Count, " + tag.TagSeenCount.ToString(); }
                    if (GlobalVariable.want_MaxRSSI) { data += ", Max RSSI, " + tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_MinRSSI) { data += ", Min RSSI, " + tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_AvgRSSI) { data += ", Avg RSSI, " + tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_RSSI) { data += ",  RSSI, " + tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_Distance) { data += ", Distance, " + InfoData.newRssiData[tag.Epc.ToHexString()].Distance; }
                    if (GlobalVariable.want_Speed) { data += ", Speed, "; }
                    if (GlobalVariable.want_AvgDistance) { data += ", Avg Distance, " + InfoData.newRssiData[tag.Epc.ToHexString()].AvgDistance; }
                    if (GlobalVariable.want_AvgSpeed) { data += ", Avg Speed,  "; }
                    if (GlobalVariable.want_ModelData) { data += ", Model Info, " + tag.ModelDetails.ModelName; }

                    Console.WriteLine(data);
                    InfoData.sw.WriteLine(data);
                    InfoData.sw.Flush();
                }
                else if (InfoData.newRssiData.ContainsKey(tag.Epc.ToHexString()))// && tag.Epc.ToHexWordString() == "4865 6C6C 6F20 4920 616D 2041")
                {
                    CalcMaxRSSI(InfoData.newRssiData, tag.Epc.ToHexString(), tag.PeakRssiInDbm);
                    CalcMinRSSI(InfoData.newRssiData, tag.Epc.ToHexString(), tag.PeakRssiInDbm);
                    CalcTagCount(InfoData.newRssiData, tag.Epc.ToHexString(), tag.TagSeenCount);
                    CalcAvgRSSI(InfoData.newRssiData, tag.Epc.ToHexString(), tag.PeakRssiInDbm, InfoData.newRssiData[tag.Epc.ToHexString()].NewTagCount, InfoData.newRssiData[tag.Epc.ToHexString()].NewAvgRSSI);
                    //UpdateTime(InfoData.newRssiData, tag.Epc.ToHexString(), tag.LastSeenTime.Utc, tag.PeakRssiInDbm);
                    CalcSpeed(InfoData.newRssiData, tag.Epc.ToHexString(), tag.LastSeenTime.Utc, tag.PeakRssiInDbm);
                    InfoData.newRssiData[tag.Epc.ToHexString()].Distance = CalcDistance(tag.PeakRssiInDbm);
                    //AvgDist(InfoData.newRssiData, tag.Epc.ToHexString(), InfoData.newRssiData[tag.Epc.ToHexString()].NewTagCount, InfoData.newRssiData[tag.Epc.ToHexString()].Distance, tag.LastSeenTime.Utc);


                    //InfoData.newRssiData[tag.Epc.ToHexString()].Distance = CalcDistance(tag.PeakRssiInDbm);
                    InfoData.newRssiData[tag.Epc.ToHexString()].LastTimeSeen = tag.LastSeenTime.Utc;
                    CalcAverageDistance(InfoData.newRssiData, tag.Epc.ToHexString(), InfoData.newRssiData[tag.Epc.ToHexString()].NewTagCount, InfoData.newRssiData[tag.Epc.ToHexString()].Distance);
                    CalcAverageSpeed(InfoData.newRssiData, tag.Epc.ToHexString(), InfoData.newRssiData[tag.Epc.ToHexString()].NewTagCount, InfoData.newRssiData[tag.Epc.ToHexString()].Speed);


                    if (GlobalVariable.want_EPC) { data += "EPC, " + tag.Epc.ToHexWordString(); }
                    if (GlobalVariable.want_LastTimeSeen) { data += ", Last Time Seen, " + tag.LastSeenTime.ToString(); }
                    if (GlobalVariable.want_FirstTimeSeen) { data += ", First Time Seen, " + tag.FirstSeenTime.ToString(); }
                    if (GlobalVariable.want_DopplerFrequency) { data += ", Doppler Frequency, " + tag.RfDopplerFrequency.ToString(); }
                    if (GlobalVariable.want_PhaseAngle) { data += ", Phase Angle, " + tag.PhaseAngleInRadians.ToString(); }
                    if (GlobalVariable.want_TID) { data += ", TID, " + tag.Tid.ToHexWordString(); }
                    if (GlobalVariable.want_TagCount) { data += ", Tag Count, " + InfoData.newRssiData[tag.Epc.ToHexString()].NewTagCount.ToString(); }
                    if (GlobalVariable.want_MaxRSSI) { data += ", Max RSSI, " + InfoData.newRssiData[tag.Epc.ToHexString()].NewMaxRSSI.ToString(); }
                    if (GlobalVariable.want_MinRSSI) { data += ", Min RSSI, " + InfoData.newRssiData[tag.Epc.ToHexString()].NewMinRSSI.ToString(); }
                    if (GlobalVariable.want_AvgRSSI) { data += ", Avg RSSI, " + InfoData.newRssiData[tag.Epc.ToHexString()].NewAvgRSSI.ToString(); }
                    if (GlobalVariable.want_RSSI) { data += ",  RSSI, " + tag.PeakRssiInDbm.ToString(); }
                    if (GlobalVariable.want_Distance) { data += ", Distance, " + InfoData.newRssiData[tag.Epc.ToHexString()].Distance; }
                    if (GlobalVariable.want_Speed) { data += ", Speed, " + InfoData.newRssiData[tag.Epc.ToHexString()].Speed.ToString(); }
                    if (GlobalVariable.want_AvgDistance) { data += ", Avg Distance, " + InfoData.newRssiData[tag.Epc.ToHexString()].AvgDistance; }
                    if (GlobalVariable.want_AvgSpeed) { data += ", Avg Speed, " + InfoData.newRssiData[tag.Epc.ToHexString()].AvgSpeed;  }
                    if (GlobalVariable.want_ModelData) { data += ", Model Info, " + tag.ModelDetails.ModelName; }
                    Console.WriteLine(data);
                    InfoData.sw.WriteLine(data);
                    InfoData.sw.Flush();
                }

                


            }
        }

        public static void Restart()
        {
            GlobalVariable.reader.Stop();
            Connection.CloseFile();
            Connection.ClearData();

            Console.WriteLine("Reader Stop");


            Connection.UpdateFileName();
            if (GlobalVariable.TrialNumber > 10)
            {
                Console.WriteLine("10 trials completed");

            }
            else
            {
                NewSettings.CustomSettings();
                Console.WriteLine("Restart Start");
            }
        }

        //public static void printData(string data, double tagCount)
        //{
        //    if (tagCount == 1)
        //    {
        //        Console.WriteLine(data);
        //        InfoData.sw.WriteLine(data);
        //        InfoData.sw.Flush();

        //    }
        //    else if (tagCount % 2 == 0)
        //    {
        //        Console.WriteLine(data);
        //        InfoData.sw.WriteLine(data);
        //        InfoData.sw.Flush();
        //    }
        //}

        public static void CloseFile()
        {
            if (GlobalVariable.isTxtFileWanted) { InfoData.sw.Close(); }
            else { InfoData.writer.Close(); }
            
            
        }
        public static void OpenFile()
        {
            ProcessStartInfo ps = new ProcessStartInfo();
            if (GlobalVariable.isTxtFileWanted){ ps.FileName = InfoData.txtfileName; Console.WriteLine(ps.FileName); }
            else { ps.FileName = InfoData.csvfileName; }
            Process.Start(ps);
            
        }


        
        
    }
}



