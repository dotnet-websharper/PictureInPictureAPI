# WebSharper Picture-in-Picture API Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Picture-in-Picture API](https://developer.mozilla.org/en-US/docs/Web/API/Picture-in-Picture_API), enabling seamless integration of Picture-in-Picture (PiP) functionality into WebSharper applications.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**:

   - Contains the F# WebSharper binding for the Picture-in-Picture API.

2. **Sample Project**:
   - Demonstrates how to use the Picture-in-Picture API with WebSharper syntax.
   - Includes a GitHub Pages demo: [View Demo](https://dotnet-websharper.github.io/PictureInPictureAPI/)

## Installation

To use this package in your WebSharper project, add the NuGet package:

```bash
   dotnet add package WebSharper.PictureInPicture
```

## Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/dotnet-websharper/PictureInPictureAPI.git
   cd PictureInPictureAPI
   ```

2. Build the Binding Project:

   ```bash
   dotnet build WebSharper.PictureInPicture/WebSharper.PictureInPicture.fsproj
   ```

3. Build and Run the Sample Project:

   ```bash
   cd WebSharper.PictureInPicture.Sample
   dotnet build
   dotnet run
   ```

4. Open the sample project in your browser to see it in action.

## Example Usage

Below is an example of how to use the Picture-in-Picture API in a WebSharper project:

```fsharp
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

    // Get references to the video element and PiP toggle button
    let video = JS.Document.GetElementById("video") |> As<HTMLVideoElement>
    let pipButton = JS.Document.GetElementById("pipButton") |> As<HTMLButtonElement>

    // Function to toggle Picture-in-Picture mode
    let togglePiPMode () =
        promise {
            try
                let doc = As<Document> <| JS.Document
                if not (isNull doc.PictureInPictureElement) then
                    // Exit Picture-in-Picture mode if it's already active
                    do! doc.ExitPictureInPicture()
                else
                    // Request to enter Picture-in-Picture mode
                    video.RequestPictureInPicture() |> ignore
            with error ->
                Console.Error($"Error toggling Picture-in-Picture mode: {error}")
        }

    // Set up event listeners to update button text when PiP mode changes
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
```

This example demonstrates how to toggle Picture-in-Picture mode for a video element, handling user interaction and updating UI elements accordingly.

## Important Considerations

- **Browser Support**: The Picture-in-Picture API is not supported on all browsers. Check the [MDN Picture-in-Picture API](https://developer.mozilla.org/en-US/docs/Web/API/Picture-in-Picture_API) for compatibility details.
- **User Gesture Requirement**: Some browsers require user interaction (e.g., a button click) before allowing Picture-in-Picture mode.
- **Limitations**: Only one video can be in Picture-in-Picture mode at a time.
