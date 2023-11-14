namespace GraphSample

open Avalonia.FuncUI.DSL

[<AutoOpen>]
module GraphPanel =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open AvaloniaGraphControl
    open Avalonia.Controls.Templates
    
    let create (attrs: IAttr<GraphPanel> list): IView<GraphPanel> =
        ViewBuilder.Create<GraphPanel>(attrs)
    
    type GraphPanel with
        static member graph<'t when 't :> GraphPanel>(value:Graph) : IAttr<'t> = 
          AttrBuilder<'t>.CreateProperty<Graph>(GraphPanel.GraphProperty, value, ValueNone)          

        static member itemTemplate<'t when 't :> GraphPanel>(value: IDataTemplate) : IAttr<'t> =
          let getter : ('t -> IDataTemplate) = (fun t -> if t.DataTemplates.Count > 0 then  t.DataTemplates.[0] else null)
          let setter : ('t * IDataTemplate -> unit) = (fun (t,v) -> t.DataTemplates.Clear(); t.DataTemplates.Add(v))
          AttrBuilder<'t>.CreateProperty<IDataTemplate>("itemTemplate", value, ValueSome getter, ValueSome setter, ValueNone)

        static member dataTemplates<'t when 't :> GraphPanel>(value: DataTemplates) : IAttr<'t> =
          let getter : ('t -> DataTemplates) = (fun t -> if t.DataTemplates = null then new DataTemplates() else t.DataTemplates)
          let setter : ('t * DataTemplates -> unit) = (fun (t,v) -> t.DataTemplates.Clear(); t.DataTemplates.AddRange(v))
          AttrBuilder<'t>.CreateProperty<DataTemplates>("DataTemplates", value, ValueSome getter, ValueSome setter, ValueNone)

[<AutoOpen>]
module TextSticker =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open AvaloniaGraphControl
  
    let create (attrs: IAttr<TextSticker> list): IView<TextSticker> =
        ViewBuilder.Create<TextSticker>(attrs)
    
    type TextSticker with
        static member shape<'t when 't :> TextSticker>(value:TextSticker.Shapes) : IAttr<'t> = 
          AttrBuilder<'t>.CreateProperty<TextSticker.Shapes>(TextSticker.ShapeProperty, value, ValueNone)          

        static member text<'t when 't :> TextSticker>(value:string) : IAttr<'t> = 
          AttrBuilder<'t>.CreateProperty<string>(TextSticker.TextProperty, value, ValueNone)          

