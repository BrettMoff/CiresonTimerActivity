$enumActive = (Get-SCSMEnumeration ActivityStatusEnum.Active$).Id
$enumOnHold = (Get-SCSMEnumeration ActivityStatusEnum.OnHold$).Id
$enumCompleted = (Get-SCSMEnumeration ActivityStatusEnum.Completed$).Id
$LogPath = (get-scsmsetting -displayname "Cireson Timer Activity Settings").LogPath
$LogEnable = (get-scsmsetting -displayname "Cireson Timer Activity Settings").Logenable

$ActiveTimers = get-scsmobject -class(get-scsmclass -name Cireson.Timer.Activity$) -filter "Status -eq $enumActive -or Status -eq $enumOnHold"
LogThis ("Number of active Timer Activities found: " + $ActiveTimers.count) -LogType "Start"
Try {
    ForEach ($Activity in $ActiveTimers) {
        If ((Get-Date) -gt $Activity.ScheduledEndTime) {
            $ActivityHash = @{"Status" = $enumCompleted}
            Set-SCSMObject -SMObject $Activity -PropertyHashTable $ActivityHash}
            LogThis ($Activity.id + " set to completed.") -LogType "Info"
    }
}
Catch{
    Logthis $_.exception.message -LogType "Error"
    }

Function LogThis {
    #This function is to allow log files to be written as needed
    param($LogText, $LogType)

    if ($LogEnable -eq 1) {
        $LogDate = get-date
        "$LogDate - $LogType - $logtext" | Out-File -filepath "$LogPath\TimerActivity.log" -append -encoding ascii
    }
}