namespace WebSharper.PictureInPicture

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let PictureInPictureWindow =
        Class "PictureInPictureWindow"

    let PictureInPictureEventInit = 
        Pattern.Config "PictureInPictureEventInit" {
            Required = []
            Optional = [
                "pictureInPictureWindow", PictureInPictureWindow.Type
            ]
        }

    let PictureInPictureEvent =
        Class "PictureInPictureEvent"
        |=> Inherits T<Dom.Event>
        |+> Static [
            Constructor (T<string>?eventType * !?PictureInPictureEventInit?eventInitDict)
        ]
        |+> Instance [
            "pictureInPictureWindow" =? PictureInPictureWindow 
        ]

    PictureInPictureWindow
    |=> Inherits T<Dom.EventTarget>
    |+> Instance [
        "width" =? T<int> 
        "height" =? T<int> 
        
        "onresize" =@ PictureInPictureEvent ^-> T<unit>
        |> WithSourceName "OnResize"
    ]
    |> ignore
        
    let Assembly =
        Assembly [
            Namespace "WebSharper.PictureInPicture" [
                PictureInPictureWindow
                PictureInPictureEventInit
                PictureInPictureEvent
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
