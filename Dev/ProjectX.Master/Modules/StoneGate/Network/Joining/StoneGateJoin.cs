using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RedLoader;
using SonsSdk;
using SonsSdk.Networking;
using ProjectX.Master.Modules.StoneGate.Mono;
using ProjectX.Master.Modules.StoneGate.Objects;
using ProjectX.Master.Modules.StoneGate.Saving;
using ProjectX.Master.Modules.StoneGate.Tools;
using UdpKit;

namespace ProjectX.Master.Modules.StoneGate.Network.Joining
{
	// Token: 0x0200001C RID: 28
	internal class StoneGateJoin : EventBase<StoneGateJoin>
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00006530 File Offset: 0x00004730
		protected override void ReadMessageServer(UdpPacket packet, BoltConnection fromConnection)
		{
			Misc.Msg($"Received StoneGateJoin REQUEST from {fromConnection}", true);
			string text = packet.ReadString();
			bool flag = text != "REQUEST_SYNC";
			if (flag)
			{
				RLog.Error("[StoneGateJoin] [ReadMessageServer] Invalid request type: " + text);
			}
			else
			{
				Misc.Msg("[StoneGateJoin] [ReadMessageServer] Received: " + text, true);
				try
				{
					bool flag2 = EventBase<StoneGateJoin>.Instance == null;
					if (flag2)
					{
						Misc.Msg("[StoneGateJoin] [ReadMessageServer] UniqueIdSync.Instance is null", true);
						this.SendEmptyResponse(fromConnection);
					}
					else
					{
						string text2 = packet.ReadString();
						bool flag3 = string.IsNullOrEmpty(text2);
						if (flag3)
						{
							Misc.Msg("[StoneGateJoin] [ReadMessageServer] toPlayerSteamId is null or empty", true);
							this.SendEmptyResponse(fromConnection);
						}
						else
						{
							HashSet<StoneGateStoreMono> allStoneGateStoreMono = Gates.GetAllStoneGateStoreMono();
							bool flag4 = allStoneGateStoreMono == null;
							if (flag4)
							{
								Misc.Msg("[StoneGateJoin] [ReadMessageServer] hashSetStoreMono is null", false);
								this.SendEmptyResponse(fromConnection);
							}
							else
							{
								bool flag5 = allStoneGateStoreMono.Count <= 0;
								if (flag5)
								{
									Misc.Msg("[StoneGateJoin] [ReadMessageServer] hashSetStoreMono is empty", false);
									this.SendEmptyResponse(fromConnection);
								}
								else
								{
									foreach (StoneGateStoreMono stoneGateStoreMono in allStoneGateStoreMono)
									{
										try
										{
											Misc.Msg("[StoneGateJoin] [ReadMessageServer] Sending StoneGateJoin RESPONSE for gate", true);
											int childIndex = stoneGateStoreMono.GetChildIndex();
											ProjectX.Master.Modules.StoneGate.Saving.Manager.GatesManager.GatesModData saveData = stoneGateStoreMono.GetSaveData();
											CreateGateParent.RotateMode rotateMode = CreateGateParent.RotateModeFromString(saveData.Mode);
											EventBase<HostEvents>.Instance.SendHostEvent(HostEvents.HostEvent.CreateStoneGate, saveData.RotationGoName, rotateMode, saveData.FloorBeamName, saveData.TopBeamName, saveData.RockWallName, saveData.ExtraPillarName, childIndex);
										}
										catch (Exception ex)
										{
											Misc.Msg("[StoneGateJoin] Error processing item: " + ex.Message + "\n" + ex.StackTrace, true);
										}
									}
								}
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Misc.Msg("[StoneGateJoin] [ReadMessageServer] Error handling request: " + ex2.Message, true);
					this.SendEmptyResponse(fromConnection);
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006794 File Offset: 0x00004994
		private void SendEmptyResponse(BoltConnection connection)
		{
			try
			{
				Packets.EventPacket eventPacket = base.NewPacket(32, connection);
				eventPacket.Packet.WriteBool(false);
				eventPacket.Packet.WriteString("EMPTY_SYNC_RESPONSE");
				base.Send(eventPacket);
				Misc.Msg("[StoneGateJoin] [SendEmptyResponse] Sent empty response instead", true);
			}
			catch (Exception ex)
			{
				Misc.Msg("[StoneGateJoin] [SendEmptyResponse] Error sending empty response: " + ex.Message, true);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006810 File Offset: 0x00004A10
		protected override void ReadMessageClient(UdpPacket packet, BoltConnection _)
		{
			bool flag = !packet.ReadBool();
			if (flag)
			{
				bool flag2 = packet.ReadString() == "EMPTY_SYNC_RESPONSE";
				if (flag2)
				{
					Misc.Msg("[StoneGateJoin] [ReadMessageClient] Received StoneGateJoin RESPONSE: Empty response received", true);
				}
				else
				{
					Misc.Msg("[StoneGateJoin] [ReadMessageClient] Received StoneGateJoin RESPONSE: Something went wrong!", true);
					SonsTools.ShowMessage("Something went wrong while syncing the initial data with server, please rejoin", 5f);
				}
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00006870 File Offset: 0x00004A70
		private void SendServerResponse()
		{
			Misc.Msg("Sending StoneGateJoin REQUEST", true);
			try
			{
				bool flag = !BoltNetwork.isRunning;
				if (flag)
				{
					Misc.Msg("[StoneGateJoin] [SendServerResponse] BoltNetwork is not running", true);
				}
				else
				{
					bool flag2 = BoltNetwork.server == null;
					if (flag2)
					{
						Misc.Msg("[StoneGateJoin] [SendServerResponse] BoltNetwork.server is null", true);
					}
					else
					{
						bool isServer = BoltNetwork.isServer;
						if (isServer)
						{
							Misc.Msg("[StoneGateJoin] [SendServerResponse] Not a client, skipping request", true);
						}
						else
						{
							Packets.EventPacket eventPacket = base.NewPacket(64, 6);
							eventPacket.Packet.WriteString("REQUEST_SYNC");
							string localPlayerSteamId = Misc.GetLocalPlayerSteamId();
							bool flag3 = string.IsNullOrEmpty(localPlayerSteamId);
							if (flag3)
							{
								RLog.Error("[StoneGate] [StoneGateJoin] [SendServerResponse] SteamID is null or empty", new object[] { true });
							}
							else
							{
								eventPacket.Packet.WriteString(Misc.GetLocalPlayerSteamId());
								base.Send(eventPacket);
								Misc.Msg("[StoneGateJoin] [SendServerResponse] Successfully sent request to server", true);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Misc.Msg("[StoneGateJoin] [SendServerResponse] Error sending request: " + ex.Message, true);
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006984 File Offset: 0x00004B84
		public void RequestInfoFromServer()
		{
			this.SendServerResponse();
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000698D File Offset: 0x00004B8D
		public override string Id
		{
			get
			{
				return "StoneGate_StoneGateJoin";
			}
		}
	}
}


