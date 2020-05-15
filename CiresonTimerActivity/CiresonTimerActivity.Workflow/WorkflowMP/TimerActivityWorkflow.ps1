$enumActive = (Get-SCSMEnumeration ActivityStatusEnum.Active$).Id
$enumOnHold = (Get-SCSMEnumeration ActivityStatusEnum.OnHold$).Id
$enumCompleted = (Get-SCSMEnumeration ActivityStatusEnum.Completed$).Id

$ActiveTimers = get-scsmobject -class(get-scsmclass -name timer_Activity) -filter "Status -eq $enumActive -or Status -eq $enumOnHold"

Try {
    ForEach ($Activity in $ActiveTimers) {
        If ((Get-Date) -gt $Activity.ScheduledEndTime) {
            $ActivityHash = @{"Status" = $enumCompleted}
            Set-SCSMObject -SMObject $Activity -PropertyHashTable $ActivityHash}
    }
}
Catch{
    out-file "C:\temp\timererror.log" -InputObject $_.exception.message -append}