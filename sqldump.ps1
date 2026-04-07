$connString = "Data Source=172.190.120.3;Initial Catalog=ReportDB;User ID=sa;Password=Dell2014#;Connect Timeout=5"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()
$query = "SELECT TOP 1 * FROM ViewPolicyEarnedPremiumByCoverageUpToDate"
$cmd = $conn.CreateCommand()
$cmd.CommandText = $query
$reader = $cmd.ExecuteReader()
Write-Host "--- ViewPolicyEarnedPremiumByCoverageUpToDate ---"
for ($i=0; $i -lt $reader.FieldCount; $i++) {
    Write-Host $reader.GetName($i)
}
$reader.Close()

$query2 = "SELECT TOP 1 * FROM ViewPolicyEarnedPremiumByCoverageVehicle"
$cmd2 = $conn.CreateCommand()
$cmd2.CommandText = $query2
$reader2 = $cmd2.ExecuteReader()
Write-Host "--- ViewPolicyEarnedPremiumByCoverageVehicle ---"
for ($i=0; $i -lt $reader2.FieldCount; $i++) {
    Write-Host $reader2.GetName($i)
}
$reader2.Close()
$conn.Close()
