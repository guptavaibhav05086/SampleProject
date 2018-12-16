Const cdoSendUsingPickup = 1 'Send message using the local SMTP service pickup directory. 
Const cdoSendUsingPort = 2 'Send the message using the network (SMTP over the network). 

Const cdoAnonymous = 0 'Do not authenticate
Const cdoBasic = 1 'basic (clear-text) authentication
Const cdoNTLM = 2 'NTLM

Set objMessage = CreateObject("CDO.Message") 
objMessage.Subject = "Example CDO Message" 
objMessage.From = "aucsalejobs@gmail.com" 
objMessage.To = "aucsalejobs@gmail.com" 
objMessage.TextBody = "This is some sample message text.." & vbCRLF & "It was sent using SMTP authentication."
objMessage.AddAttachment "c:\Demo.txt"

'==This section provides the configuration information for the remote SMTP server.

objMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2 

'Name or IP of Remote SMTP Server
objMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpserver") = "smtp.gmail.com"

'Type of authentication, NONE, Basic (Base64 encoded), NTLM
objMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = cdoBasic

'Your UserID on the SMTP server
objMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/sendusername") = "aucsalejobs@gmail.com"

'Your password on the SMTP server
objMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/sendpassword") = "Iamdon@1987"

'Server port (typically 25)
objMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25 

'Use SSL for the connection (False or True)
objMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = True

'Connection Timeout in seconds (the maximum time CDO will try to establish a connection to the SMTP server)
objMessage.Configuration.Fields.Item _
("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout") = 60

objMessage.Configuration.Fields.Update

'==End remote SMTP server configuration section==

objMessage.Send