namespace System

 module Seq =

    /// Returns a sequence that yields chunks of length n.
    /// Each chunk is returned as a list.
    let split length (xs: seq<'T>) =
        let rec loop xs =
            [
                yield Seq.truncate length xs |> Seq.toList
                match Seq.length xs <= length with
                | false -> yield! loop (Seq.skip length xs)
                | true -> ()
            ]
        loop xs
   
    let meanAndVariance numSeq =   let mean = numSeq |> Seq.average
                                   let variance = numSeq |> Seq.averageBy (fun x -> Math.Pow(x - mean,2.0))
                                   (mean, variance)
    
    let gencutTailRecursive n input =
        let rec gencut cur acc = function
            | hd::tl when cur < n ->
                gencut (cur+1) (hd::acc) tl
            | rest -> (List.rev acc), rest //need to reverse accumulator!
        gencut 0 [] input
