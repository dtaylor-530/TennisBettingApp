module common

 let getFileName = if not( System.IO.File.Exists("../../../Data/sample.sqlite")) then 
                      "../../../TennisBettingApp/Data/sample.sqlite"
                   else
                      "../../../Data/sample.sqlite"


