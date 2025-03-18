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

    let HTMLVideoElement =
        Class "HTMLVideoElement"
        |=> Inherits T<HTMLElement>
        |+> Instance [
            "disablePictureInPicture" =@ T<bool> 

            "requestPictureInPicture" => T<unit> ^-> T<Promise<_>>[PictureInPictureWindow] 

            "onenterpictureinpicture" =@ PictureInPictureEvent ^-> T<unit>
            |> WithSourceName "onEnterPictureInPicture"
            "onleavepictureinpicture " =@ PictureInPictureEvent ^-> T<unit>
            |> WithSourceName "onLeavePictureInPicture"
        ]

    let Document =
        Class "Document"
        |=> Inherits T<Dom.Node>
        |+> Instance [
            "pictureInPictureEnabled" =? T<bool> 
            "pictureInPictureElement" =? T<Dom.Element> 

            "exitPictureInPicture" => T<unit> ^-> T<Promise<_>>[T<unit>] 
        ]

    let ShadowRoot =
        Class "ShadowRoot"
        |=> Inherits T<Dom.DocumentFragment>
        |+> Instance [
            "pictureInPictureElement" =? T<Dom.Element> 
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.PictureInPicture" [
                PictureInPictureWindow
                PictureInPictureEventInit
                PictureInPictureEvent
                HTMLVideoElement
                Document
                ShadowRoot
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
