<#
.SYNOPSIS
Deletion script for FSI-Autism-Hackathon-VideoCapture Azure Environment

.DESCRIPTION
Remove-AzureEnvironment.ps1 removes a resource group and all associated resources.  Please be careful to make sure that you are deleting the correct resource group if you are using the default SubscriptionID for the hackathon event.  You may accidentally wipe another developer's environment.  This script may be used by individual developers or as automation for the devops pipeline to ensure that you have a pristine environment before you run New-AzureEnvrionment.ps1

In order to use this script, you must first have the Azure CLI installed, and then you must login with az login: az login --allow-no-subscriptions

.PARAMETER ResourceGroup
Specify a ResourceGroup to delete

.PARAMETER Subscription
When not specified, you will use the SubscriptionID shared by the hackathon project.  If you are using your Azure account, you should override this with your subscriptionID.  It is only possible to receive access to the shared account by participating in the hackathon events.

.PARAMETER Force
When specified will pass --yes to azure CLI to force deletion.  When not specified, you will be prompted to confirm whether or not you would like to delete the resource group.  This switch should be used when the script is being used for batch automation, e.g., devops pipeline.

.INPUTS

None. You cannot pipe objects to New-AzureEnvironment

.OUTPUTS

Raw output from Azure CLI

.EXAMPLE

The following command will delete all resources in a resource group named after your local username in the shared Azure subscription used during the hackathon.  

PS> .\Remove-AzureEnvironment -ResourceGroup $env:USERNAME

.EXAMPLE

The following will delete all resources in the Resource Group named AutismHackathon.  It will use your user-supplied Subscription ID in $subid.

PS> .\Remove-AzureEnvironment -ResourceGroup AutismHackathon -Subscription $subid

.EXAMPLE

The following command will delete all resources in a resource group named pipelinedev in the shared Azure subscription used during the hackathon.  It will not prompt for confirmation to delete.  This is useful in Devops pipeline


PS> .\Remove-AzureEnvironment -ResourceGroup pipelinedev -Force
#>
param(
	  [Parameter(Mandatory=$true)]
	  [string]$ResourceGroup,
	  [Parameter(Mandatory=$false)]
	  [string]$Subscription="fe295523-5199-461a-a39d-0e9b04576bcf",
	  [switch]$Force
)
$command = "az group delete --name $resourcegroup"
if ($force) {
	$command += " --yes"
}
Write-Warning "Executing: $command"
invoke-expression $command
