[![Build status](https://ci.appveyor.com/api/projects/status/s39nui8rkxipt0er/branch/master?svg=true)](https://ci.appveyor.com/project/HTBox/crisischeckin/branch/master)

crisischeckin
=============

CrisisCheckin is an application meant to capture, share and integrate the data around volunteers, organizations and resources
actively deployed into a disaster.  This ensures that we create opportunities for coordination and data sharing that will increase
effectiveness and productivity of resources delivering resources to the needs of those affected in disasters.

CrisisCheckin is being developed for many use cases but one of our key use cases is being the spotlight example of how to create
value from data sharing during response to disasters and medical crises as part of Operation Dragon Fire (http://odf.nvoad.org)

More information will come and for key issues to help contribute to excercises planned in March, April and May of 2016 please see this milestone:
https://github.com/HTBox/crisischeckin/milestones/Operation%20Dragon%20Fire%20-%20East%20Coast%20Excercise

If you're interested in helping us validate the application, please help us test it on our
staging site:  http://crisischeckin-d.azurewebsites.net

The Administrator login (on the staging site) is:
UID:  Administrator
PW: P@$$w0rd

The volunteer login (on the staging site) is:
UID: TestUser
PW: test


simple requirements
=============
Focus on simplicity and ease of use.

Contact information is name, email and phone number

Availability is Start Date to End Date (one stretch, no times, no complexity)

No worries on security for now (access given when link is received)

User ties themselves to one cluster (a cluster is an area of focus for humanitarian response)

Important notes from a member of the NGO community (Future requirements)
===========================

One problem with volunteers for crisis events is keeping unqualified and unneeded people out of the way of the most qualified and necessary workers to get into the zones.  Volunteers often can become a huge burden who actually interfere and draw away support and logistics that is needed for the local victims and necessary first-response personnel.  Some thought is needed to classify and prioritize volunteers to allow NGOs to filter out the key personnel from the rest that will be unmanageable with thousands of names.  Too many unqualified volunteers can be as bad or worse than too few qualified ones.  NGOs may need to define the traits of volunteers that they consider important or not for various purposes.

The point is that we will need to start building more of a profile for volunteers and their skills.

Installation on a Mac
====================
Since this project is an asp.net project and it is necessary to use visual studio. That being said this is one solution for developers using Macs to contribute to this project. This is also a solution for developers starting who do not already have visual studio installed on their computer.
####Step 1:
Create a fast dev environment to have visual studio. One solution to this is using Microsoft Azure, for this you have to have a Microsoft account (Hotmail or Outlook). Once you are logged into Azure follow these steps.
######a.
Select new from the lower left-hand corner. Select the virtual machine option and then select from gallery.
![Imgur](http://i.imgur.com/Nxa6490.png)
######b.
Now from the gallery options choose the visual studio on the right hand side. Suggest selecting "Visual Studio Community 2015 with Azure SDK 2.7" or the most updated version. Now continue through the steps to set up the name of your virtual machine, user name and password.
![Imgur](http://i.imgur.com/jhbVyFN.png)
######c.
Now download the Microsoft Remote Desktop app (from the App store) to be able to access your virtual machine.
######d.
Back on the azure website you have to connect to your virtual machine. This will download a Remote Desktop Program. If you have trouble with this take the information and put it into the Remote Desktop App after pressing new. For your username it may be just your username but it might be domain/username and then your password.
![Imgur](http://i.imgur.com/IglP8Wi.png)

####Step 2:
Now in the virtual machine it is now suggested to download a GitHub desktop app. Clone your forked repo and start contributing.
