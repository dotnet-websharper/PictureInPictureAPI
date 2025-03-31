namespace WebSharper.PictureInPicture

open WebSharper
open WebSharper.JavaScript

[<JavaScript; AutoOpen>]
module Extensions =

    type Dom.ShadowRoot with

        [<Inline "$this.pictureInPictureElement">]
        member this.PictureInPictureElement with get(): Dom.Element = X<Dom.Element>

    type HTMLVideoElement with

        [<Inline "$this.disablePictureInPicture">]
        member this.DisablePictureInPicture with get(): bool = X<bool>
        [<Inline "$this.disablePictureInPicture = $value">]
        member this.DisablePictureInPicture with set(value: bool) = ()

        [<Inline "$this.requestPictureInPicture()">]
        member this.RequestPictureInPicture() : Promise<PictureInPictureWindow> =
            X<Promise<PictureInPictureWindow>>

        [<Inline "$this.onenterpictureinpicture">]
        member this.OnEnterPictureInPicture with get(): (PictureInPictureEvent -> unit) = ignore
        [<Inline "$this.onenterpictureinpicture = $callback">]
        member this.OnEnterPictureInPicture with set(callback: PictureInPictureEvent -> unit) = ()

        [<Inline "$this.onleavepictureinpicture">]
        member this.OnLeavePictureInPicture with get(): (PictureInPictureEvent -> unit) = ignore
        [<Inline "$this.onleavepictureinpicture = $callback">]
        member this.OnLeavePictureInPicture with set(callback: PictureInPictureEvent -> unit) = ()
