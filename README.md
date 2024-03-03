# Failed enter home menu
This README explains what was done and when it failed.

## Using UberServer branch webservice-classic
Compile server, start the server.
Client:
Take client file:


## Using UberServer branch v4-3-10
Compile server, start the server.

Client:
- run UberPatcher from UberClient branch v4-3-9, paste the created UberStrike.UnitySdk.dll into /Managed of the UberStrike v4-3-9 application
- copy UberStrikeUsing4-3-10Server.xml in UberStrike_Data/ and rename it to UberStrike.xml

running the app results in an error at authentication of LoginMemberEmail:

System.Security.Cryptography.RijndaelManaged
Cmune.Util.Ciphers.RijndaelCipher

The log file is saved as output_logUsing4-3-10Server.txt in this repository.

This error happens because the server does not contain any RijndaelCipher authorization. Best bet is to delete RijndaelCipher from the client. Using server 4-3-10 it would make sense to take the client code of v4-3-10(UberStrike.DataCenter.UnitySdk.dll) and paste it into v4-3-9(UberStrike.UnitySdk.dll).

## Using UberServer branch v4-3-9(webservice-classic branch)
Compile server, start the server.

Client:
- run UberPatcher from UberClient branch v4-3-9, paste the created UberStrike.UnitySdk.dll into /Managed of the UberStrike v4-3-9 application
- copy UberStrikeUsing4-3-10Server.xml in UberStrike_Data/ and rename it to UberStrike.xml

It does not even find the method AuthenticateApplication.