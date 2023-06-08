## Sources of UberStrike steam client
 
The server IP address is compiled into the client.     
In Assembly-CSharp.dll -> ApplicationDataManager, put IP address in:    
WebserviceBaseUrl and ImagePath    
example with IP 13.38.94.69:    

ApplicationDataManager.WebServiceBaseUrl = "http://13.38.94.69:5000/2.0/";    
ApplicationDataManager.ImagePath = "http://13.38.94.69:5000/images/";     
			
Take the Release, which is the folder UberStrike_Data and replace it in UberStrike steam installation:     
C:\Program Files (x86)\Steam\steamapps\common\UberStrike    
Complete modified UberStrike client is added as well.
