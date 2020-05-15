$Class = Get-SCSMClass CiresonTimerActivity #Timer_Activity = "Timer Activity"

$propHash = @{ 
                ID = "AC{0}"
                TItle = "New TimerActivity created $((Get-Date).ToString("yyyyMMdd_hhmmss"))"
                Description= "Description for TA created $((Get-Date).ToString("yyyyMMdd_hhmmss"))"
                Notes = "Notes created for $((Get-Date).ToString("yyyyMMdd_hhmmss"))"
                #NotificationSentDate = (Get-Date).AddDays(-30);

            }

$object = New-SCSMObject -Class $Class -PropertyHashtable $propHash -PassThru
$object