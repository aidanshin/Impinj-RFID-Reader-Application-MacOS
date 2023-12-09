# Impinj-RFID-Reader-Application-MacOS

I built this MacOS Application using C# for a summer research program. The application's purpose was to allow the users to control how data was collected for RFID tags.
I attempted to calculate the distance for these tags but ran out of time in the program to continue my progress. 
With beginner-level programming knowledge, I could produce a functioning application that involved data structures (Dictionary), classes, and objects. 

The UI of the application: 
![](https://github.com/[aidanshin]/[Impinj-RFID-Reader-Application-MacOS]/blob/master/RFID%READER%APP.png)

# How did the application work? 
The user manages the view controller to specify data collection and storage preferences. The view controller communicates these preferences to the physical Impinj reader for data collection. The collected data is sent to a dictionary, extracted, and subsequently stored in a file.
![](https://github.com/[aidanshin]/[Impinj-RFID-Reader-Application-MacOS]/blob/master/appstructure.png)
