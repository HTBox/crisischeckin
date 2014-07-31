# CrisisCheckin Mobile Apps


### Supported Platforms
 - Xamarin.iOS
 - Xamarin.Android
 - Windows Phone 8
 - Windows Store (Future Goal)
 
### Guiding Principles
 - Keep it Fast, easy to use, and intuitive
 - Make it work before adding more features
 - Resiliency - Assume terrible network connectivity
 - Efficient - Use as little battery as possible
 - Xamarin - Keep it Starter (FREE) Edition compatible
 
### Xamarin Starter Edition
Since this is a free, open source, community effort, we are striving to keep the Xamarin projects as small as possible so that they are compatible with the (FREE) Starter Edition of Xamarin products.  We believe that if we can maintain this goal, we will have more success getting a wider audience to contribute to the projects.  It will also help people who may want to 'learn' Xamarin development by trying to contribute back to this great project! So, remember:
 - **No 3rd party libraries** - Any libraries we actually need to use will be vetted before accepting in any pull requests
 - **No MVVM, MVC, etc frameworks** - While these are nice, they go against our size limitation of the Starter edition, and they also raise the learning curve for contributing
 - **No PCL's :(** - This is unfortunate, but as part of the decision to minimize app size, NOT using PCL's gains us the advantage of having System.Json, System.Net.Http, and a few other core libraries at our disposal, without counting them towards our app size limit!
 - **iOS Layouts** - Avoid XIB's they add size bloat - Try to use MonoTouch.Dialog when possible to minimize the amount of layout code needed
 - **Android Ice Cream Sandwich (4.0.3)+** - As of July, 86% of devices run 4.0.3 or higher.  Targeting this version allows us to not depend on Android Support Libraries (which would put us over the starter edition limit), and decreases complexity in app development.

### Application flow
The typical application flow goes something like this:
1. Register - Provide Username, Name, Email, Phone, Cluster, and Password
2. Sign In - Provide Username and Password
3. List Commitments - Shows a list of Disasters you have volunteered for (aka commmitments)
4. List of Disasters - Shows a list of Disasters you can volunteer for (commit to)
5. Pick a Disaster - Provide Start and End date of your commitment to this disaster
6. Check In - When you arrive at a disaster, you can go to the details of your commitment to that disaster and Check In.  This allows coordinators to know you are available, and where you are.
7. Check Out - When you leave, you check out and make it known to the coordinators you are no longer at or available to volunteer for your commitment.

This may be more easily described in an image:

![Android Wireframe](../resources/Wireframe-Android.png)


### Web Service

To suppor