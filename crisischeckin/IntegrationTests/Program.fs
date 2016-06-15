open canopy
open runner
open System

//start an instance of the firefox browser
start firefox
resize (800, 600)

"The Administrator can log in." &&& fun _ ->
    //go to url
    Actions.Login Constants.AdminUserName Constants.AdminPassword

     // Assert that the three action buttons are displayed
    displayed "Add New Disaster"
    displayed "Add New Cluster"
    displayed "Cluster List"

"The Administrator can Add a disaster." &&& fun _ ->
    Actions.Login Constants.AdminUserName Constants.AdminPassword
    Actions.AddDisaster Constants.TestDisasterName

    // Assert that the new disaster is in the disaster list
    "td" *= Constants.TestDisasterName

"The Administrator can Add a cluster." &&! fun _ ->
    Actions.Login Constants.AdminUserName Constants.AdminPassword 
    Actions.AddACluster "Test Cluster"
    
    // Assert that the cluster shows up in the cluster list
    "td" *= "Test Cluster"

"The Test User can login" &&& fun _ ->
    Actions.Login "TestUser" "test"
    
    displayed " Logout TestUser"

"The Test User can Volunteer for a disaster" &&& fun _ ->
    Actions.Login Constants.BasicUserName Constants.BasicPassword
    let today = Dates.Today()
    let tomorrow = Dates.Tomorrow()
    Actions.VolunteerForDisaster Constants.TestDisasterName "Logistics Cluster" today tomorrow "On Site"
    "dd" *= Constants.TestDisasterName
    
"The admin can see that the user registered for the disaster" &&& fun _ ->
    Actions.Login Constants.AdminUserName Constants.AdminPassword
    Actions.ViewVolunteers()
    Actions.SelectADisaster Constants.TestDisasterName
    "div#results td" *= "User, Test"

"The user can signin for the disaster" &&& fun _ -> 
    Actions.Login "TestUser" "test"
    click "Check-in"
    "span" *= "Thank you for your help today!"
    
    
//run all tests
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()
