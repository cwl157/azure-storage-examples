# Azure Storage Examples
Example of different azure storage options with a CRUD-like app. The current storage example is securing Azure blob storage using IAM and a client secret. The ConfigConstants.cs file contains placeholders for all the keys required.
# Setup
Update ConfigConstants.cs with values from your Azure AD and storage resources.
# The application
The example application creates blob containers, downloads blobs, and uploads blobs to a storage account. The app is authenticated to Azure Storage using an Azure AD app registration and RBAC permissions. There is a blog post with details https://www.carlserver.com/blog/post/use-azure-ad-to-authorize-access-to-azure-blob-storage.
