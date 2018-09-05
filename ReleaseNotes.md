* Overall improvements and bug fixes
	- Fix for netstandard.dll, copying correct assembly which allows usage in .NET 4.6.1 and Azure Automation.
	- Fix when Update-Help is called against modules, no errors are shown.

* Updated MicrosoftPowerBIMgmt.Data module
    - New Add-PowerBIDataset for adding datasets to workspaces.
    - New Add-PowerBIRows for adding rows to a table.
	- New Get-PowerBITable for getting tables.
	- New New-PowerBIColumn for creating a column object.
	- New New-PowerBITable for creating a table object.
	- New Remove-PowerBIRow for removing rows from a table.
	- New Set-PowerBITable for updating metadata and schema for a table.

* Updated MicrosoftPowerBIMgmt.Profile module
    - Defaulted -Body to empty string when -Method is POST\PATCH, warning is displayed.