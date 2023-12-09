using System;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Dynamic;
using System.Text;
using AppKit;
using Foundation;
using Impinj.OctaneSdk;


namespace impinjspeedwayreader
{
	public partial class ViewController : NSViewController
	{
		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


		}






		public override NSObject RepresentedObject {
			get {
				return base.RepresentedObject;
			}
			set {
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}



		partial void StartButtonAction(NSButton sender)
		{
			Connection.UpdateFileName();
			//Connection.InfoData info = new Connection.InfoData();
			NewSettings.CustomSettings();


		}
		partial void StopButtonAction(NSButton sender)
		{

			GlobalVariable.reader.Stop();
			Connection.CloseFile();
			Connection.ClearData();
			
			Console.WriteLine("Reader Stop");
		}

		partial void SubmitIPAction(NSButton sender)
		{
			GlobalVariable.Host_IP = EnterIPOutlet.StringValue;
			GlobalVariable.reader.Name = EnterNameOutlet.StringValue;
			Console.WriteLine("The reader is now named: " + GlobalVariable.reader.Name);
			Console.WriteLine("Now attempting to connect to: " + GlobalVariable.Host_IP);
			Connection.ConnectToReader();

		}

		partial void OpenFileButton(NSButton sender)
		{
			Connection.OpenFile();
		}

        partial void RenameEPCButton(NSButton sender)
        {
			GlobalVariable.NewEPCName = NewEPCNameOutlet.StringValue;
			

        }

        //partial void SaveFileButton(NSButton sender)
        //{
        //	SaveFile();
        //}

        //public static string[] allowedFilesEndings = { "txt", "csv" };
        //public static string day = DateTime.Now.ToString();
        //public static NSSavePanel save = new NSSavePanel();
        //public static void SaveFile()
        //{

        //	save.AllowedFileTypes = allowedFilesEndings;
        //	save.CanCreateDirectories = true;
        //	save.ExtensionHidden = false;

        //	save.Title = "Save File";
        //	save.Message = "Choose a folder and a name to store the file.";
        //	save.Prompt = "Save now";
        //	save.NameFieldLabel = "File Name:";
        //	save.NameFieldStringValue = "Impinj Reader Data " + day;
        //	//save.DirectoryUrl = ;

        //	Console.WriteLine(save.Url);

        //	save.RunModal();
        //}


        partial void SaveSettingsButton(NSButton sender)
        {

			GlobalVariable.TargetEPCName = TargetEPCOutlet.StringValue;

			if (DataAddEPC.State == NSCellStateValue.On) { GlobalVariable.want_EPC = true; Console.WriteLine("EPC: " + GlobalVariable.want_EPC); }
			else { GlobalVariable.want_EPC = false; Console.WriteLine("EPC: " + GlobalVariable.want_EPC); }

			if (DataAddTID.State == NSCellStateValue.On) { GlobalVariable.want_TID = true; Console.WriteLine("TID: " + GlobalVariable.want_TID); }
			else { GlobalVariable.want_TID = false; Console.WriteLine("TID: " + GlobalVariable.want_TID); }

			if (DataAddTagCount.State == NSCellStateValue.On) { GlobalVariable.want_TagCount = true; Console.WriteLine("Tag Count: " + GlobalVariable.want_TagCount); }
			else { GlobalVariable.want_TagCount = false; Console.WriteLine("Tag Count: " + GlobalVariable.want_TagCount); }

			if (DataAddMaxRSSI.State == NSCellStateValue.On) { GlobalVariable.want_MaxRSSI = true; Console.WriteLine("Max RSSI: " + GlobalVariable.want_MaxRSSI); }
			else { GlobalVariable.want_MaxRSSI = false; Console.WriteLine("Max RSSI: " + GlobalVariable.want_MaxRSSI); }

			if (DataAddMinRSSI.State == NSCellStateValue.On) { GlobalVariable.want_MinRSSI = true; Console.WriteLine("Min RSSI: " + GlobalVariable.want_MinRSSI); }
			else { GlobalVariable.want_MinRSSI = false; Console.WriteLine("Min RSSI: " + GlobalVariable.want_MinRSSI); }

			if (DataAddRSSI.State == NSCellStateValue.On) { GlobalVariable.want_RSSI = true; Console.WriteLine("RSSI: " + GlobalVariable.want_MinRSSI); }
			else { GlobalVariable.want_RSSI = false; Console.WriteLine("RSSI: " + GlobalVariable.want_MinRSSI); }

			if (DataAddLastTimeSeen.State == NSCellStateValue.On) { GlobalVariable.want_LastTimeSeen = true; Console.WriteLine("Last Time Seen: " + GlobalVariable.want_LastTimeSeen); }
			else { GlobalVariable.want_LastTimeSeen = false; Console.WriteLine("Last Time Seen: " + GlobalVariable.want_LastTimeSeen); }

			if (DataAddFirstTimeSeen.State == NSCellStateValue.On) { GlobalVariable.want_FirstTimeSeen = true; Console.WriteLine("First Time Seen: " + GlobalVariable.want_FirstTimeSeen); }
			else { GlobalVariable.want_FirstTimeSeen = false; Console.WriteLine("First Time Seen: " + GlobalVariable.want_FirstTimeSeen); }

			if (DataAddPhaseAngle.State == NSCellStateValue.On) { GlobalVariable.want_PhaseAngle = true; Console.WriteLine("Phase Angle: " + GlobalVariable.want_PhaseAngle); }
			else { GlobalVariable.want_PhaseAngle = false; Console.WriteLine("Phase Angle: " + GlobalVariable.want_PhaseAngle); }

			if (DataAddDopplerFrequency.State == NSCellStateValue.On) { GlobalVariable.want_DopplerFrequency = true; Console.WriteLine("Doppler Frequency: " + GlobalVariable.want_DopplerFrequency); }
			else { GlobalVariable.want_DopplerFrequency = false; Console.WriteLine("Doppler Frequency: " + GlobalVariable.want_DopplerFrequency); }

			if (DataAddDistance.State == NSCellStateValue.On) { GlobalVariable.want_Distance = true; Console.WriteLine("Distance: " + GlobalVariable.want_Distance); }
			else { GlobalVariable.want_Distance = false; Console.WriteLine("Distance: " + GlobalVariable.want_Distance); }

			if (DataAddSpeed.State == NSCellStateValue.On) { GlobalVariable.want_Speed = true; Console.WriteLine("Speed: " + GlobalVariable.want_Speed); }
			else { GlobalVariable.want_Speed = false; Console.WriteLine("Speed: " + GlobalVariable.want_Speed); }

			if (DataAddAvgRSSI.State == NSCellStateValue.On) { GlobalVariable.want_AvgRSSI = true; Console.WriteLine("Avg RSSI: " + GlobalVariable.want_AvgRSSI); }
			else { GlobalVariable.want_AvgRSSI = false; Console.WriteLine("Avg RSSI: " + GlobalVariable.want_AvgRSSI); }

			if (DataAddAvgDistance.State == NSCellStateValue.On) { GlobalVariable.want_AvgDistance = true; Console.WriteLine("Avg Distance: " + GlobalVariable.want_AvgDistance); }
			else { GlobalVariable.want_AvgDistance = false; Console.WriteLine("Avg RSSI: " + GlobalVariable.want_AvgDistance); }

			if (DataAddAvgSpeed.State == NSCellStateValue.On) { GlobalVariable.want_AvgSpeed  = true; Console.WriteLine("Avg Distance: " + GlobalVariable.want_AvgSpeed); }
			else { GlobalVariable.want_AvgSpeed = false; Console.WriteLine("Avg RSSI: " + GlobalVariable.want_AvgSpeed); }

			if (DataAddModelData.State == NSCellStateValue.On) { GlobalVariable.want_ModelData = true; Console.WriteLine("Avg Distance: " + GlobalVariable.want_ModelData); }
			else { GlobalVariable.want_ModelData = false; Console.WriteLine("Avg RSSI: " + GlobalVariable.want_ModelData); }

			switch (CollectionTypeOutlet.SelectedTag)
            {
				case 0:
					GlobalVariable.IndividualTag = true;
					GlobalVariable.PeriodicTag = false;
					GlobalVariable.FilterTag = false;
					Console.WriteLine("Individual Tag: {0}\nPeriodic Tag: {1}\nFilter Tag: {2}", GlobalVariable.IndividualTag, GlobalVariable.PeriodicTag, GlobalVariable.FilterTag);
					break;
				case 1:
					GlobalVariable.IndividualTag = true;
					GlobalVariable.PeriodicTag = false;
					GlobalVariable.FilterTag = false;
					Console.WriteLine("Individual Tag: {0}\nPeriodic Tag: {1}\nFilter Tag: {2}", GlobalVariable.IndividualTag, GlobalVariable.PeriodicTag, GlobalVariable.FilterTag);

					break;
				case 2:
					GlobalVariable.IndividualTag = true;
					GlobalVariable.PeriodicTag = false;
					GlobalVariable.FilterTag = false;
					Console.WriteLine("Individual Tag: {0}\nPeriodic Tag: {1}\nFilter Tag: {2}", GlobalVariable.IndividualTag, GlobalVariable.PeriodicTag, GlobalVariable.FilterTag);
					break;
				default:
					break;
            }


			switch (FileTypeOutlet.SelectedTag)
			{
				case 0:
					GlobalVariable.isTxtFileWanted = true;
					GlobalVariable.isCSVFileWanted = false;
					Console.WriteLine("Text File: " + GlobalVariable.isTxtFileWanted);
					Console.WriteLine("CSV File: " + GlobalVariable.isCSVFileWanted);
					break;
				case 1:
					GlobalVariable.isCSVFileWanted = true;
					GlobalVariable.isTxtFileWanted = false;
					Console.WriteLine("CSV File: " + GlobalVariable.isCSVFileWanted);
					Console.WriteLine("Text File: " + GlobalVariable.isTxtFileWanted);
					break;
				default:
					break;
			}
		}

	}
}
