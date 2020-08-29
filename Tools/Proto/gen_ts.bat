
pbjs -t static-module -w commonjs -o ts_out/OuterMessage.js  protos/OuterMessage.proto

cd ts_out
pbts -o OuterMessage.d.ts  OuterMessage.js
cd ..