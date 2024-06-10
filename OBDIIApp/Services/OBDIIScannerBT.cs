using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
namespace OBDIIApp.Services
{
    public class OBDIIScannerBT
    {
        private readonly IBluetoothLE ble;
        private readonly IAdapter adapter;
        private readonly List<IDevice> deviceList;

        public OBDIIScannerBT()
        {
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new List<IDevice>();
        }

        public async Task ScanForDevices()
        {
            deviceList.Clear(); // Clear previous list of devices
            adapter.DeviceDiscovered += (s, a) =>
            {
                deviceList.Add(a.Device); // Add discovered device to the list
            };

            await adapter.StartScanningForDevicesAsync(); // Start scanning for devices

            // Scan for a certain period of time (e.g., 10 seconds)
            await Task.Delay(TimeSpan.FromSeconds(10));

            await adapter.StopScanningForDevicesAsync(); // Stop scanning for devices
        }

        public async Task<bool> ConnectToDevice(IDevice device)
        {
            try
            {
                await adapter.ConnectToDeviceAsync(device);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to device: " + ex.Message);
                return false;
            }
        }

        public async Task DisconnectDevice(IDevice device)
        {
            try
            {
                await adapter.DisconnectDeviceAsync(device);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to disconnect from device: " + ex.Message);
            }
        }
    }
}