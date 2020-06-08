<#
.SYNOPSIS
Deployment script for FSI-Autism-Hackathon-VideoCapture

.DESCRIPTION
New-AzureEnvironment.ps1 creates all of the necessary services in Azure needed to run the FSI-Autism-Hackathon-VideoCapture application and its services.  It leverages the Azure CLI (must be installed before running).  If the resource group or components already exist, it will still run the commands, but it does not overwrite existing services.

This script may be used to create a development resource group for your personal development or it may be used in the devops pipeline to ensure that the environments for the app are already configured.

In order to use this script, you must first have the Azure CLI installed, and then you must login with az login: az login --allow-no-subscriptions

.PARAMETER ResourceGroup
Specify a ResourceGroup to create or use (if it already exists).  If you are running this for personal development, please be sure to use a ResourceGroup name that is unique to you.

.PARAMETER Subscription
When not specified, you will use the SubscriptionID shared by the hackathon project.  If you are using your Azure account, you should override this with your subscriptionID.  It is only possible to receive access to the shared account by participating in the hackathon events.

.PARAMETER Location
Specifies the Azure code for the location you wish to use

.INPUTS

None. You cannot pipe objects to New-AzureEnvironment

.OUTPUTS

JSON output of relevant information used to connect to these services.

.EXAMPLE

The following command will create the necessary resources in the shared Azure subscription used during the hackathon.  It will create resource group with your local username.  By default this will create the services in eastus.

PS> .\New-AzureEnvironment -ResourceGroup $env:USERNAME

.EXAMPLE

The following will create a resource group and all of the needed resources under the name AutismHackathon.  It will use your user-supplied Subscription ID in $subid.

PS> .\New-AzureEnvironment -ResourceGroup AutismHackathon -Subscription $subid

.EXAMPLE

The following command will create the necessary resources in the shared Azure subscription used during the hackathon.  It will create resource group with your local username.  All resources will be created in westus.


PS> .\New-AzureEnvironment -ResourceGroup $env:username -Location westus
#>
param(
	  [Parameter(Mandatory=$true)]
	  [string]$ResourceGroup,
	  [Parameter(Mandatory=$false)]
	  [string]$Subscription="fe295523-5199-461a-a39d-0e9b04576bcf",
	  [Parameter(Mandatory=$false)]
	  [string]$Location='eastus'
)


function Test-Error {
	param(
	    [Parameter(Mandatory=$true, Position=0)]
		[bool] $lastexit,
		[Parameter(Mandatory=$false, Position=1)]
		[string] $Message="Unknown error"
	)
	if (!$lastexit) {
		Write-Error $Message
		break
	}
}

function Invoke-AzureCLI {
	param(
		[Parameter(Mandatory=$true, Position=0)]
		[string] $command
	)
	Write-Verbose $command
	$output = Invoke-Expression $command
	Test-Error -lastexit $? -message "Error invoking: $command"
	$output |convertfrom-json
}

$returns = new-object psobject -property @{resourcegroup=$resourcegroup}

Write-Verbose "Testing whether resource group exists already"
$command = "az group exists --subscription $subscription --name $ResourceGroup"
$output = Invoke-AzureCLI $command
write-verbose "Received $output from Azure"
if (!$output) {
	Invoke-AzureCLI "az group create --location $location --name $resourcegroup" |out-null
}

# Storagename must be unique across all of Azure

Write-Verbose "Creating Azure Storage"
$output = Invoke-AzureCLI "az storage account create --location $location --resource-group $resourcegroup --name fsiautism$resourcegroup" |
	select name, type, @{n="endpoint"; e={$_.primaryEndpoints.blob}}
$returns |add-member -NotePropertyName storage -NotePropertyValue $output

Write-Verbose "Creating App Service Plan"
$output = Invoke-AzureCLI "az appservice plan create --location $location --resource-group $resourcegroup --name d1-plan --sku d1" |
	select name,type
$returns |add-member -NotePropertyName appservice -NotePropertyValue $output

# AMS name must be unique across all of Azure
Write-Verbose "Creating Azure Media Service"
$output = Invoke-AzureCLI "az ams account create --location $location --resource-group $resourcegroup --storage-account $($returns.storage.name) --name fsiautism$resourcegroup" |
	select name, type, mediaServiceId
$returns |add-member -NotePropertyName AzureMediaService -NotePropertyValue $output

# CosmoDB account name must be unique across all of Azure
Write-Verbose "Creating CosmoDB Account"
$output = Invoke-AzureCLI "az cosmosdb create --resource-group $resourcegroup --capabilities EnableTable --default-consistency-level Eventual --name fsiautism$resourcegroup" |
   select name, type, documentEndpoint
$returns |add-member -NotePropertyName CosmosDBAccount -NotePropertyValue $output

Write-Verbose "Creating CosmoDB markers table"
$output = Invoke-AzureCLI "az cosmosdb table create --resource-group $resourcegroup --account-name fsiautism$resourcegroup --name Markers"
$returns |add-member -NotePropertyName MarkersTable -NotePropertyValue $output

$returns |Convertto-json
