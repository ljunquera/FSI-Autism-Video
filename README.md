# FSI-Autism-Video
Repo for FSI Austim Hackathon

## Azure Development Environment Setup

This section provides instructions for building the Azure components you will need to run the application and the various services within it. 

### Prerequisites

It relies on PowerShell on Windows (untested on Linux) and you must have the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) installed.  You must also log in via Azure CLI before running the scripts:

    az login --allow-no-subscriptions

### Spinning up the Infra

The Azure environment can be built or torn down with the `New-AzureEnvironment.ps1` and `Remove-AzureEnvironment.ps1`.

By default, these scripts work against the account that the core hackathon team is using in Azure.  However, this can be overridden with the `-Subscription` parameter.  `-ResourceGroup` is a required parameter.  If you are creating this for your personal dev purpose in the shared account, you should make sure that this is unique to you.

For full help, you may run `Get-Help` against either script:

	Get-Help -Full .\New-AzureEnvironment.ps1

The scripts will return output in JSON format that contains the relevant endpoints and service names you will need to use while developing or deploying the application.

### Adding a new Azure service to the project
If you are leveraging a new service that is not yet in the project, you must add it to the `New-AzureEnvironment.ps1` file.  The following illustrates what each component does:

#### Verbose message explaining what is happening

	Write-Verbose "Creating CosmoDB Account"
	
#### Execute az command and provide output relevant to project configuration
The following executes an Azure CLI command.  You should use $resourcegroup, $location, or $subscription as global script variables as needed.

Where the name must be unique across all of Azure, we are using fsiautism$resourcegroup as the name.

Finally, the select statement at the end selects the return properties that someone might need to consume the service.  The script ulitmately creates JSON output that could be used later in a deploy script.

	$output = Invoke-AzureCLI "az cosmosdb create --resource-group $resourcegroup --name fsiautism$resourcegroup" |
		select name, type, documentEndpoint

#### Add the output to the return object
Each notepropertyname gets converted into a root hash element that has your config.  This name must be unique to any others already used in this script:

	$returns |add-member -NotePropertyName CosmosDBAccount -NotePropertyValue $output
