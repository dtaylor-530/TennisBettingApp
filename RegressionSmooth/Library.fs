namespace RegressionSmooth

module one =
 open System

 let getPoints (points:seq<DataPoint>)=    
     seq{ for point in points do 
                              let m =point.mean |> float
                              let v =point.variance |> float
                              let w = point.weight |> float
                              let r = point.ratio |> float
                              yield (r, m*w * 100.0  / v )}

 let AdjustPoint y (ratio:float)=seq{for (m,p) in y do  
                                     let diff =(Math.Pow((Math.Abs(ratio-m)+1.0),2.0))
                                     yield  (p |> float) /diff
                                   } |> Seq.average 

//let SmoothLine points = let xy = points |> Seq.sumBy (fun (x,y)->  AdjustPoint points x)
//                        let yu = points |> Seq.sumBy (fun (x,y)-> y)
//                        xy / yu

 let normalizeLine points points2= let xy = points |> Seq.sumBy (fun (x,y)-> y)
                                   let yu = points2 |> Seq.sumBy (fun (x,y)-> y)
                                   let div = xy* 1.0 / yu
                                   div

 let SmoothLine points = (points:seq<float*float>) |> Seq.map (fun (x,y)->  (x, AdjustPoint points x))

 let AdjustToRatio points points2 =  let sumPoints = points |> Seq.sum
                                     let ratio = sumPoints / Seq.sum points2
                                     points |> Seq.map(fun x -> x / ratio)
