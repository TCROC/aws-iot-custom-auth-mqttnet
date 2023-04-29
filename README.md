# aws-iot-custom-auth-mqttnet

built with dotnet version 7.0.203

## Clone Command

```
git clone --recurse-submodules
```

## Run command

```
dotnet run <username> <password> <endpoint> <rootTopic> <authorizer> <transportImplementation> <transport>
```

### Example

```
dotnet run myid123 testpassword123 abc123-ats.iot.us-east-1.amazonaws.com topicaboutcats MyAuthorizer websockets4net websocket
```

Example output:

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