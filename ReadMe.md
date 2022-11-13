# Machine Stream API
## Architecture
This application is running on a DotNet 6.0 Backend, with a SQLLite server saving the data to the messages.db file in the container.

## How to run
To run this, go into the folder MachineStreamBackend and run docker-compose up. This will create the docker container and host it on port 5050.

Alternatively, you can go into the subfolder MachineStreamBackend and run the dockerfile there directly. You will need to map the port 80 of the docker container to a free port on your computer.
## How to use
To query the API, go to 

api/Websocket to get the full list of message payloads received from the websocket connection.

api/Websocket/{machineID} to get the full list of message payloads received for a specific machine.

api/Websocket/latest/{machineID} to get the latest message payload received for a specific machine.

api/Websocket/status/{machineID} to get the latest status received for a specific machine.