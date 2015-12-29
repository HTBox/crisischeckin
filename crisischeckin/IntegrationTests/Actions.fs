module Actions


open canopy
open runner
open System

let CreateAccount firstName lastName =
    url "http://localhost:2077/Account/Login"
    click "Register"

    "#FirstName" << firstName
    "#LastName" << lastName
    "#PhoneNumber" << "123.456.7890"
    "#Email" << firstName + "." + lastName + "@mailinator.com"
    "#txt_userName" << firstName + "." + lastName

    "#txt_password" << "monkey"
    "#ConfirmPassword" << "monkey"
    click "Create new account"

let Login username password =
    url "http://localhost:2077/Account/Login"

    // Enter the username and password
    "#UserNameOrEmail" << username
    "#Password" << password

    // Log in
    click "input.btn-success"



let VolunteerForDisaster disaster cluster startDate endDate location =
    "#disasterList" << disaster
    "#activityList" << cluster
    "#dp_startDate" << startDate
    "#dp_endDate" << endDate
    "#VolunteerType" << location
    click "#GoButton"