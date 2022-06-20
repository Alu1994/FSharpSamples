module MatthewsViewEngine

open Giraffe
open Giraffe.ViewEngine

let createTable = 
    let options = System.IO.EnumerationOptions(RecurseSubdirectories = true)
    let files = System.IO.Directory.EnumerateFiles(path = ".", searchPattern = "*", enumerationOptions = options) |> Seq.toArray

    let fileInfos = [
        for file in files do
            System.IO.FileInfo file
    ]

    html [] [
        head [] [
            link [ _rel "stylesheet"; _href "https://cdn.jsdelivr.net/npm/bulma@0.8.2/css/bulma.min.css" ]
            title [] [ str "Giraffe Demo" ]
        ]
        body [] [
            section [ _class "section" ] [
                h1 [ _class "title" ] [ str "Files" ]
                table [ _class "table" ] [
                    thead[] [
                        tr [] [
                            th [] [ Text "Name" ]
                            th [] [ Text "Length" ]
                        ]
                    ]
                    tbody[] [
                        for fi in fileInfos do
                            tr [] [
                                td [] [ str fi.Name ]
                                td [] [ str $"{fi.Length}" ]
                            ]
                    ]
                ]
            ]
        ]
    ]