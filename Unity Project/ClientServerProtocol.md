# Tower Defender client/server protocol #

URI Handler needs host id, match id, team id, username

Protocol message arguments are delimited with a pipe (i.e. '|').

| Message ID| Desc 		| Sent by/For	| Arguments | 
| --------- | ----------------- | ------------- | --------- |
| 1	    | game start	| from server, for all | none |
| 2	    | tower built 	| from client, for all | prefab name, position x, position y |
| 3	    | an enemy on friendly side dies | from client, for team | enemy id, team number |
| 4	    | | | |
| 5	    | | | |
| 6	    | | | |
| 7	    | | | |
| 8	    | | | |
| 9	    | | | |
| 10	    | | | |
