# FSI-Autism-Video


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
=======
Video can be an essential tool in behavior treatment for autism from very simplistic use cases, like the ability for Behavior Analysis to be able to view more therapist interactions and provide more feedback, but there is great opportunity for modern technologies to transform the autism industry. So, there would be immediate benefit, but also possibility of greater impact by having video data. This was the premise of the original use case when the project was started during the first Microsoft Financial Services Autism Hackathon in 2018.

## 2018 Use Case 
The goal of the 2018 project was to connect data, collected by a system (there are many of them but we used the structure from a specific provider) and overlay it over a video. Also enabled a way for analysts and therapists/technicians to add commentary to a session. The end user could view the session and see the commentary and data associated with specific points in the video.

## 2019 Use Case 
Leverage work done on prior year’s project and enhance in three potential areas. First, extract text from the video, store, and check for progress over time. Also look for key words. Second, track sentiment of subject and potentially consider additional expressions which might be useful for autism therapy. Third, create montage of key data collection points for BCBA review so they can evaluate more cases sessions and provide better service for more subjects.

## 2020 Use Case
Leverage the Azure Kinect for the video use case to collect video and utilize Kinect's capabilities to demonstrate how it could assist therapists/technicians. It could do this in a few ways, for example, learning from data collection in app, combined with Azure Kinect data to learn possible stereotypies and recommend data collection points. Ideally, it would be an ambient intelligence to help with consistency of data collection, and help the therapist/technician from collecting data so they can spend more time with the subject.

## Next Steps
There are a number of ways this project can go but it would be great to find a way for families to easily stand up a system and start collecting footage/data for some of the more basic services, while retaining the data for analysis. 
