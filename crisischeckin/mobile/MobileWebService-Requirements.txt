Web Service Requirements
-------------------------
In order to create a cross platform service that serves mobile apps on iOS, Android and Windows Phone, we should expose a REST API over the existing and forthcoming Crisis Checkin services, i.e. get disasters, clusters, user checkin. REST APIs should return JSON blobs.

The REST APIs should be a wrapper of the existing Services which are checked into the GitHub repo (crisischeckin\crisischeckin\Services). DisasterServices.cs has the bulk of the functionality. Note: The project has user and people concepts, which appeard to be duplicative (Person is being used, user is not).

We decided that a RESTful API rather Mobile Web Serviecs in Azure, because Azure requires exposing the data schema to the app, which is not needed for this scenario and could make the app more cumbersome to build and maintain. A potential advantage of using the Azure Mobile Services SDK is that the hosting is taken care of, and in a distributed development environment, the SDK generates a sample solution that auto-connects to the data, which could save time.

Draft of Mobile APIs List:
Note - none of these API signatures are locked, feel free to adapt as they are implemented into the clients. Not being consumed as of 10/5/2013.

   * User Registration API
   POST  /register?fname=limor&lname=lahiani&phone=&email=limorl@Microsoft.com&uname=limonit&token=xyz&clusterId=123 
   
   * Signin API
   POST /signin?uname=limonit

   * Signout API
   POST /signout?uname=limonit

   * Get clusters for registration
   GET /clusters

   * List of all disasters. Will show disasters in area with optional lat/long params
   GET /disasters?lat=73.15141&long=45.9393939393

   * Volunteer for a new commitment with a declared timeframe (dates)
   POST /volunteer?uname=limonit&did=4353535&sDate=848484&eDate=949494

   * (New) User check in to a certain location with a disaster id (did)
   POST /checkin?uname=limonit&did=55353&lat=56.777777&long=75.95959595  

   * (New) User check out from a disaster site once they leave 
   POST /checkout?uname=limonit

JSON definition for a disaster:
   * id
   * name
   * isActive
   * location

Data model missing functionality for mobile:
   * Location of disasters so they can be mapped, and queried for contextual mobile views
   * Disaster timeframe (separate from user checkin and active)
   * User check in once they arrive and check out once they leave
   * User authentication and  registration API (so registration can be drawn in native apps, not in a webview)


