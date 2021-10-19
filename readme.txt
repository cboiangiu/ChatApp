ChatApp
=======


Architecture overview
=====================

ChatApp
│
└───App
│   │   Consumes the Display service using DI
│   │
│   └───Client
│   │   │   Implements the Client RunningStrategy
│   │   │   Uses the Websocket.Client library
│   │
│   └───Server
│       │   Implements the Server RunningStrategy
│       │   Uses the Fleck library
│
└───Display service
    │   Implements the Display interface
    │   Uses System.Console for IO


Details:
--------

To get the port status, active TCP Listeners are checked to see if the port is in use.
The Microsoft .NET DI framework is used to provide the Display service as a singleton.
The App class uses the strategy pattern to separate the Client and Server behaviour.
The client username is provided via the "userName" header.
Data serialization/deserialization is handeled using System.Text.Json.
