open System
open FSharp.Charting
open FSharp.Charting.ChartTypes
open System.Windows.Forms
open TennisBetting

let getModifiedPoints (points:seq<datapoints.DataPoint>)=    
     seq{ for point in points do 
                              let m =point.mean
                              let v =point.variance
                              let w = point.weight
                              yield (m*w*100L/v, point.ratio)} |> Seq.toArray

let getLine y (points:seq<datapoints.DataPoint>) i = let y2 = seq{ for point in points do 
                                                                       yield (seq{for (p,m) in y do
                                                                                    let diff =1.0 /(Math.Pow((Math.Abs(point.ratio-m) + i |> float),3.0))
                                                                                    let xx = diff * (p |> float)
                                                                                    yield xx}
                                                        |> Seq.average,point.ratio)
                                                                         }
                                                     [for (i2,i3) in y2 -> (i3, i2)]  
                                                 

[<EntryPoint>]
let main argv = 
    let arr = [|for atp in 5L..15L do 
                    let points = TennisBetting.datapoints.getAllPoints |> Seq.filter (fun x -> x.atp = atp) 
                    let chart= Chart.Line(getLine (getModifiedPoints points) points atp, tournaments.getTournamentName(atp))
                    yield chart|]
    let combine = Chart.Combine(arr).WithLegend(true)
    Chart.Show combine 
    0


   







