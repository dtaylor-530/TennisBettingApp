open System
open FSharp.Charting
open FSharp.Charting.ChartTypes
open System.Windows.Forms
open TennisBetting
open RegressionSmooth



//let getChart points  atp=  Chart.Line (SmoothLine points , Name = tournaments.getTournamentName(atp))
                                       

[<EntryPoint>]
let main argv = 
    let range =  seq{1L..5L} 

    let arr = [|for atp in range do 
                 let points = TennisBetting.datapoints.getAllPoints |> Seq.filter (fun x -> x.atp = atp) 
                 //let xs = points |> Seq.map (fun x -> x.ratio)
                 let points2 = one.getPoints points  |> Seq.toArray
                 let modPoints = one.SmoothLine points2
                 let n = one.normalizeLine points2 modPoints
                 let mod2Points = modPoints |> Seq.map (fun (x,y) -> (x, y * n))
               //  yield  Chart.Line ( points2 , Name = tournaments.getTournamentName(atp))

                 yield  Chart.Line ( mod2Points , Name = tournaments.getTournamentName(atp))
              |]

    let combine =  Chart.Combine(arr).WithLegend(true)
    Chart.Show combine 
    0


   







