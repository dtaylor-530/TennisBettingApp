namespace TennisBetting

module profit =
 open odds
 open System.Linq.Expressions
 open System

 let getProfits = 
    let results = results.GetData
    let odds2 = odds.getOdds() |> Seq.toArray

    let q =query{ for x in results do
                  leftOuterJoin o in odds2  on (x.link = o.link) into result
                  select (x,result)
                 }
    let combine var (o:Odds2) =
       try
            // favourite
         let yo=if o.A_odd<o.B_odd then o.A_name else o.B_name
         let sum2 =if (o.amount > 0L) then
                     if var.winner.Equals( o.A_name) && yo .Equals(o.A_name) then
                                            ((o.A_odd-100L))*o.amount
                     elif var.winner.Equals( o.A_name) && yo.Equals(o.B_name) then
                                            -o.amount
                     elif var.winner.Equals( o.B_name) && yo.Equals(o.B_name) then
                                            ((o.B_odd-100L) )*o.amount
                     elif var.winner.Equals( o.B_name) && yo.Equals(o.A_name) then
                                            -o.amount
                     else 
                                           failwith "error"
                   else
                     0L
         printfn "Amount: %s" (sum2 |> string)
         sum2
       with
         | _Exception -> 0L

   // 
    let quer var (io:seq<Odds2>) =  query { for o in io do  
                                            where (not(o.Equals(null)))
                                            select (combine var o)}

    let yx = seq { 
                    for (var, io) in q do 
                      
                      let ys = try 
                                ((quer var io) |> Seq.toArray)
                               with
                                 | _Exception -> [|0L|]
                      yield ys
                  }
                  //|> Seq.scan(fun a  b -> a+b) |> List.tail
    let sum = yx |> Seq.map ( fun op ->  Array.sum op) |> Seq.sum
    sum
                 //for gr in record do
                 //                        leftOuterJoin rc in record
                 //                          on (gr.indx = rc.indx + 1L) into result
                 //                        yield (gr,result |> Seq.tryHead)  }