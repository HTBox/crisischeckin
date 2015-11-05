open canopy
open runner
open System

//start an instance of the firefox browser
start firefox

"The Administrator can log in." &&& fun _ ->

    //go to url
    url "http://localhost:2077/Account/Login"

    // Enter the username and password
    "#UserNameOrEmail" << "Administrator"
    "#Password" << "P@$$w0rd"

    // Log in
    click "input.btn-success"

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

//run all tests
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()
