# SMTP Basic Setup

This application uses the SMTP Server from Gmail, so before you start to deploy it's important to configure your e-mail account. Follow this steps :

1. Manager Settings
2. 2-step Verification
3. Search for Apps Password
4. Copy and paste in **passwordMail**

After that, you will probably receive a confirmation message from Gmail asking for password permission, and you should just confirm the device.

### Troubles

If you are missing the delivery email or using an incorrect address, you will receive this exception, so double-check the address.

```
[application] System.Net.Mail.SmtpException: 503-5.5.1 RCPT first. A mail transaction protocol command was issued out of
503-5.5.1 sequence. For more information, go to
503-5.5.1  https://support.google.com/a/answer/3221692 and review RFC 5321
503 5.5.1 specifications. d2e1a72fcca58-72dab9c8da5sm8754980b3a.118 - gsmtp
  at System.Net.Mail.SmtpClient.SendCore (System.Net.Mail.M
ilMessage message) [0x0031a] in <29eef1970b204e4ab8bc3f3bfaa5e01a>:0 
  at System.Net.Mail.SmtpClient.SendInternal (System.Net.Mail.MailMessage message) [0x00050] in <29eef1970b204e4ab8bc3f3bfaa5e01a>:0 
  at System.Net.Mail.SmtpClient.Send (System.Net.Mail.MailMessage message) [0x00091] in <29eef1970b204e4ab8bc3f3bfaa5e01a>:0 
```