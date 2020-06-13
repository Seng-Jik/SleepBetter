open Suave
open Suave.Successful
open Suave.Filters
open Suave.Operators

open FSharpHTML
open FSharpHTML.Elements

open GiteeDrive
open Suave.State.CookieStateStore


let giteeAccessToken = AccessToken SleepBetter.Personal.GiteeRepo.GiteeAccessToken
let giteeRepo = {
    owner = SleepBetter.Personal.GiteeRepo.GiteeRepoOwner
    repo = SleepBetter.Personal.GiteeRepo.GiteeRepoName
    branch = "master"
}

let page text (firstButton:string option) = 
    html [
        head [
            Meta (Charset "UTF-8")
            Meta (Property ("viewport","width=device-width, initial-scale=1.0, shrink-to-fit=no"))
            // Bootstrap
            InlineHTML """<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.5.0/dist/css/bootstrap.min.css" integrity="sha384-9aIt2nRpC12Uk9gS9baDl411NQApFmC26EwAOH8WgZl5MYYxFfc+NcPb1dKGj7Sk" crossorigin="anonymous">"""

            title %"Sleep Better"
        ]
        body [

            "style" %= "background: #1E1E1E;"

            // Bootstrap
            InlineHTML"""
            <script src="https://cdn.jsdelivr.net/npm/jquery@3.5.1/dist/jquery.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
            <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.0/dist/js/bootstrap.min.js" integrity="sha384-OgVRvuATP1z7JjHLkuOU7Xw704+h835Lr+6QL9UvYjZE3Ipu6Tp75j7Bh/kR0JKI" crossorigin="anonymous"></script>"""


            div [
                "class" %= "container-fluid"

                div ["style" %= "padding: 200px 0 0"]

                div [
                    "class" %= "row"
                    div [
                        "class" %= "col-12"
                        center [
                            h1 [
                                "class" %= "m-auto"
                                "style" %= "color: white;font-size: 400%"
                                Text text
                            ]
                        ]
                    ]
                    
                ]

                if firstButton.IsSome then

                    div ["style" %= "padding: 200px 0 0"]

                    center [
                        a [
                            "href" %= "/inc"
                            "class" %= "btn btn-dark m-auto btn-lg btn-block"
                            "style" %= "height: 300%; font-size: 200%;"
                            Text firstButton.Value
                        ]
                    ]
                


                    div ["style" %= "padding: 50px 0 0"]

                    center [
                        a [
                            "href" %= "/clear"
                            "class" %= "btn btn-dark m-auto btn-lg btn-block"
                            "style" %= "height: 300%; font-size: 200%;"
                            Text "清除"
                        ]
                    ]
            ]
        ]
    ]
    |> HTMLDocument
    |> string

let parseIni (x:string) =
    x.Split '\n'
    |> Array.map (fun x -> 
        let a = x.Split '='
        a.[0].Trim(),a.[1].Trim())
    |> dict

let getSleepBetterItem () =
    let root = Repo.getRoot giteeAccessToken false giteeRepo
    query {
        for i in root do
        where (match i with
                | File (d,_) -> d.path = "SleepBetter.ini"
                | _ -> false)
        select i
        exactlyOne
    }

let downloadDays () =
    getSleepBetterItem ()
    |> Item.downloadString giteeAccessToken
    |> parseIni
    |> fun x -> int x.["Days"]

let uploadDays (days:int) =
    sprintf "Days = %d" days
    |> fun x ->
        getSleepBetterItem ()
        |> Item.updateString giteeAccessToken "更新睡眠记录" x

let homePage () =
    downloadDays ()
    |> fun days ->
        let hint = sprintf "你已经坚持%d天没有熬夜了" days
        let hr,min = SleepBetter.Personal.Plan.myPlan days
        let btn = sprintf "在%02d:%02d之前睡觉" hr min |> Some
        page hint btn
    |> OK

let clearPage () =
    uploadDays 0
    page "小心猝死" None |> OK

let incPage () =
    let days =
        downloadDays ()
        |> (+) 1
    uploadDays days
    page (sprintf "你已经坚持%d天没有熬夜了" days) None |> OK

let routing = 
    statefulForSession >=>
    choose [
        GET >=> choose [
            path "/" >=> request (fun _ -> homePage ())
            path "/inc" >=> request (fun _ -> incPage ())
            path "/clear" >=> request (fun _ -> clearPage ())
        ]
    ]

let cfg = {
    defaultConfig with  
        bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" SleepBetter.Personal.WebServerConfig.Port ]
}

startWebServer cfg routing
