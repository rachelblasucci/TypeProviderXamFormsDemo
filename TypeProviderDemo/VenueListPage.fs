namespace TypeProviderDemo

open System.Collections.ObjectModel
open Xamarin.Forms
open Xamarin.Forms.Xaml
open foursquare
open System
open types
open System.Diagnostics

type VenueListPage() = 
    inherit ContentPage()

    do base.LoadFromXaml(typeof<VenueListPage>) |> ignore

    let stacklayout = base.FindByName<StackLayout>("stacklayout")
    let listView = base.FindByName<ListView>("listView")

    let obCol = new ObservableCollection<Location>()

    let template = new DataTemplate(typedefof<TextCell>)

    do listView.ItemsSource <- obCol
    do listView.ItemTemplate <- template

    let tableItems = 
        foursquare.validCountries 
        |> List.iter (fun c -> 
                        template.SetValue(TextCell.TextProperty, c.Name)
                        template.SetValue(TextCell.DetailProperty, c.CapitalCity)
                        obCol.Add <| {Country = c.Name; City = c.CapitalCity; Name=None}
                     )

    do base.Padding <- Thickness(5.0, Device.OnPlatform(20.0, 5.0, 5.0), 5.0, 5.0)
    do base.Content <- stacklayout

    override x.OnAppearing () =
        base.OnAppearing ()

        let update v = 
            match v.Name with 
            | Some n -> 
                obCol.[obCol.IndexOf ({Country = v.Country; City = v.City; Name=None})] <- v //sprintf "%s, %s: %s" v.City v.Country v.Name 
                template.SetValue(TextCell.TextProperty, v.Country)
                template.SetValue(TextCell.DetailProperty, v.City + ": " + n)
            | None -> obCol.RemoveAt <| obCol.IndexOf ({Country = v.Country; City = v.City; Name=None})

        async {
            return foursquare.validCountries 
                |> List.iter (fun country -> 
                                  match getVenueFor country with 
                                  | Some v -> update v
                                  | None -> obCol.RemoveAt <| obCol.IndexOf ({Country = country.Name; City = country.CapitalCity; Name=None}))
        } |> Async.Start 
       