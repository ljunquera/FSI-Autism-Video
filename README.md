# FSI-Autism-Video
Repo for FSI Austim Hackathon

## Azure Development Environment Setup

This section provides instructions for building the Azure components you will need to run the application and the various services within it.  It relies on PowerShell on Windows (untested on Linux) and you must have the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) installed.

The Azure environment can be built or torn down with the `New-AzureEnvironment.ps1` and `Remove-AzureEnvironment.ps1`.

By default, these scripts work against the account that the core hackathon team is using in Azure.  However, this can be overridden with the `-Subscription` parameter.  `-ResourceGroup` is a required parameter.  If you are creating this for your personal dev purpose in the shared account, you should make sure that this is unique to you.

For full help, you may run `Get-Help` against either script:

	Get-Help -Full .\New-AzureEnvironment.ps1

The scripts will return output in JSON format that contains the relevant endpoints and service names you will need to use while developing or deploying the application.

If you are leveraging a new service that is not yet in the project, you must add it to the `New-AzureEnvironment.ps1` file.  The following snippet illustrates what each component does:

	# Verbose message explaining what is happening
	Write-Verbose "Creating CosmoDB Account"
	# Capture the output from an Azure CLI command using $fsiautism$resourcegroup for names that must be unique across all of Azure
	# The select statement grabs the output that you want the end user to have - typically this is just what they need to use the service.
	$output = Invoke-AzureCLI "az cosmosdb create --resource-group $resourcegroup --name fsiautism$resourcegroup" |
		select name, type, documentEndpoint
	# Add the selected output to $returns which contains the output of the script that is rendered as JSON at the end
	$returns |add-member -NotePropertyName CosmosDBAccount -NotePropertyValue $output

## TODO 
  * Create deploy scripts to deploy services to the environment using the output of New-AzureEnvironment
  * Consider how to use output of New-AzureEnvironment to allow developers to tweak config settings automatically for their dev environment
