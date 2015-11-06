# Tower Defender client/server protocol #

URI Handler needs host id, match id, team id, username

Protocol message arguments are delimited with a pipe (i.e. '|').

| Message ID| Desc 		| Sent by/For	| Arguments | 
| --------- | ----------------- | ------------- | --------- |
| 1	    | join game	| from client, for all | username, team id, userid (can be randomly generated) |
| 2	    | tower built 	| from client, for all | prefab name, position x, position y |
| 3	    | enemy dies in team territory | from client, for all | enemy id, team id |
| 4	    | health update | from client, for all | userid, team id, health |
| 5	    | send enemy to opposing team | from client, for all | enemy id, prefab name, team id to attack |
| 6	    | chat message | from client, for all | username, message content, team id (1/2 for team communication, 0 for all) |
| 7	    | | | |
| 8	    | | | |
| 9	    | | | |
| 10	    | | | |
