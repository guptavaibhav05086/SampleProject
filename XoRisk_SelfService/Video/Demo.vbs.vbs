msgbox("This is my first script, Click OK to close")
Const cdoSendUsingPickup = 1
    Const cdoSendUsingPort = 2 'Must use this to use Delivery Notification
    Const cdoAnonymous = 0
    Const cdoBasic = 1 ' clear text
    Const cdoNTLM = 2 'NTLM
    'Delivery Status Notifications
    Const cdoDSNDefault = 0 'None
    Const cdoDSNNever = 1 'None
    Const cdoDSNFailure = 2 'Failure
    Const cdoDSNSuccess = 4 'Success
    Const cdoDSNDelay = 8 'Delay
    Const cdoDSNSuccessFailOrDelay = 14 'Success, failure or delay

    set objMsg = CreateObject("CDO.Message")
    set objConf = CreateObject("CDO.Configuration")

    Set objFlds = objConf.Fields
    With objFlds
      .Item("http://schemas.microsoft.com/cdo/configuration/sendusing") = cdoSendUsingPort
      .Item("http://schemas.microsoft.com/cdo/configuration/smtpserver") = "smtp.gmail.com"
      .Item("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = cdoBasic
      .Item("http://schemas.microsoft.com/cdo/configuration/sendusername") = "aucsalejobs@gmail.com"
      .Item("http://schemas.microsoft.com/cdo/configuration/sendpassword") = "iamdon@1987"
      .Update
    End With

    strBody = "This is a sample message." & vbCRLF
    strBody = strBody & "It was sent using CDO." & vbCRLF

    With objMsg
      Set .Configuration = objConf
      .To = "aucsalejobs@gmail.com"
      .From = "aucsalejobs@gmail.com"
      .Subject = "This is a CDO test message"
      .TextBody = strBody
       'use .HTMLBody to send HTML email.
      .Addattachment "c:\Demo.txt"
      .Fields("urn:schemas:mailheader:disposition-notification-to") = "aucsalejobs@gmail.com"
      .Fields("urn:schemas:mailheader:return-receipt-to") = "aucsalejobs@gmail.com"
      .DSNOptions = cdoDSNSuccessFailOrDelay
      .Fields.update
      .Send
msgbox("Send Mail Executed")
    End With