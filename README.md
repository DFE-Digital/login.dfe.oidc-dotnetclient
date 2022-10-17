# Introduction 
This repository contains a sample .NET client written in C# for integrating with DfE Sign-in.

### What is DfE Sign-in
DfE Sign-in is a single sign-on (SSO) and identity and access management (IDAM) solution used within the Department for Education tailored towards sector facing organisations (schools, colleges, training providers etc.) and their internal users.

![DSI Landing Page](/images/dsi-landing-page.jpg)

### Who is this sample client for?
Technical team members within service teams across DfE that need to integrate with DfE Sign-in for SSO and IDAM who are running on a .NET technology stack.

### Credits
This code was originally built by Ramya Athiappan which the DfE Sign-in team thank her for.

# Getting Started
Follow these simple steps to get the client working:
1.	Installation
2.	Getting setup in DfE Sign-in
4.  Running the .NET client

# 1 Installation
This guidance is provided specifically for Visual Studio 2022 edition. For another IDE such as Visual Studio Code please consult the relevant documentation.

1. Download and install [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) if you do not already have it
2. Once downloaded, open Visual Studio then 
3. Click on File > Clone Repository
4. Enter ```https://github.com/DFE-Digital/login.dfe.oidc-dotnetclient.git```
5. Click Clone
6. Done

# 2 Getting setup in DfE Sign-in

## 2.1 Pre-requisites prior to on-boarding

### ESFA Teams migrating from Pirean IDAMS to DfE Sign-in
Please contact the Pirean IDAMS migration team to assist with trialing DSI integration, or alternatively if and when you are ready we can raise a ticket for your specific service to be setup in the DSI Test environment. 

### All other Service Teams on-boarding to DSI
Please follow the standard process for DfE Sign-in by submitting your request ticket here https://help.signin.education.gov.uk/contact-us and our team will be in touch with an on-boarding request form to complete.

# 2.2 Setting up your user permissions setup

## Setup your user account
Note: If you already have a user account in the Test environment you can skip these steps
1. Go to https://test-services.signin.education.gov.uk/
2. Click on the *'create a DfE Sign-in account'* link
3. Enter your *First Name*, *Last Name* and chosen *Email Address*
4. Click *Continue*
5. Go to your email, you will have recieved a verification code
6. Click on *Verify email and set password button*
7. Enter your *Verification Code*
8. Enter your chosen Password twice
9. Registration complete

## 2.3 Request access to the Sample Organisation
Note: The sample organisation is currently hidden from public view so please contact your DSI point of contact to ask them to add you as a member.

## 2.4 Request access to the Sample Service
1. Visit https://test-services.signin.education.gov.uk/
2. Click on *Start now*
3. Enter your *Email Address* and *Password*

![DSI Signin Page](/images/dsi-signin-page.jpg)

4. Click on *Request access to a service*
5. Select the *[Sample Service]* and click on Continue
6. Select your chosen *sub-service* (otherwise known as role name) and click Continue
7. Review the details and click *Request service*
8. Wait for your request to be approved
9. Once approved you will receive an email confirmation

## 2.5 Visit the Manage Console to retrieve the service authentication credentials
Note: Speak to your Piream Migration team contact to authorise access to the Manage Console before following these steps
1. Visit https://test-manage.signin.education.gov.uk/
2. Select the *[Sample Service]* and click on Continue

![DSI Manage Service Page](/images/dsi-manage-service-page.jpg)

3. Click on *Service configuration*
5. Scroll down the page to find the credentials required to setup the .NET client

![DSI Manage Service Main Page](/images/dsi-manage-config.jpg)

## 2.6 Configure your *appsettings.Development.json* file
1. Copy the values from ```Client Id```, ```Client Secret```, and ```API Secret``` into your *appsettings.Development.json* file
2. Make sure the ```Redirect Urls``` list includes the one specified in your ```CallbackUrl```
    > Note: Be sure to include your localhost and port number e.g. if your file says ```authentication/callback``` make sure the Redirect Urls list includes https://localhost:[port]/authentication/callback
4. Make sure the  ```Logout Redirect Urls``` list includes the one specified in your ```SignoutRedirectUrl``` 

# 3 Running the .NET client
1. Return to Visual Studio 2022
2. Click on *Debug > Start Without Debugging*
3. Click the *Start* link in the client landing page
4. Sign-in to DSI using your user account which you created in step 2.2

![DSI Manage Service Main Page](/images/client-sign-in.jpg)

5. You should now see the claims present in the OIDC/OAuth token and further data returned from the DfE Sign-in Public API  

![DSI Manage Service Main Page](/images/client-claims-returned.jpg)

# General troubleshooting
If you have any issues running the client, double check the settings in your local *appsettings.Development.json* file are correct: 
1. Sign into the Manage Console (https://test-manage.signin.education.gov.uk)
2. Select the service you are authentication users with 
3. Compare the values in the Service Configuration page

## For ESFA Teams migrating to DfE Sign-in
Please contact the Pirean IDAMS migration team via your dedicated Slack channel or via your ususual on-boarding point of contact.

## For all other Service Teams
If you are still having issues, feel free to contact the DSI team at our [#dfe_sign-in](https://ukgovernmentdfe.slack.com/archives/C5S500XB6) slack channel or via our  contact us page here https://help.signin.education.gov.uk/contact-us
