protoc.exe --csharp_out=./csharp_out  protos/OuterMessage.proto


protoc.exe -o ./lua_output/OuterMessage.pb  protos/OuterMessage.proto

cd ProtoCS
Proto2Cs.exe
cd ..

pause

