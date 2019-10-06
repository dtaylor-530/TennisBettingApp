module common

 let getFileName = if not( System.IO.File.Exists("../../../Data/sample.sqlite")) then 
                    if not( System.IO.File.Exists("../../../TennisBettingApp/Data/sample.sqlite")) then 
                        @"C:\Users\rytal\Documents\Visual Studio 2019\Projects\MasterProject\TennisBettingApp\TennisBettingApp\Data\sample.sqlite"
                    else
                       "../../../TennisBettingApp/Data/sample.sqlite"
                   else
                       "../../../Data/sample.sqlite"
       
 let getFileName2 file = if not( System.IO.File.Exists("../../../Data/"+ file)) then 
                          if not( System.IO.File.Exists("../../../TennisBettingApp/Data/"+ file)) then 
                           @"C:\Users\rytal\Documents\Visual Studio 2019\Projects\MasterProject\TennisBettingApp\TennisBettingApp\Data\"+ file
                          else
                           "../../../TennisBettingApp/Data/"+ file
                         else
                           "../../../Data/"+ file