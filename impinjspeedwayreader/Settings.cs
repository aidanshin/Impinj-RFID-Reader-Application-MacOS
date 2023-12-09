using System;
using Impinj.OctaneSdk;


namespace impinjspeedwayreader
{
	public class NewSettings
	{
		public NewSettings()
		{
		}

        public static void CustomSettings()
        {
            try
            {
                // Get the default settings.
                // We'll use these as a starting point
                // and then modify the settings we're 
                // interested in.
                Settings settings = GlobalVariable.reader.QueryDefaultSettings();

                if (GlobalVariable.IndividualTag || GlobalVariable.FilterTag)
                {
                    settings.AutoStart.Mode = AutoStartMode.Immediate;
                    Console.WriteLine("Reader Started");
                    settings.AutoStop.Mode = AutoStopMode.None;
                    settings.Report.Mode = ReportMode.Individual;



                    settings.Filters.TagFilter1.MemoryBank = MemoryBank.Epc;
                    settings.Filters.TagFilter1.BitPointer = BitPointers.Epc;
                    settings.Filters.TagFilter1.TagMask = "E28011606000021360067D80";

                    settings.Filters.TagFilter1.BitCount = 16;

                    settings.Filters.Mode = TagFilterMode.OnlyFilter1;
                }
                else if (GlobalVariable.PeriodicTag)
                {
                    //--------------
                    // Send a tag report every time the reader stops (period is over).
                    settings.Report.Mode = ReportMode.BatchAfterStop;


                    ////Reading tags for 5 seconds every 10 seconds

                    settings.AutoStart.Mode = AutoStartMode.Periodic;
                    settings.AutoStart.PeriodInMs = 5000;
                    settings.AutoStop.Mode = AutoStopMode.Duration;
                    settings.AutoStop.DurationInMs = 10000;
                    //--------------
                }
                

               
                // Use Advanced GPO to set GPO #1 
                // when an client (LLRP) connection is present.
                settings.Gpos.GetGpo(1).Mode = GpoMode.LLRPConnectionStatus;

                // Tell the reader to include the timestamp in all tag reports.
                settings.Report.IncludeFirstSeenTime = true;
                settings.Report.IncludeLastSeenTime = true;
                settings.Report.IncludeSeenCount = true;
                settings.Report.IncludePhaseAngle = true;
                settings.Report.IncludePeakRssi = true;
                settings.Report.IncludeFastId = true;
                settings.Report.IncludeDopplerFrequency = true;
                

                //------------>>>>>>>>>>> //ADD BUTTONS FOR ATTENA AND CRC
                //settings.Report.IncludeAntennaPortNumber = true;
                //settings.Report.IncludeCrc = true;
                //------------>>>>>>>>>>>

                settings.ReaderMode = ReaderMode.DenseReaderM8;
                // If this application disconnects from the 
                // reader, hold all tag reports and events.
                settings.HoldReportsOnDisconnect = true;

                // Enable keepalives.
                settings.Keepalives.Enabled = true;
                settings.Keepalives.PeriodInMs = 5000;

                // Enable link monitor mode.
                // If our application fails to reply to
                // five consecutive keepalive messages,
                // the reader will close the network connection.
                settings.Keepalives.EnableLinkMonitorMode = true;
                settings.Keepalives.LinkDownThreshold = 5;
                
                
                // Assign an event handler that will be called
                // when keepalive messages are received.
                GlobalVariable.reader.KeepaliveReceived += Connection.OnKeepaliveReceived;

                // Assign an event handler that will be called
                // if the reader stops sending keepalives.
                GlobalVariable.reader.ConnectionLost += Connection.OnConnectionLost;

                // Apply the newly modified settings.
                GlobalVariable.reader.ApplySettings(settings);

                // Save the settings to the reader's 
                // non-volatile memory. This will
                // allow the settings to persist
                // through a power cycle.
                GlobalVariable.reader.SaveSettings();

                // Assign the TagsReported event handler.
                // This specifies which method to call
                // when tags reports are available.

                if(GlobalVariable.isTxtFileWanted)
                {
                    GlobalVariable.reader.TagsReported += Connection.OnTagsReportedToTxt;
                }
                else
                {
                    GlobalVariable.reader.TagsReported += Connection.OnTagsReportedToCsv;
                }
                
                
            }
            catch (OctaneSdkException e)
            {
                // Handle Octane SDK errors.
                Console.WriteLine("Octane SDK exception: {0}", e.Message);
            }
            catch (Exception e)
            {
                // Handle other .NET errors.
                Console.WriteLine("Exception : {0}", e.Message);
            }
        }
    }
}

