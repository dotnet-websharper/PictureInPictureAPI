namespace WebSharper.PictureInPicture.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.PictureInPicture

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let video = JS.Document.GetElementById("video") |> As<HTMLVideoElement>
    let pipButton = JS.Document.GetElementById("pipButton") |> As<HTMLButtonElement>

    let togglePiPMode () =
        promise {
            try
                let doc = As<Document> <| JS.Document
                if not (isNull doc.PictureInPictureElement) then
                    do! doc.ExitPictureInPicture()
                else
                    video.RequestPictureInPicture() |> ignore
            with error ->
                Console.Error($"Error toggling Picture-in-Picture mode: {error}")
        }

    let setupPiPEvents () =
        video.AddEventListener("enterpictureinpicture", fun (_: Dom.Event) ->
            pipButton.InnerText <- "Exit PiP Mode"
        )
        video.AddEventListener("leavepictureinpicture", fun (_: Dom.Event) ->
            pipButton.InnerText <- "Toggle PiP Mode"
        )

    [<SPAEntryPoint>]
    let Main () =

        IndexTemplate.Main()
            .PageInit(fun () -> 
                setupPiPEvents()
                pipButton.AddEventListener("click", fun (_: Dom.Event) ->
                    async {
                        do! togglePiPMode() |> Promise.AsAsync
                    } |> Async.StartImmediate
                )
            )
            .Doc()
        |> Doc.RunById "main"
