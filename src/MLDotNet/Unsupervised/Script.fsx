
open System
open System.IO

let file = @"C:\projects\machinelearningdotnet\src\MLDotNet\Unsupervised\userprofiles-toptags.txt"


let headers, observations =
    let raw =
        file
        |> File.ReadAllLines

    let headers = (raw.[0].Split ',').[1..]
    let observations =
        raw.[1..]
        |> Array.map (fun line -> (line.Split ',').[1..])
        |> Array.map (Array.map float)

    headers, observations

printfn "%16s %8s %8s %8s" "Tag Name" "Avg" "Min" "Max"
headers
    |> Array.iteri (fun i name ->
        let col = observations |> Array.map (fun obs -> obs.[i])
        let avg = col |> Array.average
        let min = col |> Array.min
        let max = col |> Array.max

        printfn "%16s %8.1f %8.1f %8.1f" name avg min max)

#r @"..\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"
#load @"..\packages\FSharp.Charting.0.90.14\FSharp.Charting.fsx"
open FSharp.Charting
let labels = ChartTypes.LabelStyle(Interval=0.25)

headers
|> Seq.mapi (fun i name ->
    name,
    observations
    |> Seq.averageBy (fun obs -> obs.[i]))
|> Chart.Bar
|> fun chart -> chart.WithXAxis(LabelStyle=labels)

#load "KMeans.fs"
open KMeans

type Observation = float []
let features = headers.Length
let distance (obs1: Observation) (obs2: Observation) =
    (obs1, obs2)
    ||> Seq.map2 (fun u1 u2 -> pown (u1 - u2) 2)
    |> Seq.sum

let centroidOf (cluster:Observation seq) =
    Array.init features (fun f ->
        cluster
        |> Seq.averageBy (fun user -> user.[f]))

let observations1 =
    observations
    |> Array.map (Array.map float)
    |> Array.filter (fun x -> Array.sum x > 0.)

let (clusters1, classifier1) =
    let clustering = clusterize distance centroidOf
    let k = 5
    clustering observations1 k

clusters1
|> Seq.iter (fun (id, profile) ->
    printfn "CLUSTER %i" id
    profile
    |> Array.iteri (fun i value -> printfn "%16s %.1f" headers.[i] value))

Chart.Combine [
    for (id, profile) in clusters1 ->
        profile
        |> Seq.mapi (fun i value -> headers.[i], value)
        |> Chart.Bar
    ]
|> fun chart -> chart.WithXAxis(LabelStyle=labels)

observations1
|> Seq.countBy (fun obs -> classifier1 obs)
|> Seq.iter (fun (clusterId, count) ->
    printfn "cluster %i: %i elements" clusterId count)
