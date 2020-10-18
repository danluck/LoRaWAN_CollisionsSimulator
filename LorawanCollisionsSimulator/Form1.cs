﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LorawanCollisionsSimulator
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			checkBoxIsConfirmed.Checked = Settings.IsConfirmed;
		}

		private void buttonDoEmulation_Click(object sender, EventArgs e)
		{
			// Чтение параметров с формы в настройки
			Settings.EndNodesCount = (uint)numericUpDownEndNodesCount.Value;
			Settings.IsConfirmed = checkBoxIsConfirmed.Checked;

			Console.WriteLine("Settings.EndNodesCount={0}", Settings.EndNodesCount);
			Console.WriteLine("Settings.PacketsPerHour={0}", Settings.PacketsPerHour);
			Console.WriteLine("Settings.IsConfirmed={0}", Settings.IsConfirmed);

			CreateEndNodes();
			EndNodesCollisionFinder.DoCollisionFind(_endNodes);
			ShowEndNodesTransmitTimes();

			CreateGateway();
			ShowGatewayTxTime();
		}

		private void CreateEndNodes()
		{
			_endNodes = new EndNode[Settings.EndNodesCount];
			for (uint i = 0; i < _endNodes.Length; i++)
			{
				_endNodes[i] = new EndNode();
			}
		}

		private void ShowEndNodesTransmitTimes()
		{
			for (uint i = 0; i < _endNodes.Length; i++)
			{
				Console.WriteLine("Node {0}", i);
				var transmitTimes = _endNodes[i].GetTransmissionLog();
				for (uint j = 0; j < transmitTimes.Length; j++)
				{
					Console.WriteLine("[ch={0}][{1}..{2}][gw={3}][c={4}]",
						transmitTimes[j].ChannelNumber,
						transmitTimes[j].StartMs,
						transmitTimes[j].EndMs,
						transmitTimes[j].IsPacketCanBeListenByGateway,
						transmitTimes[j].IsPacketCollisionsWithOtherEndNodes);
				}
				Console.WriteLine("");
			}
		}

		private void CreateGateway()
		{
			_gateway = new Gateway();
		}
		private void ShowGatewayTxTime()
		{
			var gatewayTransmissionLog = _gateway.GetGatewayTx(_endNodes);
			Console.WriteLine("gatewayTransmissionLog.Count={0}",
				gatewayTransmissionLog.Count);
		}

		private IEndNode[] _endNodes;
		private Gateway _gateway;
	}
}
