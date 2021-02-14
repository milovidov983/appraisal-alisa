Remove-Item 'D:\Source\PedProject\appraisal-alisa\AliceAppraisal\obj' -Recurse -ErrorAction Ignore
Remove-Item 'D:\tmp\appraisal-bot-src.zip' -ErrorAction Ignore
& "C:\Program Files\7-Zip\7z.exe" a -tzip -ssw -mx0 -r0 "D:\tmp\appraisal-bot-src.zip" "D:\Source\PedProject\appraisal-alisa\AliceAppraisal\*"

yc serverless function version create `
--function-name=aliceappraisalbot `
--runtime dotnetcore31-preview `
--entrypoint AliceAppraisal.Application.EntryPoint `
--memory 128m `
--execution-timeout 3s `
--source-path d:\tmp\appraisal-bot-src.zip `
--environment ASPNETCORE_ENVIRONMENT=production `
--environment SheetConfig:SpreadsheetId=1cHYd0kfb-f1ATHkmyD9g1TMdGi6AgCwhAfeEFPhcD8E 


dotnet restore D:\Source\PedProject\appraisal-alisa