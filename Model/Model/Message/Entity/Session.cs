﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NiceET
{

	public sealed class Session : Entity
	{
		private static int RpcId { get; set; }
		private AChannel channel;

		private readonly Dictionary<int, Action<IResponse>> requestCallback = new Dictionary<int, Action<IResponse>>();
		private readonly byte[] opcodeBytes = new byte[2];

		public long LastRecvTime { get; private set; }
		public long LastSendTime { get; private set; }

		public NetworkComponent Network
		{
			get
			{
				return this.GetParent<NetworkComponent>();
			}
		}

		public int Error
		{
			get
			{
				return this.channel.Error;
			}
			set
			{
				this.channel.Error = value;
			}
		}

		public void Awake(AChannel aChannel)
		{
			long timeNow = TimeHelper.Now();
			this.LastRecvTime = timeNow;
			this.LastSendTime = timeNow;

			this.channel = aChannel;
			this.requestCallback.Clear();
			long id = this.Id;
			channel.ErrorCallback += (c, e) =>
			{
				this.Network.Remove(id);
			};
			channel.ReadCallback += this.OnRead;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			this.Network.Remove(this.Id);

			base.Dispose();

			foreach (Action<IResponse> action in this.requestCallback.Values.ToArray())
			{
				action.Invoke(new ErrorResponse { Error = this.Error });
			}

			int error = this.channel.Error;
			if (this.channel.Error != 0)
			{
				Log.Info($"session dispose: {this.Id} ErrorCode: {error}, please see ErrorCode.cs!");
			}

			this.channel.Dispose();

			this.requestCallback.Clear();
		}

		public string RemoteAddress
		{
			get
			{
				return this.channel.RemoteAddress;
			}
		}

		public ChannelType ChannelType
		{
			get
			{
				return this.channel.ChannelType;
			}
		}

		public MemoryStream Stream
		{
			get
			{
				return this.channel.Stream;
			}
		}

		public void OnRead(MemoryStream memoryStream)
		{
			try
			{
				this.Run(memoryStream);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Run(MemoryStream memoryStream)
		{
			int seq = BitConverter.ToInt32(memoryStream.GetBuffer(), Packet.MessageIndex);
			ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);

			memoryStream.Seek(Packet.MinPacketSize + Packet.OpcodeIndex, SeekOrigin.Begin);
			object message;
			try
			{
				object instance = OpcodeTypeComponent.Instance.GetInstance(opcode);
				message = this.Network.MessagePacker.DeserializeFrom(instance, memoryStream);

				if (OpcodeHelper.IsNeedDebugLogMessage(opcode))
				{
					Log.Msg(message);
				}
			}
			catch (Exception e)
			{
				// 出现任何消息解析异常都要断开Session，防止客户端伪造消息
				Log.Error($"opcode: {opcode} {this.Network.Count} {e}, ip: {this.RemoteAddress}");
				this.Error = ErrorCode.ERR_PacketParserError;
				this.Network.Remove(this.Id);
				return;
			}

			RunMessage(opcode, message);
		}

		private void RunMessage(ushort opcode, object message)
		{
			this.LastRecvTime = TimeHelper.Now();

			if (!(message is IResponse response))
			{
				this.Network.MessageDispatcher.Dispatch(this, opcode, message);
				return;
			}

			if (message is IActorResponse)
			{
				this.Network.MessageDispatcher.Dispatch(this, opcode, message);
				return;
			}

			Action<IResponse> action;
			if (!this.requestCallback.TryGetValue(response.RpcId, out action))
			{
				throw new Exception($"not found rpc, response message: {StringHelper.MessageToStr(response)}");
			}
			this.requestCallback.Remove(response.RpcId);

			action(response);
		}

		public ETTask<IResponse> CallWithoutException(IRequest request)
		{
			int rpcId = ++RpcId;
			var tcs = new ETTaskCompletionSource<IResponse>();
			this.requestCallback[rpcId] = (response) =>
			{
				if (response is ErrorResponse)
				{
					tcs.SetException(new Exception($"Rpc error: {MongoHelper.ToJson(response)}"));
					return;
				}

				tcs.SetResult(response);
			};

			request.RpcId = rpcId;
			this.Send(request);
			return tcs.Task;
		}

		public ETTask<IResponse> Call(IRequest request)
		{
			int rpcId = ++RpcId;
			var tcs = new ETTaskCompletionSource<IResponse>();
			this.requestCallback[rpcId] = (response) =>
			{
				if (response is ErrorResponse)
				{
					tcs.SetException(new Exception($"Rpc error: {MongoHelper.ToJson(response)}"));
					return;
				}

				if (ErrorCode.IsRpcNeedThrowException(response.Error))
				{
					tcs.SetException(new Exception($"Rpc error: {MongoHelper.ToJson(response)}"));
					return;
				}

				tcs.SetResult(response);
			};

			request.RpcId = rpcId;
			this.Send(request);
			return tcs.Task;
		}

		public void Reply(IResponse message)
		{
			if (this.IsDisposed)
			{
				throw new Exception("session已经被Dispose了");
			}
			this.Send(message);
		}

		public void Send(IMessage message)
		{
			ushort opcode = OpcodeTypeComponent.Instance.GetOpcode(message.GetType());

			Send(opcode, message);
		}

		public void Send(ushort opcode, object message)
		{
			if (this.IsDisposed)
			{
				throw new Exception("session已经被Dispose了");
			}

			this.LastSendTime = TimeHelper.Now();

			if (OpcodeHelper.IsNeedDebugLogMessage(opcode))
			{
				Log.Msg(message);
			}

			MemoryStream stream = this.Stream;

			int seq = 11;
			stream.Seek(Packet.MinPacketSize + Packet.OpcodeIndex, SeekOrigin.Begin);
			stream.SetLength(Packet.MinPacketSize + Packet.OpcodeIndex);
			this.Network.MessagePacker.SerializeTo(message, stream);
			stream.Seek(0, SeekOrigin.Begin);

			opcodeBytes.WriteTo(0, opcode);
			Array.Copy(opcodeBytes, 0, stream.GetBuffer(), Packet.OpcodeIndex, opcodeBytes.Length);

			this.Send(stream);
		}

		public void Send(MemoryStream stream)
		{
			channel.Send(stream);
		}
	}
}