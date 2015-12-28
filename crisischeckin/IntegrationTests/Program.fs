open canopy
open runner
open System

//start an instance of the firefox browser
start firefox

"The Administrator can log in." &&& fun _ ->

    //go to url
    Actions.Login Constants.AdminUserName Constants.AdminPassword

     // Assert that the three action buttons are displayed
    displayed "Add New Disaster"
    displayed "Add New Cluster"
    displayed "Cluster List"

"The Administrator can Add a disaster." &&& fun _ ->
    click "Add New Disaster"

    "#Name" << "Indiana Earth Quake"

    click "input.btn-success"

    // Assert that the new disaster is in the disaster list
    "td" *= "Indiana Earth Quake"

"The Administrator can Add a cluster." &&& fun _ ->
    Actions.Login Constants.AdminUserName Constants.AdminPassword 
    click "Add New Cluster"
    "#Name" << "Test Cluster"
    click "input.btn-success"

    // Assert that the cluster shows up in the cluster list
    "td" *= "Test Cluster"

//"A new user can create an account" &&& fun _ ->
//    Actions.CreateAccount "Test" "User"
//    "h3" *= "Registration was successful"

"The Test User can login" &&&& fun _ ->
    Actions.Login "TestUser" "test"

//run all tests
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()
