# aws-iot-custom-auth-mqttnet

built with dotnet version 7.0.203

Uses https://github.com/dotnet/MQTTnet as the mqtt library to communicate with aws iot

## Clone Command

```
git clone --recurse-submodules
```

## Run command

```
dotnet run <username> <password> <endpoint> <rootTopic> <authorizer> <transportImplementation> <transport>
```

## Args

| Arg | Description |
| ----------- | ----------- |
| username | the username / client id of the connecting client |
| password | the password to authenticate against the authorizer with |
| endpoint | the endpoint of aws iot core |
| rootTopic | the root topic that the client will be expecting to subscribe to |
| authorizer | the name of the aws iot authorizer |
| transportImplementation | `dotnet` or `websockets4net` |
| transport | `tcp` or `websocket` |

## Example

```
dotnet run myid123 testpassword123 abc123-ats.iot.us-east-1.amazonaws.com topicaboutcats MyAuthorizer websockets4net websocket
```

## Example output

```
Running mqtt example application!

===================

Args Used

username:                  myid123
password:                  testpassword123
endpoint:                  abc123-ats.iot.us-east-1.amazonaws.com
rootTopic:                 topicaboutcats
authorizer:                MyAuthorizer
transportImplementation:   websockets4net
transport:                 websocket

===================

...
```