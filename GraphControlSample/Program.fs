namespace GraphSample
open System
open Elmish
open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Elmish
open AvaloniaGraphControl
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.Controls.Templates

module GraphApp =
    type [<CLIMutable>] Node = {Label:string}
    let order (a,b) = if a >= b then a,b else b,a

    let deviceRels =
      [
        "gnodeb_id", ["cell_id"; "rmod"; "bbmod"]
        "enodeb_id", ["cell_id"; "rmod"; "bbomd"]
        "cell_id", ["sector_id"]
        "rmod",["sfp"]
        "bbmod",["sfp"; "smod"; "cabinet"]
        "smod", ["cabinet"]
      ]

    let sampleGraph = new AvaloniaGraphControl.Graph()
    sampleGraph.Edges.Clear()
    deviceRels |> List.collect (fun (r,xs) -> xs |> List.map(fun b -> order(r,b))) |> List.distinct
    |> List.iter (fun (a,b) -> 
      sampleGraph.Edges.Add(AvaloniaGraphControl.Edge(
        {Label=a},
        {Label=b},
        tailSymbol = AvaloniaGraphControl.Edge.Symbol.None,
        headSymbol = AvaloniaGraphControl.Edge.Symbol.None)))

    type State = { count : int ; graph:AvaloniaGraphControl.Graph}
    let init = { count = 0; graph=sampleGraph}

    type Msg = Increment | Decrement | Reset

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | Reset -> init
    
    let view (state: State) (dispatch) =
      Viewbox.create [
        Viewbox.child (
          GraphPanel.create [
             GraphPanel.dataTemplates (
               let ds = DataTemplates()
               ds.AddRange(
                 [
                   DataTemplateView<Node>.create (fun (data:Node) ->
                     TextSticker.create [
                       TextSticker.shape TextSticker.Shapes.Ellipse
                       TextSticker.text data.Label
                     ])          
                 ])
               ds
               )            
             GraphPanel.graph state.graph
            //GraphPanel.itemTemplate (
            //      DataTemplateView<Node>.create (fun (data:Node) ->
            //        TextSticker.create [
            //          TextSticker.shape TextSticker.Shapes.Ellipse
            //          TextSticker.text data.Label
            //        ])                        
            //)



            ]
            )            
          ]

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "BasicTemplate"
        base.Background <- Brushes.White
        base.Width <- 400.0
        base.Height <- 400.0
        Program.mkSimple (fun () -> GraphApp.init) GraphApp.update GraphApp.view
        |> Program.withHost this
        |> Program.run

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
            
