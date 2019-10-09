open System
open FSharp.Charting
open FSharp.Charting.ChartTypes
open System.Windows.Forms
open TennisBetting


[<EntryPoint>]
let main argv = 
    
    let profits = profit.getProfits
    let arr =  Seq.scan (fun x a-> x + a) 0.0 profits |> Seq.mapi (fun i x ->(i,x)) 

    let combine =  Chart.Line( arr).WithLegend(true)
    Chart.Show combine 
    0


   







