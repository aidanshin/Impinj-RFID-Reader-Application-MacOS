using System;
using AppKit;
using Impinj.OctaneSdk;
using System.IO;

namespace impinjspeedwayreader
{
	public class GlobalVariable
	{
		public static string Host_IP;
		
		public static ImpinjReader reader = new ImpinjReader();

		public static string TargetEPCName;

		public static string NewEPCName;
		
		public static double MeasuredValue = -41;//-41.5;//48.9250313406676;
		public static double TrialNumber = 0;

		public static string EPC;
		public static string LastTimeSeen;

		public static bool isTxtFileWanted;
		public static bool isCSVFileWanted;

		public static bool IndividualTag;
		public static bool PeriodicTag;
		public static bool FilterTag;

		public static bool wantCertainEPC;

		public static bool desiredTimeChange = false;

		public static bool want_EPC = true;
		public static bool want_TID = true;
		public static bool want_TagCount = true;
		public static bool want_MaxRSSI = true;
		public static bool want_MinRSSI = true;
		public static bool want_RSSI = true;
		public static bool want_AvgRSSI = true;
		public static bool want_LastTimeSeen = true;
		public static bool want_FirstTimeSeen = true;
		public static bool want_PhaseAngle = true;
		public static bool want_DopplerFrequency = true;
		public static bool want_Distance = true;
		public static bool want_Speed = true;
		public static bool want_AvgDistance = true;
		public static bool want_AvgSpeed = true;
		public static bool want_ModelData = true;
	}
}

