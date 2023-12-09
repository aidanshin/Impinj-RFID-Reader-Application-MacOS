// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace impinjspeedwayreader
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSSegmentedControl CollectionTypeOutlet { get; set; }

		[Outlet]
		AppKit.NSButton DataAddAvgDistance { get; set; }

		[Outlet]
		AppKit.NSButton DataAddAvgRSSI { get; set; }

		[Outlet]
		AppKit.NSButton DataAddAvgSpeed { get; set; }

		[Outlet]
		AppKit.NSButton DataAddDistance { get; set; }

		[Outlet]
		AppKit.NSButton DataAddDopplerFrequency { get; set; }

		[Outlet]
		AppKit.NSButton DataAddEPC { get; set; }

		[Outlet]
		AppKit.NSButton DataAddFirstTimeSeen { get; set; }

		[Outlet]
		AppKit.NSButton DataAddLastTimeSeen { get; set; }

		[Outlet]
		AppKit.NSButton DataAddMaxRSSI { get; set; }

		[Outlet]
		AppKit.NSButton DataAddMinRSSI { get; set; }

		[Outlet]
		AppKit.NSButton DataAddModelData { get; set; }

		[Outlet]
		AppKit.NSButton DataAddPhaseAngle { get; set; }

		[Outlet]
		AppKit.NSButton DataAddRSSI { get; set; }

		[Outlet]
		AppKit.NSButton DataAddSpeed { get; set; }

		[Outlet]
		AppKit.NSButton DataAddTagCount { get; set; }

		[Outlet]
		AppKit.NSButton DataAddTID { get; set; }

		[Outlet]
		AppKit.NSTextField EnterIPOutlet { get; set; }

		[Outlet]
		AppKit.NSTextField EnterNameOutlet { get; set; }

		[Outlet]
		AppKit.NSSegmentedControl FileTypeOutlet { get; set; }

		[Outlet]
		AppKit.NSView MainView { get; set; }

		[Outlet]
		AppKit.NSTextField NewEPCNameOutlet { get; set; }

		[Outlet]
		AppKit.NSTableView ReaderTable { get; set; }

		[Outlet]
		AppKit.NSTextField TargetEPCOutlet { get; set; }

		[Action ("EditEPCButton:")]
		partial void EditEPCButton (AppKit.NSButton sender);

		[Action ("OpenFileButton:")]
		partial void OpenFileButton (AppKit.NSButton sender);

		[Action ("RenameEPCButton:")]
		partial void RenameEPCButton (AppKit.NSButton sender);

		[Action ("SaveFileButton:")]
		partial void SaveFileButton (AppKit.NSButton sender);

		[Action ("SaveSettingsButton:")]
		partial void SaveSettingsButton (AppKit.NSButton sender);

		[Action ("StartButtonAction:")]
		partial void StartButtonAction (AppKit.NSButton sender);

		[Action ("StopButtonAction:")]
		partial void StopButtonAction (AppKit.NSButton sender);

		[Action ("SubmitIPAction:")]
		partial void SubmitIPAction (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CollectionTypeOutlet != null) {
				CollectionTypeOutlet.Dispose ();
				CollectionTypeOutlet = null;
			}

			if (NewEPCNameOutlet != null) {
				NewEPCNameOutlet.Dispose ();
				NewEPCNameOutlet = null;
			}

			if (DataAddAvgDistance != null) {
				DataAddAvgDistance.Dispose ();
				DataAddAvgDistance = null;
			}

			if (DataAddAvgRSSI != null) {
				DataAddAvgRSSI.Dispose ();
				DataAddAvgRSSI = null;
			}

			if (DataAddAvgSpeed != null) {
				DataAddAvgSpeed.Dispose ();
				DataAddAvgSpeed = null;
			}

			if (DataAddDistance != null) {
				DataAddDistance.Dispose ();
				DataAddDistance = null;
			}

			if (DataAddDopplerFrequency != null) {
				DataAddDopplerFrequency.Dispose ();
				DataAddDopplerFrequency = null;
			}

			if (DataAddEPC != null) {
				DataAddEPC.Dispose ();
				DataAddEPC = null;
			}

			if (DataAddFirstTimeSeen != null) {
				DataAddFirstTimeSeen.Dispose ();
				DataAddFirstTimeSeen = null;
			}

			if (DataAddLastTimeSeen != null) {
				DataAddLastTimeSeen.Dispose ();
				DataAddLastTimeSeen = null;
			}

			if (DataAddMaxRSSI != null) {
				DataAddMaxRSSI.Dispose ();
				DataAddMaxRSSI = null;
			}

			if (DataAddMinRSSI != null) {
				DataAddMinRSSI.Dispose ();
				DataAddMinRSSI = null;
			}

			if (DataAddModelData != null) {
				DataAddModelData.Dispose ();
				DataAddModelData = null;
			}

			if (DataAddPhaseAngle != null) {
				DataAddPhaseAngle.Dispose ();
				DataAddPhaseAngle = null;
			}

			if (DataAddRSSI != null) {
				DataAddRSSI.Dispose ();
				DataAddRSSI = null;
			}

			if (DataAddSpeed != null) {
				DataAddSpeed.Dispose ();
				DataAddSpeed = null;
			}

			if (DataAddTagCount != null) {
				DataAddTagCount.Dispose ();
				DataAddTagCount = null;
			}

			if (DataAddTID != null) {
				DataAddTID.Dispose ();
				DataAddTID = null;
			}

			if (EnterIPOutlet != null) {
				EnterIPOutlet.Dispose ();
				EnterIPOutlet = null;
			}

			if (EnterNameOutlet != null) {
				EnterNameOutlet.Dispose ();
				EnterNameOutlet = null;
			}

			if (FileTypeOutlet != null) {
				FileTypeOutlet.Dispose ();
				FileTypeOutlet = null;
			}

			if (MainView != null) {
				MainView.Dispose ();
				MainView = null;
			}

			if (ReaderTable != null) {
				ReaderTable.Dispose ();
				ReaderTable = null;
			}

			if (TargetEPCOutlet != null) {
				TargetEPCOutlet.Dispose ();
				TargetEPCOutlet = null;
			}
		}
	}
}
