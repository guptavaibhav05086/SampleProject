Dim iMsg As Object
    Dim iConf As Object
    Dim Flds As Variant


    Set iMsg = CreateObject("CDO.Message")
    Set iConf = CreateObject("CDO.Configuration")

    iConf.Load -1
    Set Flds = iConf.Fields
    
     With Flds
        .Item("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = True
        .Item("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1
        .Item("http://schemas.microsoft.com/cdo/configuration/sendusername") = "aucsalejobs@gmail.com"
        .Item("http://schemas.microsoft.com/cdo/configuration/sendpassword") = "Iamdon@1987"
        .Item("http://schemas.microsoft.com/cdo/configuration/smtpserver") = "smtp.gmail.com" 'smtp mail server
        .Item("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
        .Item("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25 'stmp server
        .Update
    End With

    With iMsg

        Set .Configuration = iConf
        .To = "guptavaibhav.05086@gmail.com"
        .From = "aucsalejobs@gmail.com"
        .Subject = "Hello" & Time
        .TextBody = "Hi Dude"
        .Send
    End With

    Set iMsg = Nothing
    Set iConf = Nothing