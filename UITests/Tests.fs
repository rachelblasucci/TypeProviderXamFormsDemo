namespace TypeProviderDemo.UITests

open System
open System.IO
open NUnit.Framework
open Xamarin.UITest
open Xamarin.UITest.Queries

type Tests() = 
    [<TestCase(Platform.iOS)>]
    [<TestCase(Platform.Android)>]
    member this.`` Shows welcome screen`` (platform : Platform) = 
        let app = AppInitializer.startApp (platform)
        let results = app.WaitForElement(fun c -> c.Marked("Welcome to F# Xamarin.Forms!"))
        let screenshot = app.Screenshot("Welcome screen.")
        Assert.Greater(results.Length, 0)

