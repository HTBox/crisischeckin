module Dates

open System

let Today _ =
    sprintf "%d/%d/%d" DateTime.Now.Month DateTime.Now.Day DateTime.Now.Year

let Tomorrow _ =
    let oneDay = new TimeSpan(1, 0, 0, 0)
    let tomorrowsDate = DateTime.Today.Add(oneDay)
    sprintf "%d/%d/%d" tomorrowsDate.Month tomorrowsDate.Day tomorrowsDate.Year
